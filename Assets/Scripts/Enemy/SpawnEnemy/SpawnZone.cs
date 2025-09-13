using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public string poolKey;
    public int count;
}

[RequireComponent(typeof(BoxCollider))]
public class SpawnZone : MonoBehaviour
{
    [SerializeField] private EnemySpawnInfo[] spawnList;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnce && hasTriggered) return;

        SpawnEnemies();
        hasTriggered = true;
    }

    private void SpawnEnemies()
    {
        foreach (var info in spawnList)
        {
            for (int i = 0; i < info.count; i++)
            {
                Vector3 pos = GetRandomPointInArea();
                GameObject enemy = MultiObjectPool.Instance.SpawnFromPool(info.poolKey,pos,Quaternion.identity);
            }
        }
    }

    private Vector3 GetRandomPointInArea()
    {
        if (spawnArea == null)
            spawnArea = transform;

        Vector3 size = spawnArea.localScale;
        Vector3 center = spawnArea.position;

        float x = Random.Range(-size.x / 2f, size.x / 2f);
        float z = Random.Range(-size.z / 2f, size.z / 2f);

        return new Vector3(center.x + x, center.y, center.z + z);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnArea.position, spawnArea.localScale);
        }
    }
}
