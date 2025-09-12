using JetBrains.Annotations;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [Header("Attributes: ")]
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float lifeTimer = 3f;

    [Header("Effects: ")]
    [SerializeField] private GameObject _effectHitToWall;

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
            gameObject.SetActive(false);
    }
    private void EffectHitToWall()
    {
        if(_effectHitToWall != null)
        {
            GameObject effect = Instantiate(_effectHitToWall, transform.position, _effectHitToWall.transform.rotation);
            Destroy(effect, 2f);
        }
    }
    public void SetDamage(int damage) => this._damage = damage;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Va chạm với: " + other.gameObject.name);

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
            gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Ground") && !other.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
            EffectHitToWall();
        }
    }
}
