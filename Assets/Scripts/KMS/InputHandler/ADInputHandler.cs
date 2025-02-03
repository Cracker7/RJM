using UnityEngine;

public class ADInputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 HandleInput()
    {
        Debug.Log("ADŰ �Է� �޴���");
        // A Ű�� D Ű �Է� �ޱ�
        bool isAKeyPressed = Input.GetKey(KeyCode.A);
        bool isDKeyPressed = Input.GetKey(KeyCode.D);

        if (isAKeyPressed)
        {
            return -transform.right;
        }
        else if (isDKeyPressed)
        {
            return transform.right;
        }
        return Vector3.zero;

    }
}
