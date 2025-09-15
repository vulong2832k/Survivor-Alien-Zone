using UnityEngine;

[CreateAssetMenu(fileName = "RangerAttackSO", menuName = "EnemySO/RangerAttackSO")]
public class RangerAttackSO : EnemyAttackSO
{
    [SerializeField] private GameObject _bulletAttack;

    public override AttackResult EnemyAttack(Transform enemy, Transform target, int damage)
    {
        Transform firePoint = enemy.Find("FirePoint");
        if (firePoint == null) return null;

        Vector3 dir = (target.position - firePoint.position).normalized;

        GameObject bullet = MultiObjectPool.Instance.SpawnFromPool("EnemyBullet",firePoint.position,Quaternion.LookRotation(dir));

        if (bullet != null)
        {
            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(damage);
                bulletScript.SetDirection(dir);
            }
        }

        return new AttackResult
        {
            attacker = enemy,
            target = target,
            damage = damage
        };
    }
}
