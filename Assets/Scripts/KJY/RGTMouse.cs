using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RGTMouse : MonoBehaviour
{

    //public float forwardForce = 10f;  // �̵� ��
    //public float torqueForce = 5f;    // ȸ�� ��
    //public float rotationDamping = 0.95f; // ȸ�� ���� ���

    //private Rigidbody rollingRigidbody;
    //private bool isDragging = false;
    //private Vector3 lastMousePosition;

    //void Start()
    //{
    //    rollingRigidbody = GetComponent<Rigidbody>();
    //}

    //void Update()
    //{
    //    // ���콺 ��ư�� ������ �巡�� ����
    //    if (Input.GetMouseButton(0))
    //    {
    //        isDragging = true;
    //        lastMousePosition = Input.mousePosition;
    //    }

    //    // ���콺�� ���� �巡�� ����
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        isDragging = false;
    //    }

    //    // ���콺 �巡�� ���� �� �̵� �� ȸ��
    //    if (isDragging)
    //    {
    //        Vector3 mouseDelta = Input.mousePosition - lastMousePosition; // ���콺 �̵� �Ÿ� ���

    //        if(mouseDelta.x > 0)
    //        {

    //        }

    //        rollingRigidbody.AddForce(Vector3.right * mouseDelta.x * forwardForce * Time.deltaTime, ForceMode.Force);
    //        rollingRigidbody.AddTorque(Vector3.up * mouseDelta.x * torqueForce * Time.deltaTime, ForceMode.Force);

    //        lastMousePosition = Input.mousePosition; // ���� ���콺 ��ġ ����
    //    }

    //    // ���콺�� ������ ���������� ȸ�� ����
    //    if (!isDragging)
    //    {
    //        rollingRigidbody.angularVelocity *= rotationDamping;
    //    }
    //}



    //���콺�� Ŭ���ߴ�. �׸��� ���̸� ���� ��¤���� �ν��Ѵ�. ��¤������ x ���� ���콺�� x���� ���� ���� �Ѵ�.

    //private bool isHat;
    //public Camera mainCamera; // ī�޶�
    //public float moveSpeed = 10f; // �̵� �ӵ�

    //private Rigidbody rb;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
    //}
    //private void Update()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            GameObject clickedObject = hit.collider.gameObject;
    //            if (clickedObject.tag == "hat")
    //            {
    //                isHat = true;

    //            }
    //            if (isHat)
    //            {
    //                //Vector3 objectPosition = clickedObject.transform.position; // ���� ��ġ
    //                //objectPosition.x = hit.point.x; // ���콺 ��ġ�� x �� ����
    //                //clickedObject.transform.position = objectPosition; // ��ġ ������Ʈ

    //                Vector3 targetPosition = hit.point; // �浹�� ����
    //                Vector3 direction = targetPosition - transform.position; // ���� ��ġ�� ��ǥ ��ġ ���� ����

    //                // ��ǥ ��ġ�� �̵� (Rigidbody�� ����Ͽ� ������ �̵�)
    //                rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);
    //            }
    //        }
    //    }

    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        isHat = false;
    //    }

    //}



    //void Update()
    //{
    //    // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ (Z���� ī�޶���� �Ÿ�)
    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        Vector3 targetPosition = hit.point; // �浹�� ����
    //        Vector3 direction = targetPosition - transform.position; // ���� ��ġ�� ��ǥ ��ġ ���� ����

    //        // ��ǥ ��ġ�� �̵� (Rigidbody�� ����Ͽ� ������ �̵�)
    //        rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);
    //    }
    //}




    //private Rigidbody rb;
    //private bool isDragging = false;
    //private Vector3 offset;

    //public Camera mainCamera; // ī�޶�

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
    //}

    //void OnMouseDown()
    //{
    //    // ���콺�� Ŭ���� ��ü���� ���̸� ����ؼ� �巡���� �� ���콺�� ��ü�� ��� ��ġ�� ����
    //    offset = transform.position - GetMouseWorldPos();
    //    isDragging = true;
    //}

    //void OnMouseUp()
    //{
    //    isDragging = false; // ���콺�� ���� �巡�� ����
    //}

    //void Update()
    //{
    //    if (isDragging)
    //    {
    //        // ���콺 ��ġ�� ���� �����̵��� Rigidbody�� ����� ���������� �̵�
    //        Vector3 targetPosition = GetMouseWorldPos() + offset;
    //        Vector3 direction = targetPosition - transform.position;

    //        // X, Z�����θ� �̵� (Y���� ��ü�� ���������� ����)
    //        targetPosition.y = transform.position.y;

    //        rb.MovePosition(targetPosition); // ������ �̵�
    //    }
    //}

    //Vector3 GetMouseWorldPos()
    //{
    //    // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ (Z���� ī�޶󿡼� ��ü������ �Ÿ�)
    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        return hit.point;
    //    }

    //    return Vector3.zero;
    //}

    public Camera mainCamera;  // ���� ī�޶�
    private Rigidbody rb;  // ���� ������ ��ü�� Rigidbody
    private SpringJoint springJoint;  // ���콺�� ���� �� ����� SpringJoint
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư�� ������ ��
        {
            TryGrabObject();
        }
        else if (Input.GetMouseButtonUp(0)) // ���콺�� ������ ��
        {
            ReleaseObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Rigidbody hitRb = hit.rigidbody;

            if (hitRb != null && !hitRb.isKinematic) // Rigidbody�� �ְ�, ���������� ������ �� �ִ� ���
            {
                rb = hitRb;

                // SpringJoint�� ������ �߰�
                if (springJoint == null)
                {
                    GameObject jointHolder = new GameObject("SpringJointHolder");
                    //jointHolder.transform.position = hit.point;
                    springJoint = jointHolder.AddComponent<SpringJoint>();
                    springJoint.connectedBody = rb;
                    springJoint.autoConfigureConnectedAnchor = false;
                    springJoint.spring = 200f; // ������ ���� (���� ���� ����)
                    springJoint.damper = 10f; // ���� (�ʹ� Ƣ�� �ʵ���)
                    springJoint.maxDistance = 0.01f; // �ִ� �Ÿ� ����
                }
                //rb.position = hit.point;
                //SPJ.x = hit.point.x;
                //springJoint.transform.position = hit.point;

                Vector3 SPJ = springJoint.transform.position;
                SPJ.x = hit.point.x;
                springJoint.transform.position = SPJ;
                springJoint.connectedAnchor = rb.transform.InverseTransformPoint(hit.point);

                rb.freezeRotation = true;
                isDragging = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isDragging && springJoint != null)
        {
            // ���콺 ��ġ�� ���󰡵��� SpringJoint ��ġ ������Ʈ
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                springJoint.transform.position = hit.point;
            }
        }
    }

    void ReleaseObject()
    {
        isDragging = false;
        if (springJoint != null)
        {
            Destroy(springJoint.gameObject); // SpringJoint ����
            springJoint = null;
        }
        if (rb != null)
        {
            rb.freezeRotation = false; // ȸ�� ���� ����
        }
        rb = null;
    }


}
