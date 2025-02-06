using UnityEngine;
using System; // Enum ���� ��� ���

public class sliderM : MonoBehaviour
{
    // ���¸� ��Ÿ���� enum ���� (�±� �̸��� �ҹ��ڷ� ����ϹǷ� ToLower()�� ��ȯ)
    public enum CollisionState
    {
        Win,
        Fail,
        Pass
    }

    public RectTransform handle;
    private float moveSpot;            // �ڵ��� �̵� ��ġ�� �����ϴ� ����
    private bool movingRight = true;   // �ڵ��� ���������� �̵� ������ ����
    private bool isPaused = false;     // �ڵ��� �̵��� �Ͻ� �����Ǿ����� ����
    public Canvas canvas;

    public CollisionState lastCollisionState;
    public static event Action OnShutdown;

    private void Start()
    {
        moveSpot = handle.anchoredPosition.x; // �ڵ��� �ʱ� ��ġ ����
    }

    private void Update()
    {
        HandleInput(); // ����� �Է� ó��
        if (!isPaused) // �̵��� �Ͻ� ������ �ƴ� ��
        {
            MoveHandle(); // �ڵ� �̵�
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused; // �̵� ���� ���
            if (isPaused) // �̵��� �Ͻ� �����Ǿ��� ��
            {
                lastCollisionState = CheckCollision();  // �浹 üũ (���ο��� ���¿� ���� ó��)
                //Invoke("ShutDown"); // 1�� �Ŀ� canvas ����
                ShutDown();
            }
        }
    }

    private void MoveHandle()
    {
        // Time.deltaTime�� Time.timeScale�� �̿��Ͽ� �̵� �ӵ� ����
        float moveSpeed = Time.deltaTime * 60 / Time.timeScale;
        moveSpot += movingRight ? moveSpeed : -moveSpeed; // �̵� ���⿡ ���� ��ǥ ����/����

        // �¿� �Ѱ��� üũ (0 ~ 140)
        if (moveSpot >= 140)
        {
            movingRight = false;
        }
        else if (moveSpot <= 0)
        {
            movingRight = true;
        }

        // �ڵ��� ��ġ ������Ʈ
        handle.anchoredPosition = new Vector2(moveSpot, handle.anchoredPosition.y);
    }

    /// <summary>
    /// �浹�� üũ�ϰ�, �浹�� ���¿� ���� �α׸� ����մϴ�.
    /// </summary>
    public CollisionState CheckCollision()
    {
        string[] tags = { "win", "fail", "pass" };
        foreach (string tag in tags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag); // �ش� �±׸� ���� ��ü���� ã��
            foreach (GameObject obj in objects)
            {
                RectTransform rt = obj.GetComponent<RectTransform>(); // ��ü�� RectTransform�� ������
                if (RectTransformUtility.RectangleContainsScreenPoint(rt, handle.position)) // �ڵ��� ��ü ���� �ִ��� Ȯ��
                {
                    // Debug.Log($"Handle is on {tag} "); // �ڵ��� �ش� �±� ���� ������ �α׿� ���
                    switch (tag)
                    {
                        case "win":
                            Debug.Log("win");
                            return CollisionState.Win;
                        case "fail":
                            Debug.Log("fail");
                            return CollisionState.Fail;
                        case "pass":
                            Debug.Log("pass");
                            return CollisionState.Pass;

                    }

                }
            }
        }
        return CollisionState.Fail;
    }

    private void ShutDown()
    {
        canvas.gameObject.SetActive(false);

        OnShutdown?.Invoke();
    }

    // ���߿� �ٽ� ������ �� ����� �Լ�
    public void OpenCanvas()
    {
        canvas.gameObject.SetActive(true);
        isPaused = false;
    }
}
