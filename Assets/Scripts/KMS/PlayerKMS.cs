using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
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
    public float interactionRange = 2f;
    public KeyCode interactKeyCode = KeyCode.G;

    // ������ �̵� ����
    private Vector3 startPosition;
    private float transitionTime = 0f;
    public float transitionDuration = 1f;
    public float jumpHeight = 3f;
    public float mountThreshold = 0.5f;
    private InteractableObject targetObject = null;
    private Vector3 lastKnownMountPoint;
    public GameObject currentObjectPrefab;

    private void Awake()
    {
        // ������Ʈ ĳ��
        meshRenderer = GetComponent<MeshRenderer>();
        currentMovement = GetComponent<IMovement>();
        currentInput = GetComponent<IInputHandler>();
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
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Riding && currentObjectPrefab != null)
        {
            transform.position = currentObjectPrefab.transform.position;
            transform.rotation = currentObjectPrefab.transform.rotation;
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
        if (targetObject == null)
        {
            currentState = PlayerState.Idle;
            return;
        }

        transitionTime += Time.deltaTime;
        float normalizedTime = transitionTime / transitionDuration;

        Vector3 targetPosition = targetObject.mountPoint.position;
        lastKnownMountPoint = targetPosition;

        float height = Mathf.Sin(normalizedTime * Mathf.PI) * jumpHeight;
        Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, normalizedTime) + Vector3.up * height;

        transform.position = currentPosition;

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (normalizedTime >= 1.0f || distanceToTarget < mountThreshold)
        {
            CompleteTransition();
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
        currentMovement = currentObjectPrefab.GetComponent<IMovement>();
        currentInput = currentObjectPrefab.GetComponent<IInputHandler>();

        // 6. ���� ������Ʈ
        currentState = PlayerState.Riding;

    }

    private void ExitObject()
    {
        // Ÿ�� �ִ� ������Ʈ�� �ִٸ�, �ش� ������Ʈ���� ����
        if (currentInteractableObject != null)
        {
            // �÷��̾� �޽� ������ Ȱ��ȭ
            meshRenderer.enabled = true;

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
        // �÷��̾� �޽� ������ ��Ȱ��ȭ
        meshRenderer.enabled = false;

        // Ÿ�� ��Ȱ��ȭ
        target.gameObject.SetActive(false);

        //// ���ο� ������ ����
        //Vector3 spawnPosition = target.mountPoint.position;
        //Quaternion spawnRotation = target.mountPoint.rotation;
        // ���ο� ������ ����
        Vector3 spawnPosition = target.transform.position;
        Quaternion spawnRotation = target.transform.rotation;

        currentObjectPrefab = Instantiate(target.objectData.ridePrefab,
                                         spawnPosition+new Vector3(0,0.1f,0),
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