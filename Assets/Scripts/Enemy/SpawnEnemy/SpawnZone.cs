using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
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
                EnemyPoolManager.Instance.SpawnEnemy(info.enemyPrefab, pos, Quaternion.identity);
            }
        }
    }

    private Vector3 GetRandomPointInArea()
    {
        Vector3 size = spawnArea.localScale;
        Vector3 center = spawnArea.position;

        float x = Random.Range(-size.x / 2, size.x / 2);
        float z = Random.Range(-size.z / 2, size.z / 2);

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
