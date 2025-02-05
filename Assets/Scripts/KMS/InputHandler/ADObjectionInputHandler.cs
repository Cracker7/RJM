using UnityEngine;

public class ADObjectionInputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 HandleInput()
    {
        Debug.Log("�ݴ� ADŰ �Է� �޴���");
        // A Ű�� D Ű �Է� �ޱ�
        float isAKeyPressed = Input.GetAxis("Horizontal");

        // ȭ��ǥ Ű�� ������ ��� ���� 0���� �����
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            isAKeyPressed = 0f;
        }

        return new Vector3(-isAKeyPressed, 0, 0);

    }
}
