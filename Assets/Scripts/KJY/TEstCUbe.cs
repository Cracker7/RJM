using UnityEngine;

public class TEstCUbe : MonoBehaviour
{
    private Rigidbody rb;
    public float rollForce = 10f;  // �������� ��
    public float moveForce = 5f;   // �¿� �̵� ��

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 10f; // �ִ� ȸ�� �ӵ� ����
    }

    void FixedUpdate()
    {
        ApplyRolling();
        HandleInput();
    }

    void ApplyRolling()
    {
        // ������ �����Ͽ� ������ ���
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal);
            rb.AddTorque(Vector3.Cross(slopeDirection, Vector3.up) * rollForce);
            Debug.Log("Torque : " + Vector3.Cross(slopeDirection, Vector3.up)*rollForce);
        }
    }

    void HandleInput()
    {
        float moveInput = Input.GetAxis("Horizontal"); // A, D �Ǵ� �¿� ȭ��ǥ �Է� �ޱ�
        rb.AddForce(Vector3.right * moveInput * moveForce, ForceMode.Acceleration);
    }


}
