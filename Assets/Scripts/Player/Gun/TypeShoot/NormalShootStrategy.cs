using UnityEngine;

public class NormalShootStrategy : IShootStrategy
{
    public void Shoot(Transform firePoint, Vector3 shootDirection, GunAttributes gunData, string bulletKey)
    {
        GameObject bullet = MultiObjectPool.Instance.SpawnFromPool(
            bulletKey,
            firePoint.position,
            firePoint.rotation
        );

        if (bullet != null)
        {
            BulletPlayer bulletScript = bullet.GetComponent<BulletPlayer>();
            if (bulletScript != null)
                bulletScript.SetDamage(gunData.Damage);
        }
    }
}
