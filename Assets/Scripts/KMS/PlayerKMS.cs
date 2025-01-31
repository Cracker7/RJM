using UnityEngine;
using UnityEngine.UIElements;

public class PlayerKMS: MonoBehaviour
{
    public IInputHandler currentInput;
    public IMovementController currentMovement;
    private InteractableObject currentInteractableObject;

    public float interactionRange = 2f;

    public KeyCode interactKeyCode = KeyCode.G;

    private void Awake()
    {
        //// �⺻���� Movement, Input �ʱ�ȭ
        //currentInput = new ADInputHandler(5f);
        //// �÷��̾ �⺻������ AD �����Ʈ�� �޷��־����
        //currentMovement = GetComponent<IMovementController>();
    }

    private void Update()
    {
        if (currentInput != null && currentMovement != null)
        {
            currentMovement.Move(currentInput.HandleInput());
        }

        if (Input.GetKey(interactKeyCode))
        {
            CheckForInteractableObjects();
            EnterObject(currentInteractableObject);
        }
    }

    // ������ �ִ� ���� Ż �� ã��
    private void CheckForInteractableObjects()
    {
        // interactionRange�� ��ü�� �߽ɿ��� ã�� ������
        // ������ �� ��ü�� ���� ���� ���� + range�� �����ϴ� �Լ��� �ʿ��� ����
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);

        InteractableObject closestObject = null;
        float closestDistance = Mathf.Infinity;


        foreach (var hitCollider in hitColliders)
        {
            InteractableObject interactable = hitCollider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = interactable;
                }
            }
        }

        currentInteractableObject = closestObject;
    }


    // ��ü�� �ٴ� �Լ�
    public void EnterObject(InteractableObject interactableObject)
    {
        if (interactableObject.mountPoint != null)
        {
            transform.position = interactableObject.mountPoint.position;
            transform.rotation = interactableObject.mountPoint.rotation;
        }
        // else������ ��ü�� ��ã�������� ��� ������ �����ָ� ���ƺ���
        ChangeMovement(interactableObject.movementController);
        ChangeInput(interactableObject.inputHandler);

        // ž���� �� ��� �Ұ������� ���� ��
        //if (interactableObject.mountAnimationName != null && animator != null)
        //{
        //    animator.Play(interactableObject.mountAnimationName);
        //}
    }

    // ��ü�� �������� �Լ�
    public void ExitObject()
    {
        if (currentInteractableObject != null)
        {
            currentMovement = GetComponent<IMovementController>();
            
            currentInteractableObject = null;
            //if (animator != null)
            //{
            //    animator.Play("Default");
            //}
        }
    }


    // Movement�� �����ϴ� �Լ�
    public void ChangeMovement(IMovementController newMovement)
    {
        currentMovement = newMovement;
    }
    
    // inputHandler�� �����ϴ� �Լ�
    public void ChangeInput(IInputHandler newInputHandler)
    {
        currentInput = newInputHandler;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

}
