using UnityEngine;

public class DebugVelocity : MonoBehaviour
{
    private float time;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        time += Time.deltaTime;

        if (time > 1f)
        {
            Debug.Log("���Ͼ� �ӵ� : " + rb.linearVelocity.magnitude);
            Debug.Log("�� �ӵ� : " + rb.angularVelocity.magnitude);
            time = 0;
        }
    }
}
