using UnityEngine;

public class ArrowInputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 HandleInput()
    {
        // ȭ��ǥ Ű �Է¿� ���� �̵� ���� ���� �ʱ�ȭ
        Vector3 input = Vector3.zero;

        // �¿� �Է� ó��
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input.x = -1;  // ���� �̵�
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            input.x = 1;   // ������ �̵�
        }

        input = input.normalized;  // �Է� ���� ����ȭ

        return input;
    }
}
