using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;

    [System.Serializable]
    public class EnemyPool
    {
        public GameObject enemyPrefab;
        public int initSize;
        public Queue<GameObject> poolQueue = new Queue<GameObject>();
    }

    [SerializeField] private EnemyPool[] enemyPools;
    [SerializeField] private Transform _poolEnemy;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        if (_poolEnemy == null)
        {
            _poolEnemy = new GameObject("PoolEnemy").transform;
        }

        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var pool in enemyPools)
        {
            for (int i = 0; i < pool.initSize; i++)
            {
                GameObject obj = Instantiate(pool.enemyPrefab, _poolEnemy);
                obj.SetActive(false);

                var enemyCtrl = obj.GetComponent<EnemyController>();
                if (enemyCtrl != null)
                    enemyCtrl.SetPrefabReference(pool.enemyPrefab);

                pool.poolQueue.Enqueue(obj);
            }
        }
    }
    public GameObject SpawnEnemy(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        EnemyPool pool = GetPool(prefab);
        if(pool == null) return null;

        GameObject obj = (pool.poolQueue.Count > 0) ? pool.poolQueue.Dequeue() : Instantiate(prefab, _poolEnemy);
        var enemyCtrl = obj.GetComponent<EnemyController>();
        if (enemyCtrl != null)
            enemyCtrl.SetPrefabReference(prefab);

        obj.transform.SetParent(_poolEnemy);
        obj.transform.position = pos;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }
    public void ReturnToPool(GameObject prefab, GameObject enemyObj)
    {
        EnemyPool pool = GetPool(prefab);
        if (pool == null)
        {
            Destroy(enemyObj);
            return;
        }
        enemyObj.SetActive(false);
        pool.poolQueue.Enqueue(enemyObj);
;
    }
    private EnemyPool GetPool(GameObject prefab)
    {
        foreach (var pool in enemyPools)
        {
            if (pool.enemyPrefab == prefab) return pool;
        }
        return null;
    }
}
