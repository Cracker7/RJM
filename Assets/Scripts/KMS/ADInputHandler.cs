using UnityEngine;

public class ADInputHandler : IInputHandler
{
    private float _moveSpeed = 5f;
    public ADInputHandler(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public Vector3 HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // A, D Ű �Է°��� -1 ~ 1 ������ ������ ��ȯ

        return new Vector2(horizontalInput, 0);
    }
}
