using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ���� ��� (ĳ����)
    public Vector3 offset = new Vector3(0, 10, 0); // ī�޶� ��ġ ����
    public float smoothSpeed = 5f; // �ε巯�� �̵� �ӵ�

    void LateUpdate()
    {
        if (target == null) return;

        // ��ǥ ��ġ ���
        Vector3 targetPosition = target.position + offset;

        // �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // ĳ���͸� �ٶ󺸵��� ����
        transform.LookAt(target);
    }
}
