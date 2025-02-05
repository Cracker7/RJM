using UnityEngine;

public class ArrowInputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 HandleInput()
    {
        float isAKeyPressed = Input.GetAxis("Horizontal");

        // ȭ��ǥ Ű�� ������ ��� ���� 0���� �����
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            isAKeyPressed = 0f;
        }

        return new Vector3(isAKeyPressed, 0, 0);
    }
}
