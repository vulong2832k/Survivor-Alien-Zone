using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private int _poolSize;

    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        if (_container == null)
        {
            _container = new GameObject("BulletPool").transform;
        }

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _container);
            bullet.SetActive(false);
            _pool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if(_pool.Count == 0)
        {
            GameObject extraBullet = Object.Instantiate( _bulletPrefab, _container);
            extraBullet.SetActive(false);
            _pool.Enqueue(extraBullet);
        }

        GameObject bullet = _pool.Dequeue();
        _pool.Enqueue(bullet);
        return bullet;
    }
}
