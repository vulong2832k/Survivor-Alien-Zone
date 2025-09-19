using System;
using UnityEngine;

public abstract class EnemyBulletBase : MonoBehaviour
{
    [SerializeField] protected BulletEnemySO _data;

    protected float _lifeTimer;
    protected Vector3 _direction;

    private int _currentDamage;

    protected virtual void OnEnable()
    {
        _lifeTimer = _data.lifeTime;
    }

    protected virtual void Update()
    {
        HandleMovement();

        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer <= 0f)
        {
            OnLifeTimeExpried();
        }
    }

    protected abstract void HandleMovement();

    protected virtual void OnLifeTimeExpried()
    {
        Explode();
        MultiObjectPool.Instance.ReturnToPool(_data.poolKey, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.isTrigger) return;

        if (!_data.explosive && other.CompareTag("Player") && other.transform.root.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_currentDamage);
        }
        Explode();
        MultiObjectPool.Instance.ReturnToPool(_data.poolKey, gameObject);
    }
    protected virtual void Explode()
    {
        if (_data.explosive)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _data.explosionRadius);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IDamageable target))
                {
                    target.TakeDamage(_currentDamage);
                }
            }
        }
    }
    public virtual void Init(Vector3 direction, int damage)
    {
        _direction = direction.normalized;
        _currentDamage = damage;
    }
}
