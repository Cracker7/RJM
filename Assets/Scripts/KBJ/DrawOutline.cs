using System.Linq;
using UnityEngine;

public class DrawOutline : MonoBehaviour
{
    public float moveSpeed = 5f;  // �̵� �ӵ�
    public float detectionRadius = 5f; // ���� ����
    public LayerMask layer;

    private GameObject closestObject;
    private GameObject previousClosestObject;

    void Update()
    {
        MovePlayer();
        DetectClosestObject();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");  // A, D (����, ������)
        float moveZ = Input.GetAxisRaw("Vertical");    // W, S (��, �Ʒ�)

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed * Time.deltaTime;
        if (move != Vector3.zero)
        {
            transform.Translate(move, Space.World);
        }
    }

    void DetectClosestObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, layer);

        GameObject newClosestObject = colliders
            .Select(c => c.gameObject)
            .OrderBy(go => (go.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();

        if (newClosestObject != closestObject)
        {
            UpdateOutlineEffect(newClosestObject);
            closestObject = newClosestObject;
        }
    }

    void UpdateOutlineEffect(GameObject newObject)
    {
        if (previousClosestObject != null)
        {
            RemoveOutline(previousClosestObject);
        }

        if (newObject != null)
        {
            AddOutline(newObject);
        }

        previousClosestObject = newObject;
    }

    void AddOutline(GameObject obj)
    {
        if (!obj.TryGetComponent(out Outline outline))
        {
            outline = obj.AddComponent<Outline>();
        }
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
    }

    void RemoveOutline(GameObject obj)
    {
        if (obj.TryGetComponent(out Outline outline))
        {
            Destroy(outline);
        }
    }

    // ����׿����� ���� ���� �ð�ȭ
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
