using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Attributes: ")]
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime = 3f;

    private int _damage;
    private Vector3 _direction;
    private float _lifeTimer;

    void OnEnable()
    {
        _lifeTimer = _lifeTime;
    }

    void Update()
    {
        BulletHandle();

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0f)
        {
            BulletPoolEnemy.Instance.ReturnBullet(gameObject);
        }
    }
    private void BulletHandle()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }
    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) return;

        if (other.CompareTag("Player"))
        {
            IDamageable damageable = other.transform.root.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(_damage);

            BulletPoolEnemy.Instance.ReturnBullet(gameObject);
        }
        else if (!other.isTrigger && !other.CompareTag("Enemy"))
        {
            BulletPoolEnemy.Instance.ReturnBullet(gameObject);
        }
    }

}
