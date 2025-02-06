using UnityEngine;

public class PlayerKMS : MonoBehaviour
{
    // ���� �ӽ�
    private enum PlayerState { Idle, Transitioning, Riding }
    private PlayerState currentState = PlayerState.Idle;

    // ������Ʈ ĳ��
    private MeshRenderer meshRenderer;
    public IInputHandler currentInput;
    public IMovement currentMovement;

    // ��ȣ �ۿ� ����
    private InteractableObject currentInteractableObject;
    [Header("��ȣ�ۿ� ����")]
    public float interactionRange = 2f;
    public KeyCode interactKeyCode = KeyCode.G;

    // ������ �̵� ����
    [Header("������ �̵� ����")]
    public float transitionDuration = 1f;
    public float jumpHeight = 3f;
    public float mountThreshold = 0.5f;
    private Vector3 startPosition;
    private float transitionTime = 0f;
    private InteractableObject targetObject = null;
    private Vector3 lastKnownMountPoint;

    [Header("�ð� ����")]
    public float timeScale = 0.01f;
    private bool hasSlowedTime = false;

    [Space(10)]
    public sliderM miniGame;
    public GameObject currentObjectPrefab;

    private void Awake()
    {
        // ������Ʈ ĳ��
        meshRenderer = GetComponent<MeshRenderer>();
        currentMovement = GetComponent<IMovement>();
        currentInput = GetComponent<IInputHandler>();
    }

    private void OnEnable()
    {
        sliderM.OnShutdown += ResetTimeScale;
    }

    private void OnDisable()
    {
        sliderM.OnShutdown -= ResetTimeScale;
    }

    private void Update()
    {
        // Transitioning ������ ���� �Է��� �����Ѵ�.
        if (currentState == PlayerState.Transitioning)
        {
            UpdateTransition();
        }
        else
        {
            // Idle�̳� Riding ������ ���� �׻� ���ͷ��� �Է��� ó���ϵ��� ��
            HandleInput();

            // Riding ���¶�� Riding ���� �߰� ������ ó��
            if (currentState == PlayerState.Riding)
            {
                HandleRiding();
            }
        }
        Debug.Log("���̵� ���� : " + PlayerState.Riding);
        Debug.Log("���õ� ������ : " + currentObjectPrefab);
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Riding && currentObjectPrefab != null)
        {
            //transform.position = targetObject.mountPoint.position;
            //transform.rotation = currentInteractableObject.transform.rotation;
            HandleRidingMovement();
        }
        else if (currentState == PlayerState.Idle)
        {
            HandlePlayerMovement();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKey(interactKeyCode))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        InteractableObject nearestObject = CheckForInteractableObjects();

        if ((currentInteractableObject != null && nearestObject != null && nearestObject != currentInteractableObject) ||
            (currentInteractableObject == null && nearestObject != null))
        {
            StartTransition(nearestObject);
        }
    }

    private void StartTransition(InteractableObject target)
    {
        ExitObject(); // ���� ������Ʈ���� ������

        currentState = PlayerState.Transitioning;
        targetObject = target;
        startPosition = transform.position;
        transitionTime = 0f;
        lastKnownMountPoint = target.mountPoint.position;

        // �̵� �߿��� �Է°� �̵��� ��Ȱ��ȭ
        currentInput = null;
        currentMovement = null;

    }

    private void UpdateTransition()
    {
        // targetObject�� ������ ���¸� Idle�� ��ȯ�ϰ� �Լ� ����
        if (targetObject == null)
        {
            currentState = PlayerState.Idle;
            return;
        }

        // ��ȯ ���� �ð� ������Ʈ �� ���� ���� ���
        transitionTime += Time.deltaTime;
        float normalizedTime = transitionTime / transitionDuration;

        // ��ǥ ��ġ (mountPoint) ��������
        Vector3 targetPosition = targetObject.mountPoint.position;
        lastKnownMountPoint = targetPosition;

        // ���� ������ ���� ���� ���� ��� (���� �Լ��� �̿��� �ε巯�� ���/�ϰ� ȿ��)
        float height = Mathf.Sin(normalizedTime * Mathf.PI) * jumpHeight;
        // ���� ��ġ���� ��ǥ ��ġ�� ���� �����ϰ�, ����(height) �������� ���Ͽ� ���� ��ġ ���
        Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, normalizedTime) + Vector3.up * height;
        transform.position = currentPosition;

        // ��ǥ ���� ��� ��, �ش� ������ ���ϵ��� �ε巯�� ȸ�� ���� ����
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // ��ǥ ��ġ���� �Ÿ� ���
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // �ִ� ���̿� �������� �� Ÿ�ӽ������� ������ ���� (���⼭�� normalizedTime�� 0.5 ��ó�� ��)
        // 0.05f�� ��� ������ �ξ� 0.45 ~ 0.55 �������� ����
        if (!hasSlowedTime && Mathf.Abs(normalizedTime - 0.5f) < 0.05f)
        {
            Time.timeScale = timeScale;
            hasSlowedTime = true;
            // �̴ϰ��� ����
            miniGame.OpenCanvas();
            // �̺�Ʈ�� �̴ϰ����� ������ Ÿ�ӽ������� �ǵ��ƿ�.
        }

        // ��ȯ ������ �Ϸ�Ǿ��ų� (normalizedTime >= 1.0f)
        // ��ǥ�� ����� ����������� (distanceToTarget < mountThreshold) ��ȯ �Ϸ� ó��
        if (normalizedTime >= 1.0f || distanceToTarget < mountThreshold)
        {
            CompleteTransition();
            hasSlowedTime = false;
        }
    }

    private void CompleteTransition()
    {
        if (targetObject != null)
        {
            transform.position = lastKnownMountPoint;
            transform.rotation = targetObject.mountPoint.rotation;

            EnterObject(targetObject);
        }

        //currentState = PlayerState.Idle;
        targetObject = null;
    }

    private void EnterObject(InteractableObject interactableObject)
    {
        // 1. ���� ������ ����
        if (currentObjectPrefab != null)
        {
            Destroy(currentObjectPrefab);
            currentObjectPrefab = null;
        }

        // 2. �÷��̾� �޽� ������ ��Ȱ��ȭ
        meshRenderer.enabled = false;

        // ������ Ÿ�� �ִ� ������Ʈ���� ������ ó��
        if (currentInteractableObject != null)
        {
            currentInteractableObject.gameObject.SetActive(true);
        }

        // 3. ���ο� ������Ʈ ����
        currentInteractableObject = interactableObject;

        // 4. ���ο� ������Ʈ Ÿ�� (������ ����)
        Ride(currentInteractableObject);

        // 5. currentMovement�� currentInput�� �����տ��� ��������
        currentMovement = currentObjectPrefab.GetComponentInChildren<IMovement>();
        currentInput = currentObjectPrefab.GetComponentInChildren<IInputHandler>();

        // 6. ���� ������Ʈ
        currentState = PlayerState.Riding;

        transform.SetParent(currentObjectPrefab.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void ExitObject()
    {
        transform.SetParent(null);

        meshRenderer.enabled = true;
        // Ÿ�� �ִ� ������Ʈ�� �ִٸ�, �ش� ������Ʈ���� ����
        if (currentInteractableObject != null)
        {
            // ���� ���� ��ġ�� ȸ���� ������ ��ġ�� ȸ������ ����
            currentInteractableObject.transform.position = currentObjectPrefab.transform.position;
            currentInteractableObject.transform.rotation = currentObjectPrefab.transform.rotation;

            // ������ Ÿ�� �ִ� ������Ʈ �ٽ� Ȱ��ȭ
            currentInteractableObject.gameObject.SetActive(true);

            // currentInteractableObject �ʱ�ȭ
            currentInteractableObject = null;

            // ���� ������ ����
            if (currentObjectPrefab != null)
            {
                Destroy(currentObjectPrefab);
                currentObjectPrefab = null;
            }

            // �÷��̾��� �⺻ �̵� �� �Է� ��Ʈ�ѷ��� ����
            currentMovement = GetComponent<IMovement>();
            currentInput = GetComponent<IInputHandler>();
        }


    }

    private void Ride(InteractableObject target)
    {
        // Ÿ�� ��Ȱ��ȭ
        target.gameObject.SetActive(false);

        //// ���ο� ������ ����
        //Vector3 spawnPosition = target.mountPoint.position;
        //Quaternion spawnRotation = target.mountPoint.rotation;
        // ���ο� ������ ����
        Vector3 spawnPosition = target.transform.position;
        Quaternion spawnRotation = target.transform.rotation;

        // �̴ϰ��� ����� ���� �����Ǵ� �������� �޶����
        currentObjectPrefab = Instantiate(SelectPrefab(target),
                                         spawnPosition + new Vector3(0,0.1f,0),
                                         spawnRotation);
    }

    private InteractableObject CheckForInteractableObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
        InteractableObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            InteractableObject interactable = hitCollider.GetComponent<InteractableObject>();

            if (interactable != null && interactable != currentInteractableObject &&
                !(currentObjectPrefab != null && interactable == currentObjectPrefab.GetComponent<InteractableObject>()))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = interactable;
                }
            }
        }

        return closestObject;
    }

    private void HandleRiding()
    {
        // Riding ���¿����� �߰����� ���� (��: Ư�� �ִϸ��̼� ���)
    }

    private void HandleRidingMovement()
    {
        if (currentInput != null && currentMovement != null)
        {
            Debug.Log("��ǲ,�����Ʈ�� ������");
            Vector3 moveDirection = currentInput.HandleInput();
            currentMovement.Move(moveDirection);
        }
    }

    private void HandlePlayerMovement()
    {
        if (currentInput != null && currentMovement != null)
        {
            Vector3 moveDirection = currentInput.HandleInput();
            currentMovement.Move(moveDirection);
        }
    }

    // �̺�Ʈ�� ȣ��� �� ����� �޼���
    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
        Debug.Log("Time scale reset to 1f.");
    }

    // �̴ϰ����� ����� ���� ������ ����
    private GameObject SelectPrefab(InteractableObject target)
    {
        GameObject Prefab = null;

        if (miniGame.lastCollisionState == sliderM.CollisionState.Win)
        {
            Debug.Log("�̴ϰ��� ����");

            Prefab = target.objectData.winPrefab;
        }
        else if (miniGame.lastCollisionState == sliderM.CollisionState.Pass)
        {
            Debug.Log("�̴ϰ��� �н�");

            Prefab = target.objectData.passPrefab;
            if (Prefab == null)
                Prefab = target.objectData.winPrefab;
        }
        else
        {
            Debug.Log("�̴ϰ��� ����");

            // ���� ����? ��������
        }

        return Prefab;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        if (currentState == PlayerState.Transitioning && targetObject != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetObject.mountPoint.position);
            Gizmos.DrawWireSphere(targetObject.mountPoint.position, mountThreshold);
        }
    }
}