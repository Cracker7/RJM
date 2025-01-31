using UnityEngine;

public class sliderM : MonoBehaviour
{
    public RectTransform handle; 
    private float moveSpot; // �ڵ��� �̵� ��ġ�� �����ϴ� ����
    private bool movingRight = true; // �ڵ��� ���������� �̵� ������ ���θ� ��Ÿ���� ����
    private bool isPaused = false; // �ڵ��� �̵��� �Ͻ� �����Ǿ����� ���θ� ��Ÿ���� ����
    public Canvas canvas;

    private void Start()
    {
        moveSpot = handle.anchoredPosition.x; // �ڵ��� �ʱ� ��ġ�� moveSpot ������ ����
    }

    private void Update()
    {
        HandleInput(); // ����� �Է��� ó��
        if (!isPaused) // �̵��� �Ͻ� �������� ���� ���
        {
            MoveHandle(); // �ڵ��� �̵�
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused; // �̵� ���¸� ���
            if (isPaused) // �̵��� �Ͻ� ������ ���
            {
                CheckCollision(); // �浹�� Ȯ��
                Invoke("ShutDown", 1f); // canvas ����
            }

        }
    }

    private void MoveHandle()
    {
        float moveSpeed = Time.deltaTime * 60; // �̵� �ӵ��� ����
        moveSpot += movingRight ? moveSpeed : -moveSpeed; // �̵� ���⿡ ���� moveSpot�� ���� �Ǵ� ����

        if (moveSpot >= 140) // moveSpot�� 140 �̻��̸�
        {
            movingRight = false; // �̵� ������ �������� ����
        }
        else if (moveSpot <= 0) // moveSpot�� 0 �����̸�
        {
            movingRight = true; // �̵� ������ ���������� ����
        }

        handle.anchoredPosition = new Vector2(moveSpot, handle.anchoredPosition.y); // �ڵ��� ��ġ�� ������Ʈ
    }

    private void CheckCollision()
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
                    switch(tag)
                    {
                        case "win":
                            Debug.Log("win");
                            break;
                        case "fail":
                            Debug.Log("win");
                            break;
                        case "pass":
                            Debug.Log("pass");
                            break;

                    }

                }
            }
        }
    }

    private void ShutDown()
    {
        canvas.gameObject.SetActive(false);
    }

   
    // ���߿� �ٽ� �����Ҷ� ����� �Լ�
    public void OpenCanvas()
    {
        canvas.gameObject.SetActive(true);
        isPaused = false;
    }

}
