using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public Terrain[] terrain;           // �ͷ��� ����
    public GameObject[] objectPrefab;   // ��ġ�� ������Ʈ ������
    public int numberOfObjects = 100; // ��ġ�� ������Ʈ ����

    void Start()
    {
        if (terrain == null || objectPrefab == null)
        {
            Debug.LogError("Terrain �Ǵ� objectPrefab�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        foreach (var t in terrain)
        {
            TerrainData terrainData = t.terrainData;
            Vector3 terrainSize = terrainData.size;

            for (int i = 0; i < numberOfObjects; i++)
            {
                // �ͷ��� ���� ���� ���� x, z ��ǥ ���� (�ͷ����� ���� ��ǥ ����)
                float randomX = Random.Range(0f, terrainSize.x);
                float randomZ = Random.Range(0f, terrainSize.z);

                // ���� ��ǥ�� ��ȯ (�ͷ����� ���� ��ǥ�� �ٸ� ��ġ�� ���� ��� ���)
                Vector3 terrainPosition = t.transform.position;
                Vector3 samplePosition = new Vector3(randomX + terrainPosition.x, 0, randomZ + terrainPosition.z);

                // �ش� ��ġ�� ����(y) �� ���
                float y = t.SampleHeight(samplePosition) + terrainPosition.y;

                // ���� ��ġ ��ġ
                Vector3 finalPosition = new Vector3(samplePosition.x, y, samplePosition.z);

                // ������Ʈ ����
                Instantiate(RandomPrefab(), finalPosition, Quaternion.identity);
            }
        }
    }

    private GameObject RandomPrefab()
    {
        // �������� ������Ʈ �������� ����
        int randomIndex = Random.Range(0, objectPrefab.Length);
        return objectPrefab[randomIndex];
    }
}
