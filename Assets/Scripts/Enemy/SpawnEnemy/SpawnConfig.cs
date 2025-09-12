using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int spawnCount;
}

[CreateAssetMenu(fileName = "SpawnConfig", menuName = "Enemy/Spawn Config")]
public class SpawnConfig : ScriptableObject
{
    public EnemySpawnData[] enemies;
}
