using UnityEngine;

public class RGTADKeyObjection : MonoBehaviour, IMovement
{
    //�̵� �ӵ�
    [SerializeField] private float moveSpeed = 50f;
    //�ε巯�� �̵��� ���� �ð�
    [SerializeField] private float smoothTime = 0.1f;

    //��ǥ ��ġ
    private Vector3 TargetPosition;
    //���� �ӵ�
    private Vector3 velocity = Vector3.zero;


    //public void ADObjection(GameObject _object)
    //{
    //    TargetPosition = _object.transform.position;
    //    GameObject Object = _object;

    //    // AŰ�� ������ �������� �̵�
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        TargetPosition += Vector3.right * MoveSpeed * Time.deltaTime;
    //    }

    //    // DŰ�� ������ ���������� �̵�
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        TargetPosition += Vector3.left * MoveSpeed * Time.deltaTime;
    //    }

    //    // �ε巴�� �̵�
    //    Object.transform.position = Vector3.SmoothDamp(Object.transform.position, TargetPosition, ref velocity, smoothTime);
    //}

    public void Move(Vector3 input)
    {
        // ���� ��ġ�� �������� targetPosition�� ����մϴ�.
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = currentPosition + input * moveSpeed * Time.deltaTime;

        // SmoothDamp�� �̿��� �ε巴�� targetPosition���� �̵��մϴ�.
        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocity, smoothTime);
    }
}
