using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGTCapsule : MonoBehaviour
{

    public Transform rollingObject; // �������� ��ü�� Transform
    public Rigidbody characterRigidbody; // ĳ������ Rigidbody

    private Vector3 offset; // ĳ���Ϳ� ��ü ������ �ʱ� ��ġ ����

    void Start()
    {
        if (rollingObject == null || characterRigidbody == null)
        {
            Debug.LogError("RollingObject �Ǵ� CharacterRigidbody�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ĳ���Ϳ� ��ü �� �ʱ� ��ġ ���̸� ���
        offset = transform.position - rollingObject.position;

        // ĳ������ ȸ���� �����Ͽ� ��ü�� ȸ���� ������ ���� �ʵ��� ����
        characterRigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (rollingObject == null) return;

        // ��ü�� ���� ��ġ + �ʱ� �������� ��ǥ ��ġ�� ���
        Vector3 targetPosition = rollingObject.position + offset;

        // ĳ���͸� ��ǥ ��ġ�� �ε巴�� �̵�
        characterRigidbody.MovePosition(targetPosition);
    }


}
