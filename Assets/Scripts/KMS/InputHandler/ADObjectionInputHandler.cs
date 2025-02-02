using UnityEngine;

public class ADObjectionInputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 HandleInput()
    {
        // A Ű�� D Ű �Է� �ޱ�
        bool isAKeyPressed = Input.GetKey(KeyCode.A);
        bool isDKeyPressed = Input.GetKey(KeyCode.D);

        if (isAKeyPressed)
        {
            return transform.right;
        }
        else if (isDKeyPressed)
        {
            return -transform.right;
        }
        return Vector3.zero;

    }
}
