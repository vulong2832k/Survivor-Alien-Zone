using UnityEngine;

[CreateAssetMenu(fileName = "RangerAttackSO", menuName = "EnemySO/RangerAttackSO")]
public class RangerAttackSO : EnemyAttackSO
{
    [SerializeField] private GameObject _bulletAttack;
    [SerializeField] private int _damage;
    
    public override void EnemyAttack(Transform enemy, Transform target)
    {
        Transform firePoint = enemy.Find("FirePoint");
        if (firePoint == null) return;

        Vector3 dir = (target.position - firePoint.position).normalized;
        GameObject bullet = BulletPoolEnemy.Instance.SpawnBullet(
            firePoint.position,
            Quaternion.LookRotation(dir)
        );
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(_damage);
            bulletScript.SetDirection(dir);
        }
    }
}
