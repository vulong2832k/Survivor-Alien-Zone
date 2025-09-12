using UnityEngine;

[CreateAssetMenu(fileName = "AmmoSO", menuName = "Inventory/AmmoSO")]
public class AmmoSO : ItemSO
{
    public GunType ammoForType;
    public float percentPerGun = 0.15f;
    public float percentAll = 0.05f;
}
