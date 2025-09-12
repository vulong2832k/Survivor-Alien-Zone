using UnityEngine;

public class ShotgunShootStrategy : IShootStrategy
{
    public void Shoot(Transform firePoint, Vector3 shootDirection, GunAttributes gunData, BulletPool pool)
    {
        int quantityPerShoot = gunData.QuantityPerShoot;
        float spearAngle = gunData.SpeardAngle;

        for(int i = 0; i < quantityPerShoot; i++)
        {
            Quaternion spreadRotation = Quaternion.Euler(
                Random.Range(-spearAngle, spearAngle),
                Random.Range(-spearAngle, spearAngle),
                0);

            Vector3 pelletDir = spreadRotation * shootDirection;

            GameObject bullet = pool.GetBullet();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(pelletDir);
            bullet.SetActive(true);

            BulletPlayer bulletPlayer = bullet.GetComponent<BulletPlayer>();
            if(bulletPlayer != null)
            {
                bulletPlayer.SetDamage(gunData.Damage / quantityPerShoot);
            }
        }
    }
}
