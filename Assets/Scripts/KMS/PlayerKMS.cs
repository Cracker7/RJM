using System.Collections.Generic;
using UnityEngine;

public class PlayerKMS : MonoBehaviour
{
    // ���� �ӽ�
    private enum PlayerState { Idle, Transitioning, Riding, Dead }
    private PlayerState currentState = PlayerState.Idle;

    // ������Ʈ ĳ��
    private List<SkinnedMeshRenderer> skinRenderer;
    public IInputHandler currentInput;
    public IMovement currentMovement;

    // ���׵� ���� ������Ʈ ĳ��
    private List<Rigidbody> ragdollRigidbodies;
    private List<Collider> ragdollColliders;
    private Rigidbody mainRigidbody;  // ��Ʈ ������Ʈ�� ������ٵ�
    private Collider mainCollider;     // ��Ʈ ������Ʈ�� �ݶ��̴�

    // ���� ����
    [Header("���� ����")]
    public bool useGravity = true;
    public bool isKinematic = false;

    // ��ȣ �ۿ� ����
    private InteractableObject currentInteractableObject;
    [Header("��ȣ�ۿ� ����")]
    public float interactionRange = 2f;
    public KeyCode interactKeyCode = KeyCode.G;
    public float maxRange = 10f;
    public float minRange = 2f;

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

    // �ӵ� ���� ����
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;
    private bool hasSavedVelocity = false;

    private void Awake()
    {
        // �⺻ ������Ʈ ĳ��
        skinRenderer = new List<SkinnedMeshRenderer>(GetComponentsInChildren<SkinnedMeshRenderer>());
        currentMovement = GetComponent<IMovement>();
        currentInput = GetComponent<IInputHandler>();

        // ���׵� ������Ʈ ĳ��
        CacheRagdollComponents();

        // �ʱ� ���� ���� ����
        SetPhysicsState(true, false, false);
    }

    // ���׵� ������Ʈ���� ĳ���ϴ� �޼���
    private void CacheRagdollComponents()
    {
        // ���� ������Ʈ ĳ��
        mainRigidbody = GetComponent<Rigidbody>();
        mainCollider = GetComponent<Collider>();

        // �ڽ� ������Ʈ�� ĳ��
        ragdollRigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        ragdollColliders = new List<Collider>(GetComponentsInChildren<Collider>());

        // ���� ������Ʈ�� ����Ʈ�� ���ԵǾ� �ִٸ� ����
        if (mainRigidbody != null && ragdollRigidbodies.Contains(mainRigidbody))
        {
            ragdollRigidbodies.Remove(mainRigidbody);
        }
        if (mainCollider != null && ragdollColliders.Contains(mainCollider))
        {
            ragdollColliders.Remove(mainCollider);
        }
    }

    private void OnEnable()
    {
        //sliderM.OnShutdown += ResetTimeScale;
    }

    private void OnDisable()
    {
        //sliderM.OnShutdown -= ResetTimeScale;
    }

    private void Update()
    {
        // Transitioning ������ ���� �Է��� �����Ѵ�.
        if (currentState == PlayerState.Transitioning)
        {
            UpdateTransition();
        }
        else if (currentState == PlayerState.Dead)
        {
            // �÷��̾ �׾��� �� �����ϴ� �Լ�

            // �θ� ���� ����
            if (currentObjectPrefab != null)
                transform.SetParent(null);

            // �޽� ������ Ȱ��ȭ
            foreach (SkinnedMeshRenderer skin in skinRenderer)
            {
                skin.enabled = true;
            }
            
            // �׾��� ���� �߰� ���� (�̴ϰ��� ����, ������ ���� ��)
        }
        else
        {
            // �������� Idle�̳� Riding ���¿��� ��ȣ�ۿ� Ű �Է��� ó��������,
            // ���� ž���� �浹 ��(OnCollisionEnter) ó���ϹǷ� �Է� ���� �ڵ�� �����մϴ�.

            // Riding ���¶�� Riding ���� �߰� ������ ó��
            if (currentState == PlayerState.Riding)
            {
                HandleRiding();
            }
        }
        Debug.Log("���̵� ���� : " + currentState);
        Debug.Log("���õ� ������ : " + currentObjectPrefab);
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Riding && currentObjectPrefab != null)
        {
            HandleRidingMovement();
        }
        else if (currentState == PlayerState.Idle)
        {
            HandlePlayerMovement();
        }
    }

    // ���� ���� ������ ���� ���� �޼���
    private void SetPhysicsState(bool enablePhysics, bool isKinematic, bool isTrigger)
    {
        // ���� ������Ʈ ����
        if (mainRigidbody != null)
        {
            mainRigidbody.useGravity = enablePhysics;
            mainRigidbody.isKinematic = isKinematic;
        }

        if (mainCollider != null)
        {
            mainCollider.isTrigger = isTrigger;
            mainCollider.enabled = true;
        }

        // ��� ���׵� ������ٵ� ����
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb != null)
            {
                rb.useGravity = enablePhysics;
                rb.isKinematic = isKinematic;
            }
        }

        // ��� ���׵� �ݶ��̴� ����
        foreach (Collider col in ragdollColliders)
        {
            if (col != null)
            {
                col.isTrigger = isTrigger;
                col.enabled = true;
            }
        }
    }

    private void UpdatePlayerState(PlayerState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                // �Ϲ� ����: ��� ���� Ȱ��ȭ
                SetPhysicsState(true, false, false);
                break;

            case PlayerState.Transitioning:
                // ��ȯ ����: ��� ���� ��Ȱ��ȭ, Ű�׸�ƽ Ȱ��ȭ
                SetPhysicsState(false, true, true);
                break;

            case PlayerState.Dead:
                // ���� ���� : �Ϲ� ���¿� ����
                SetPhysicsState(true, false, false);
                break;

            case PlayerState.Riding:
                // ž�� ����: ��� �ݶ��̴� ��Ȱ��ȭ
                DisableAllPhysics();
                break;
        }
    }

    // ��� ���� ������Ʈ ��Ȱ��ȭ
    private void DisableAllPhysics()
    {
        // ���� ������Ʈ ��Ȱ��ȭ
        if (mainRigidbody != null)
        {
            mainRigidbody.isKinematic = true;
            mainRigidbody.useGravity = false;
        }
        if (mainCollider != null)
        {
            mainCollider.enabled = false;
        }

        // ��� ���׵� ������Ʈ ��Ȱ��ȭ
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        foreach (Collider col in ragdollColliders)
        {
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }

    // ������ Ű �Է� ��� ��ȣ�ۿ� �Լ��� �� �̻� ������� �ʽ��ϴ�.
    // private void HandleInput()
    // {
    //     if (Input.GetKey(interactKeyCode))
    //     {
    //         Debug.Log("��ȣ �ۿ� Ű�� ����");
    //         HandleInteraction();
    //     }
    // }

    // ���� HandleInteraction()�� ������� �����Ƿ� ���� ������.
    // �ʿ��� ��� ���� �ٸ� �Է� ó���� Ȱ���ϼ���.

    /// <summary>
    /// �÷��̾ �ٸ� ������Ʈ�� �浹�ϸ�, �ش� ������Ʈ�� InteractableObject��� ž�� ��ȯ�� �����մϴ�.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // ���� Idle ������ ���� �浹�� ž���� ����մϴ�.
        if (currentState != PlayerState.Idle)
            return;

        // �浹�� ������Ʈ���� InteractableObject ������Ʈ�� �����ɴϴ�.
        InteractableObject interactable = collision.gameObject.GetComponent<InteractableObject>();
        if (interactable != null && interactable != currentInteractableObject)
        {
            Debug.Log("�浹�Ͽ� ��ȣ�ۿ� ����");
            StartTransition(interactable);
        }
    }

    private void StartTransition(InteractableObject target)
    {
        SaveCurrentVelocity(); // ��ȯ ���� ���� ���� �ӵ� ����

        ExitObject(); // ���� ������Ʈ���� ������

        UpdatePlayerState(PlayerState.Transitioning);
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

        // ��ȯ ������ �Ϸ�Ǿ��ų� (normalizedTime >= 1.0f)
        // ��ǥ�� ����� ����������� (distanceToTarget < mountThreshold) ��ȯ �Ϸ� ó��
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
        foreach (SkinnedMeshRenderer skin in skinRenderer)
        {
            skin.enabled = false;
        }

        // ������ Ÿ�� �ִ� ������Ʈ���� ������ ó��
        if (currentInteractableObject != null)
        {
            currentInteractableObject.gameObject.SetActive(true);
        }

        // 3. ���ο� ������Ʈ ����
        currentInteractableObject = interactableObject;

        // 4. ���ο� ������Ʈ Ÿ�� (������ ����)
        Ride(currentInteractableObject);

        // �������� ������ �Ŀ� ����� �ӵ� ����
        ApplySavedVelocity();

        // 5. currentMovement�� currentInput�� �����տ��� ��������
        currentMovement = currentObjectPrefab.GetComponentInChildren<IMovement>();
        currentInput = currentObjectPrefab.GetComponentInChildren<IInputHandler>();

        // 6. ���� ������Ʈ
        UpdatePlayerState(PlayerState.Riding);
        transform.SetParent(currentObjectPrefab.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void ExitObject()
    {
        transform.SetParent(null);
        UpdatePlayerState(PlayerState.Idle);

        foreach (SkinnedMeshRenderer skin in skinRenderer)
        {
            skin.enabled = true;
        }

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

        // ������ ���� ��ġ �� ȸ�� ����
        Vector3 spawnPosition = target.transform.position;
        Quaternion spawnRotation = target.transform.rotation;

        // �̴ϰ��� ����� ���� �����Ǵ� �������� �޶��� �� ����
        // �Ʒ� �ڵ�� �̴ϰ��� �¸� ���������� �����ϴ� �����Դϴ�.
        currentObjectPrefab = Instantiate(target.objectData.winPrefab,
                                         spawnPosition + new Vector3(0, 1f, 0),
                                         spawnRotation);
    }

    // Ż �� �ִ� ������Ʈ ã�� �Լ� (���� ������ ����)
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

    // �̴ϰ����� ����� ���� ������ ���� (���� Ride()���� ���� winPrefab ���)
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
            UpdatePlayerState(PlayerState.Dead);
        }

        return Prefab;
    }

    public void durabilityZero()
    {
        UpdatePlayerState(PlayerState.Dead);
    }

    private void SaveCurrentVelocity()
    {
        if (currentObjectPrefab != null)
        {
            Rigidbody rb = currentObjectPrefab.GetComponent<Rigidbody>();
            if (rb != null)
            {
                savedVelocity = rb.linearVelocity;
                savedAngularVelocity = rb.angularVelocity;
                hasSavedVelocity = true;
                Debug.Log($"Saved velocity: {savedVelocity}, Angular velocity: {savedAngularVelocity}");
            }
        }
    }

    private void ApplySavedVelocity()
    {
        if (hasSavedVelocity && currentObjectPrefab != null)
        {
            Rigidbody rb = currentObjectPrefab.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = savedVelocity;
                rb.angularVelocity = savedAngularVelocity;
                Debug.Log($"Applied velocity: {savedVelocity}, Angular velocity: {savedAngularVelocity}");
            }
            hasSavedVelocity = false;
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

    private void OnTriggerEnter(Collider other) {
        InteractableObject interactable = other.GetComponent<InteractableObject>();
        if(interactable != null)
        {
            Ride(interactable);
        }
    }
}
