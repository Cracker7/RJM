using UnityEngine;

public class RGTArrowKey : MonoBehaviour, IMovement
{

    //�̵��ӵ�
    [SerializeField] private float moveSpeed = 50f; 
    //�ε巯�� �̵��ð�
    [SerializeField] private float smoothTime = 0.1f; 

    //��ǥ��ġ
    private Vector3 targetPosition; 
    //����ӵ�
    private Vector3 velocity = Vector3.zero;


    //public void ArrowKey(GameObject _object)
    //{
    //    GameObject Object = _object;
    //    targetPosition = Object.transform.position; 
    //    // ����Ű �Է� ���� (�¿� �̵�)
    //    float moveInput = Input.GetAxis("Horizontal"); // ����(-1), ������(1)
    //    targetPosition += Vector3.right * moveInput * moveSpeed * Time.deltaTime;

    //    // �ε巴�� �̵�
    //    Object.transform.position = Vector3.SmoothDamp(Object.transform.position, targetPosition, ref velocity, smoothTime);
    //}

    public void Move(float input)
    {
        Vector3 currentPosition = transform.position;
        //Vector3 targetPosition = currentPosition + input * moveSpeed * Time.deltaTime;

        // SmoothDamp�� �̿��� �ε巴�� ��ǥ ��ġ�� �̵�
        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocity, smoothTime);
    }
}
