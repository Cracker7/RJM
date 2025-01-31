using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class RGTUIKey : MonoBehaviour
{
    //�̵��ӵ�
    [SerializeField] private float MoveSpeed = 50f;
    //�ε巯�� �̵��ð�
    [SerializeField] private float SmoothTime = 0.1f;

    //��ǥ��ġ
    private Vector3 TargetPosition;
    //����ӵ�
    private Vector3 velocity = Vector3.zero;


    public void UIKey(GameObject _object)
    {
        TargetPosition = _object.transform.position;
        GameObject Object = _object;

        if (Input.GetMouseButton(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition; // ���콺 ��ġ

            // Raycast ����� ������ ����Ʈ
            System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            // ��� ó��
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Left")) // �±װ� "Left"�� UI ���
                {
                    Debug.Log("Left Ŭ��");
                    TargetPosition += Vector3.left * MoveSpeed * Time.deltaTime;
                }
                else if (result.gameObject.CompareTag("Right")) // �±װ� "Right"�� UI ���
                {
                    Debug.Log("Right Ŭ��");
                    TargetPosition += Vector3.right * MoveSpeed * Time.deltaTime;
                }
            }
                Object.transform.position = Vector3.SmoothDamp(Object.transform.position, TargetPosition, ref velocity, SmoothTime);
        }
    }


}
