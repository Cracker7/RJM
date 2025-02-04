using UnityEngine;

public class RGTMouseV2 : MonoBehaviour
{

    //��ü �ڿ������� ������ �̵��ϰ� �ְ�, ���콺 Ŭ������ �� ��ü�� X��ǥ�� ���콺�� X��ǥ�� ���󰣴�.
    //���� �̿��Ͽ� �̵��ؾ� �Ѵ�.


    public GameObject hat;
    private bool isDragging = false;
    public float torqueForce = 10f; // �¿� ȸ�� ��
    public float forwardForce = 15f; // ���� ��

    [SerializeField] private Rigidbody[] ragdollLimbs;


    private void Update()
    {
        FallowThePos(hat);
    }

    private void GetMouseButton()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.tag == "hat")
                {
                    isDragging = true;
                }

            }
        }
        else
        {
            isDragging = false;
        }

    }

    public void FallowThePos(GameObject _object)
    {
        Rigidbody rb = _object.GetComponent<Rigidbody>();
        GameObject Object = _object;

        rb.AddForce(Vector3.forward * forwardForce, ForceMode.Force);
        rb.AddTorque(Vector3.up * -torqueForce, ForceMode.Force);
        GetMouseButton();

        if(isDragging)
        {
            //������ x��ǥ, ���콺�� x��ǥ ���� ����.

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                Vector3 newPosition = Object.transform.position;
                newPosition.x = hit.point.x;
                Object.transform.position = newPosition;
            }
        }
    }

    //private void FlapRagdoll(Vector3 direction)
    //{
    //    foreach (Rigidbody limb in ragdollLimbs)
    //    {
    //        Vector3 randomForce = direction * -1 * ragdollFlapForce * Random.Range(0.8f, 1.2f);
    //        limb.AddForce(randomForce, ForceMode.Impulse);  // ������ ���� �߰�
    //    }
    //}

}
