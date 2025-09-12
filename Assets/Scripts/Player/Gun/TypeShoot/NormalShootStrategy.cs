using UnityEngine;

public class NormalShootStrategy : IShootStrategy
{
    public void Shoot(Transform firePoint, Vector3 shootDirection, GunAttributes gunData, BulletPool pool)
    {
        GameObject bullet = pool.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);

        BulletPlayer bulletScript = bullet.GetComponent<BulletPlayer>();
        if (bulletScript != null)
            bulletScript.SetDamage(gunData.Damage);
    }
}
