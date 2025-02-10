using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float maxDurability; // ��ü�� �ִ� ������
    public float currentDurability; // ��ü�� ���� ������
    public bool isOccupied = false; // ���� ��� ������ ����

    public Transform mountPoint; // ž�� ��ġ
    public ObjectSpecificData objectData; // ��ü�� ���� ������

    public IMovement movementController; // ��ü�� �̵� ��Ʈ�ѷ�
    public IInputHandler inputHandler; // ž�½� ����� �Է� ���
    
    //public string mountAnimationName; // ž�� �ִϸ��̼� �̸�
    //public MountType mountType; // ž�� ����

    private void Awake()
    {
        Init();
        currentDurability = objectData.durability;
        maxDurability = objectData.durability;
    }

    public void Init()
    {
        movementController = GetComponent<IMovement>();
        inputHandler = GetComponent<IInputHandler>();
    }

    //// �÷��̾ ��ȣ�ۿ� �� ȣ��Ǵ� �Լ�
    //public virtual void Interact(PlayerKMS player)
    //{
    //    if (!isOccupied)
    //    {
    //        if (inputHandler != null)
    //            player.ChangeInput(inputHandler); // �÷��̾��� �Է� ����� ����
            
    //        player.EnterObject(this); // �÷��̾ ��ü�� ž�½�Ŵ
    //        isOccupied = true; // ��ü�� ��� ������ ǥ��
    //    }
    //}

    // ��ü�� �������� �Ծ��� �� ȣ��Ǵ� �Լ�
    public virtual void TakeDamage(float damage)
    {
        currentDurability -= damage; // ������ ����
        if (currentDurability <= 0)
        {
            DestroyObject(); // �������� 0 ���ϰ� �Ǹ� ��ü�� �ı�

            //var player = FindObjectsByType<PlayerKMS>();
            // var player = FindFirstObjectByType<PlayerKMS>();
            // player.durabilityZero();
        }
    }

    // ��ü�� �ı��� �� ȣ��Ǵ� �Լ�
    private void DestroyObject()
    {
        // �ı� ���� ����
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TakeDamage(collision.impulse.magnitude);
        //Debug.Log("���� ��ݷ�" + collision.impulse.magnitude);
    }
}
