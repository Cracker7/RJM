using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    // ȸ�� �ӵ��� �����մϴ� (����: ��/��).
    public float rotationSpeed = 100f;

    void Update()
    {
        // Y���� �������� ȸ���մϴ�.
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
