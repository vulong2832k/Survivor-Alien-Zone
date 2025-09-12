using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    [Header("Gun: ")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GunAttributes _gunAttributes;
    [SerializeField] private IShootStrategy _shootStrategy;

    public GunAttributes GunAttributes
    {
        get { return _gunAttributes; }
    }


    public GunType GunType
    {
        get
        {
            if (_gunAttributes == null)
            {
                return GunType.None;
            }
            return _gunAttributes.GunType;
        }
    }

    private bool _isReloading = false;
    [SerializeField] private int _currentAmmo;
    public int CurrentAmmo => _currentAmmo;

    [Header("Recoil:")]
    private float _recoilIntensityCounter = 0;

    [Header("Bullet: ")]
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private int _reserveAmmo;
    public int ReserveAmmo => _reserveAmmo;
    private float _fireCooldown;

    [Header("Components: ")]
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _ammoImage;

    [Header("Effects: ")]
    [SerializeField] private GameObject _muzzleFlashEffect;
    private ParticleSystem _muzzleFlashParticle;

    private void OnEnable()
    {
        var weaponSwitching = FindAnyObjectByType<WeaponSwitching>();
        if (weaponSwitching != null && weaponSwitching.CurrentGun == this)
        {
            UpdateAmmoUIText();
            UpdateAmmoUIImage();
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        _isReloading = false;
    }
    private void Awake()
    {
        GetComponentWhenAwake();
    }
    void Start()
    {
        if (_gunAttributes != null && _currentAmmo <= 0)
            _currentAmmo = _gunAttributes.Ammo;

        _currentAmmo = _gunAttributes.Ammo;
        _reserveAmmo = _gunAttributes.ReserveAmmo;

        SetTypeShoot();
        UpdateAmmoUIText();
    }

    void Update()
    {
        HandleShooting();
        HandleReload();
        UpdateAmmoUIImage();

        if (_recoilIntensityCounter > 0)
            _recoilIntensityCounter -= Time.deltaTime * 5f;
        else
            _recoilIntensityCounter = 0;

    }
    private void GetComponentWhenAwake()
    {
        if (_firePoint == null)
            _firePoint = transform.Find("FirePoint");

        if (_gunAttributes == null)
        {
            GunSOHolder holder = GetComponentInChildren<GunSOHolder>();
            if (holder != null)
                _gunAttributes = holder.GunAttributes;
        }

        if (_bulletPool == null)
            _bulletPool = FindAnyObjectByType<BulletPool>();

        if (_ammoText == null)
        {
            GameObject ammoTextObj = GameObject.Find("AmmoText");
            if (ammoTextObj != null)
                _ammoText = ammoTextObj.GetComponent<TextMeshProUGUI>();
        }

        if (_ammoImage == null)
        {
            GameObject ammoImgObj = GameObject.Find("AmmoImage");
            if (ammoImgObj != null)
                _ammoImage = ammoImgObj.GetComponent<Image>();
        }

        if (_muzzleFlashEffect != null)
            _muzzleFlashParticle = _muzzleFlashEffect.GetComponent<ParticleSystem>();
    }
    private void SetTypeShoot()
    {
        switch (_gunAttributes.GunType)
        {
            case GunType.Shotgun:
                _shootStrategy = new ShotgunShootStrategy();
                break;
            default:
                _shootStrategy = new NormalShootStrategy();
                break;
        }
    }
    private void HandleShooting()
    {
        if(Input.GetMouseButton(0) && CanFire())
        {
            Shooting();
        }
        _fireCooldown -= Time.deltaTime;
    }
    private bool CanFire() => _fireCooldown <= 0 && !_isReloading && _currentAmmo > 0;
    private void Shooting()
    {
        _fireCooldown = _gunAttributes.FireSpeed;
        _currentAmmo--;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100f);

        Vector3 shootDirection = (targetPoint - _firePoint.position).normalized;

        _shootStrategy?.Shoot(_firePoint, shootDirection, _gunAttributes, _bulletPool);

        _recoilIntensityCounter++;
        if (_recoilIntensityCounter > _gunAttributes.RecoilMax)
            _recoilIntensityCounter = _gunAttributes.RecoilMax;

        UpdateAmmoUIText();
        DisplayEffectMuzzleFlash();
        Debug.DrawRay(_firePoint.position, shootDirection * 100f, Color.red, 1f);
    }

    private void HandleReload()
    {
        if (_isReloading) return;

        if (Input.GetKeyDown(KeyCode.R) && _currentAmmo != _gunAttributes.Ammo || _currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }
    private IEnumerator Reload()
    {
        _isReloading = true;
        UpdateAmmoUIText(true);

        yield return new WaitForSeconds(_gunAttributes.Reload);
        int ammoNeeded = _gunAttributes.Ammo - _currentAmmo;

        if (_reserveAmmo >= ammoNeeded)
        {
            _currentAmmo += ammoNeeded;
            _reserveAmmo -= ammoNeeded;
        }
        else
        {
            _currentAmmo += _reserveAmmo;
            _reserveAmmo = 0;
        }

        _reserveAmmo = Mathf.Clamp(_reserveAmmo, 0, _gunAttributes.MaxAmmo);

        _isReloading = false;
        UpdateAmmoUIText();
    }
    private Vector2 GetRecoilScreenPoint()
    {
        _recoilIntensityCounter++;
        if (_recoilIntensityCounter > _gunAttributes.RecoilMax)
            _recoilIntensityCounter = _gunAttributes.RecoilMax;

        float xOffset = Random.Range(0, _recoilIntensityCounter) * _gunAttributes.RecoilX;
        float yOffset = Random.Range(0, _recoilIntensityCounter) * _gunAttributes.RecoilY;

        if (Random.value < 0.5f) xOffset = -xOffset;
        if (_gunAttributes.InvertY && Random.value < 0.5f) yOffset = -yOffset;

        float x = (Screen.width / 2f) + xOffset;
        float y = (Screen.height / 2f) + yOffset;

        return new Vector2(x, y);
    }
    #region UI
    public void UpdateAmmoUIText(bool isReloading = false)
    {
        if (_ammoText != null)
        {
            if (isReloading)
            {
                _ammoText.text = "Reloading...";
                _ammoText.color = Color.yellow;
            }
            else
            {
                _ammoText.text = $"{_currentAmmo:D3} / {_reserveAmmo:D3}";
                _ammoText.color = (_reserveAmmo == 0 && _currentAmmo == 0) ? Color.red : Color.white;
            }
            if(_currentAmmo == 0 && _reserveAmmo == 0)
            {
                _ammoText.text = "00 / 00";
                _ammoText.color = Color.red;
            }
        }
    }
    public void AddReserveAmmo(int amount)
    {
        _reserveAmmo += amount;
        _reserveAmmo = Mathf.Clamp(_reserveAmmo, 0, _gunAttributes.MaxAmmo);

        var weaponSwitching = FindAnyObjectByType<WeaponSwitching>();
        if (weaponSwitching != null && weaponSwitching.CurrentGun == this)
        {
            UpdateAmmoUIText();
        }
    }

    private void UpdateAmmoUIImage()
    {
        if (_ammoImage == null || _gunAttributes == null) return;

        float fill = 1f - (_fireCooldown / _gunAttributes.FireSpeed);
        _ammoImage.fillAmount = Mathf.Clamp01(fill);
    }
    #endregion
    #region Effect
    private void DisplayEffectMuzzleFlash()
    {
        if (_muzzleFlashParticle != null)
        {
            _muzzleFlashParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _muzzleFlashParticle.Play();
        }
    }
    #endregion
}
