using UnityEngine;
using UnityEngine.Purchasing;

public class RGTCharacterBalanceController : MonoBehaviour
{
    //[SerializeField] private AnimationClip Anim;
    [SerializeField] private Animator animator;
    [SerializeField] Transform TheBall;
    [SerializeField] Rigidbody CharacterRigidbody;
    [SerializeField] private float balanceSpeed = 100f;
    // �� ������ �߽��� ��� ����
    public float balanceHeight = 3.5f; 



    private void Start()
    {
        CharacterRigidbody.isKinematic = true;
        Animator animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // ĳ���Ͱ� �� ���� ������ ���̿� �ֵ��� ����
        transform.position = TheBall.position + Vector3.up * balanceHeight;
    }


    private void Update()
    {
        Vector3 centerPosition = new Vector3(TheBall.position.x, TheBall.position.y + balanceHeight, TheBall.position.z);
        
        animator.SetBool("SetRightActive", true);
        //animator.SetBool("SetLeftActive", true);
        //animator.SetBool("SetUpActive", true);
        //animator.SetBool("SetBackActive", true);
        animator.SetBool("Activefalse",true);

        //ȸ������ �����ϴ� �ɷ� �ٲ�� ��
        if (Input.GetKey(KeyCode.LeftArrow))
        {

            //transform.eulerAngles += Vector3.forward * balanceSpeed * Time.deltaTime;
            //transform.Rotate(0f,0f,+RotationValue);
            //animator.SetBool("SetLeftActive", false);
            RotateZ(1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("SetRightActive", false);
            //transform.eulerAngles += Vector3.back * balanceSpeed * Time.deltaTime;
            RotateZ(-1);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.eulerAngles += Vector3.right * balanceSpeed * Time.deltaTime;
            //animator.SetBool("SetUpActive", false);
            RotateX(1);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.eulerAngles += Vector3.up * balanceSpeed * Time.deltaTime;
            //animator.SetBool("SetBackActive", false);
            RotateX(-1);
        }
        //else
        {

        }

        //CheckRotation();


    }

    private void RotateX(float _direction)
    {
        float xRotation = _direction * balanceSpeed * Time.deltaTime;
        //transform.Rotate(xRotation, 0f, 0f, Space.Self);

        float newXRotation = transform.eulerAngles.x + xRotation;

        // ����Ƽ�� EulerAngles�� 0~360���� ǥ���ǹǷ� -60~60���� ��ȯ�ؾ� ��
        if (newXRotation > 180f)
            newXRotation -= 360f;

        newXRotation = Mathf.Clamp(newXRotation, -60f, 60f);

        transform.rotation = Quaternion.Euler(newXRotation, 0f, 0f);
    }

    private void RotateZ(float _direction)
    {
        float zRotation = _direction * balanceSpeed * Time.deltaTime;
        //transform.Rotate( 0f, 0f, zRotation, Space.Self);
        float newZRotation = transform.eulerAngles.z + zRotation;

        // ����Ƽ�� EulerAngles�� 0~360���� ǥ���ǹǷ� -60~60���� ��ȯ�ؾ� ��
        if (newZRotation > 180f)
            newZRotation -= 360f;

        newZRotation = Mathf.Clamp(newZRotation, -60f, 60f);

        transform.rotation = Quaternion.Euler(0f, 0f, newZRotation);
    }

    private void RanDomRotation()
    {

        float randomRotation = Random.Range(-90f, 90f) * Time.deltaTime;
        transform.Rotate(0, randomRotation, randomRotation);

    }

    //private void RanDomRotation1()
    //{
    //    //transform.Translate(new Vector3(Random.Range(0f, 1.3f),0f, Random.Range(0f, 1.3f)));
    //    //transform.rotation = transform.localRotation(Quaternion.Euler(Random.Range(1f, 90f), Random.Range(1f, 90f), Random.Range(1f, 90f)));

    //    float randomRotation = Random.Range(-90f, 90f) * Time.deltaTime;
    //    transform.Rotate(0, randomRotation, randomRotation);

    //}
    private void RanDomRotation1()
    {
        float randomX = Random.Range(-10f, 10f) * Time.deltaTime;
        float randomZ = Random.Range(-10f, 10f) * Time.deltaTime;

        float newXRotation = transform.eulerAngles.x + randomX;
        float newZRotation = transform.eulerAngles.z + randomZ;

        if (newXRotation > 180f) newXRotation -= 360f;
        if (newZRotation > 180f) newZRotation -= 360f;

        newXRotation = Mathf.Clamp(newXRotation, -60f, 60f);
        newZRotation = Mathf.Clamp(newZRotation, -60f, 60f);

        transform.rotation = Quaternion.Euler(newXRotation, 0f, newZRotation);
    }



    //üũ ȸ�� ��
    private void CheckRotation()
    {
        //���� üũ�ϱ� ���࿡ 60���� ũ�� 
        if(transform.rotation.eulerAngles.x > 60f)
        {
            CharacterRigidbody.useGravity = true;
            CharacterRigidbody.isKinematic = false;
            this.enabled = false;
        }
    }





}