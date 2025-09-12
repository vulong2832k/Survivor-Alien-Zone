using UnityEngine;

public enum GunType
{
    None = 0,
    Pistol,
    Shotgun,
    Rifle,
    Sniper,
    SMG,
    MG,
    Universal
}

[CreateAssetMenu(fileName = "GunAttributes", menuName = "GunSO/GunAttributes")]
public class GunAttributes : ScriptableObject
{
    [Header("Stats All: ")]
    [SerializeField] private string _name;
    [SerializeField] private int _damage;
    [SerializeField] private int _ammo;
    [SerializeField] private int _reserveAmmo;
    [SerializeField] private int _maxAmmoReserve;
    [SerializeField] private float _fireSpeed;
    [SerializeField] private float _reload;
    [SerializeField] private GameObject _gunPrefab;


    [Header("Stats Bonus Type Shotgun: ")]
    [SerializeField] public int _quantityPerShoot;
    [SerializeField] public float _speardAngle;

    [Header("Types: ")]
    [SerializeField] private GunType _gunType;

    [Header("Recoil Settings")]
    [SerializeField] private float _recoilX;
    [SerializeField] private float _recoilY;
    [SerializeField] private float _recoilMax;
    [SerializeField] private bool _invertY = false;


    public string Name => _name;
    public int Damage => _damage;
    public int Ammo => _ammo;
    public int ReserveAmmo => _reserveAmmo;
    public int MaxAmmo => _maxAmmoReserve;
    public float FireSpeed => _fireSpeed;
    public float Reload => _reload;
    public GunType GunType => _gunType;

    public GameObject GunPrefab => _gunPrefab;

    public float RecoilX => _recoilX;
    public float RecoilY => _recoilY;
    public float RecoilMax => _recoilMax;
    public bool InvertY => _invertY;

    //Type Shotgun
    public int QuantityPerShoot => _quantityPerShoot;
    public float SpeardAngle => _speardAngle;

}
