using UnityEngine;

public class RGTCharacterBalanceController : MonoBehaviour
{
    public Transform rollingObject; // �������� ��ü�� Transform
    public Rigidbody characterRigidbody; // ĳ������ Rigidbody
    public float followSpeed = 5f; // ��ü�� ���󰡴� �⺻ �̵� �ӵ�
    public float balanceSpeed = 2f; // ���� ���� �ӵ�
    public float maxBalanceDistance = 4f; // ������ ������ �� �ִ� �ִ� �Ÿ�

    public float randomMoveSpeed = 0.1f; // ���� �̵� �ӵ�
    public float randomMoveInterval = 2f; // ���� �̵� ����

    private Vector3 offset; // ��ü�� ĳ���� �� �ʱ� ��ġ ����
    private Vector3 randomDirection; // ���� �̵� ����
    private float nextRandomMoveTime; // ���� ���� �̵� �ð�


    void Start()
    {
        if (rollingObject == null || characterRigidbody == null)
        {
            Debug.LogError("RollingObject �Ǵ� CharacterRigidbody�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �ʱ� ��ġ ���� ���
        offset = transform.position - rollingObject.position;

        // ĳ������ ȸ���� ����
        characterRigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (rollingObject == null) return;

        // ��ü�� ���� ��ġ + �ʱ� ������ ���
        Vector3 targetPosition = rollingObject.position + offset;

        // ����Ű �Է����� ���� ����
        Vector3 balanceAdjustment = Vector3.zero;

        if (balanceAdjustment == Vector3.zero)
        {
            // ���� �ð����� ���� ���� ����
            if (Time.time >= nextRandomMoveTime)
            {
                SetRandomDirection();
            }
            balanceAdjustment += randomDirection * randomMoveSpeed * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) // �� Ű
            balanceAdjustment += Vector3.left * balanceSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) // �� Ű
            balanceAdjustment += Vector3.right * balanceSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.UpArrow)) // �� Ű
            balanceAdjustment += Vector3.forward * balanceSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.DownArrow)) // �� Ű
            balanceAdjustment += Vector3.back * balanceSpeed * Time.fixedDeltaTime;

        // ���� ������ ������ ��ǥ ��ġ
        targetPosition += balanceAdjustment;

        // ĳ���� �̵�
        characterRigidbody.MovePosition(targetPosition);

        // ���� ���� Ȯ��
        CheckBalance(targetPosition);

        Debug.Log("Pos : " + transform.position);
    }

    private void CheckBalance(Vector3 targetPosition)
    {
        // ĳ���Ͱ� ��ü �߽ɿ��� �ʹ� �־����� ������ �Ҿ��ٰ� �Ǵ�
        float distanceFromCenter = Vector3.Distance(rollingObject.position, transform.position);

        if (distanceFromCenter > maxBalanceDistance)
        {
            // ������ �Ҿ��� �� ó��
            Debug.Log("������ �Ҿ����ϴ�!");
            characterRigidbody.useGravity = true; // �߷��� Ȱ��ȭ�Ͽ� ĳ���Ͱ� ���������� ��
            this.enabled = false; // ��ũ��Ʈ�� ��Ȱ��ȭ�Ͽ� �� �̻� ������ ����
        }
    }

    private void SetRandomDirection()
    {
        // ������ ���� ����
        randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ).normalized;

        // ���� ���� �̵� �ð� ����
        nextRandomMoveTime = Time.time + randomMoveInterval;
    }

}