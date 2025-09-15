using JetBrains.Annotations;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [Header("Attributes: ")]
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float lifeTimer = 3f;

    [Header("Effects: ")]
    [SerializeField] private string _hitEffectKey = "HitEffect";

    private float _timer;

    private void OnEnable()
    {
        _timer = 0f;    
    }

    void Update()
    {
        BulletController();
    }
    private void BulletController()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        _timer += Time.deltaTime;
        if(_timer >= lifeTimer)
            MultiObjectPool.Instance.ReturnToPool("PlayerBullet", gameObject);
    }
    private void EffectHitToWall()
    {
        GameObject effect = MultiObjectPool.Instance.SpawnFromPool(_hitEffectKey, transform.position, Quaternion.identity);
    }
    public void SetDamage(int damage) => this._damage = damage;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
            MultiObjectPool.Instance.ReturnToPool("PlayerBullet", gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Ground") && !other.CompareTag("Bullet"))
        {
            EffectHitToWall();
            MultiObjectPool.Instance.ReturnToPool("PlayerBullet", gameObject);
        }
    }
}
