using UnityEngine;

public class WASDInputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 HandleInput()
    {
        Debug.Log("WASD Input");

        float XAxis = Input.GetAxis("Horizontal");
        float ZAxis = Input.GetAxis("Vertical");
        float YAxis = Input.GetAxis("Jump");

        // ȭ��ǥ Ű�� ������ ��� ���� 0���� �����
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            XAxis = 0f;
        
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            ZAxis = 0f;

        return new Vector3(XAxis, YAxis, ZAxis);

    }
}
