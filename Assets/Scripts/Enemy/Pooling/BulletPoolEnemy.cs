using System.Collections.Generic;
using UnityEngine;

public class BulletPoolEnemy : MonoBehaviour
{
    public static BulletPoolEnemy Instance;

    [SerializeField] private GameObject _enemyBulletPrefab;
    [SerializeField] private int _initSize = 100;

    private Queue<GameObject> _bulletQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _initSize; i++)
        {
            GameObject bullet = Instantiate(_enemyBulletPrefab, transform);
            bullet.SetActive(false);
            _bulletQueue.Enqueue(bullet);
        }
    }

    public GameObject SpawnBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = (_bulletQueue.Count > 0) ? _bulletQueue.Dequeue() : Instantiate(_enemyBulletPrefab, transform);

        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);
        _bulletQueue.Enqueue(bullet);
    }
}
