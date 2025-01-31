using UnityEngine;

public class RGTMouseV2 : MonoBehaviour
{

    //��ü �ڿ������� ������ �̵��ϰ� �ְ�, ���콺 Ŭ������ �� ��ü�� X��ǥ�� ���콺�� X��ǥ�� ���󰣴�.
    //���� �̿��Ͽ� �̵��ؾ� �Ѵ�.


    private bool isDragging = false;


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
                else
                {
                    isDragging = false;
                }
            }
        }

    }

    public void FallowThePos(GameObject _object)
    {
        Rigidbody rb = _object.GetComponent<Rigidbody>();
        GameObject Object = _object;

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

    

}
