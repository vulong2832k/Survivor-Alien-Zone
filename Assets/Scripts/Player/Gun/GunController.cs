using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [Header("Refs: ")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GunSOHolder _soHolder;
    [SerializeField] private IShootStrategy _shootStrategy;

    [Header("Bullet Key: ")]
    [SerializeField] private string _bulletKey = "BulletPlayer";

    [Header("Ammo: ")]
    private bool _isReloading = false;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _reserveAmmo;
    private float _fireCooldown;

    // Data lấy từ Holder
    public GunAttributes GunAttributes => _soHolder.GunAttributes;
    public GunType GunType => GunAttributes.GunType;

    public int CurrentAmmo => _currentAmmo;
    public int ReserveAmmo => _reserveAmmo;

    [Header("UI: ")]
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _ammoImage;

    [Header("Effects: ")]
    [SerializeField] private GameObject _muzzleFlashEffect;
    private ParticleSystem _muzzleFlashParticle;

    private void Awake()
    {
        if (_soHolder == null)
            _soHolder = GetComponentInChildren<GunSOHolder>();

        if (_firePoint == null)
            _firePoint = transform.Find("FirePoint");

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

    private void Start()
    {
        if (GunAttributes != null)
        {
            _currentAmmo = GunAttributes.Ammo;
            _reserveAmmo = GunAttributes.ReserveAmmo;
        }

        SetTypeShoot();
        UpdateAmmoUIText();
    }

    private void Update()
    {
        HandleShooting();
        HandleReload();
        UpdateAmmoUIImage();

        if (_fireCooldown > 0)
            _fireCooldown -= Time.deltaTime;
    }

    private void SetTypeShoot()
    {
        switch (GunType)
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
        if (Input.GetMouseButton(0) && CanFire())
        {
            Shooting();
        }
    }

    private bool CanFire() =>
        _fireCooldown <= 0 && !_isReloading && _currentAmmo > 0;

    private void Shooting()
    {
        _fireCooldown = GunAttributes.FireSpeed;
        _currentAmmo--;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 100f)
            ? hit.point
            : ray.GetPoint(100f);

        Vector3 shootDirection = (targetPoint - _firePoint.position).normalized;

        _shootStrategy?.Shoot(_firePoint, shootDirection, GunAttributes, _bulletKey);

        UpdateAmmoUIText();
        DisplayEffectMuzzleFlash();

        Debug.DrawRay(_firePoint.position, shootDirection * 100f, Color.red, 1f);
    }

    private void HandleReload()
    {
        if (_isReloading) return;

        if (Input.GetKeyDown(KeyCode.R) &&
            (_currentAmmo != GunAttributes.Ammo || _currentAmmo <= 0))
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        UpdateAmmoUIText(true);

        yield return new WaitForSeconds(GunAttributes.Reload);

        int ammoNeeded = GunAttributes.Ammo - _currentAmmo;

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

        _reserveAmmo = Mathf.Clamp(_reserveAmmo, 0, GunAttributes.MaxAmmo);

        _isReloading = false;
        UpdateAmmoUIText();
    }

    #region UI
    public void UpdateAmmoUIText(bool isReloading = false)
    {
        if (_ammoText == null) return;

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
    }

    public void AddReserveAmmo(int amount)
    {
        _reserveAmmo += amount;
        _reserveAmmo = Mathf.Clamp(_reserveAmmo, 0, GunAttributes.MaxAmmo);

        UpdateAmmoUIText();
    }

    private void UpdateAmmoUIImage()
    {
        if (_ammoImage == null || GunAttributes == null) return;

        float fill = 1f - (_fireCooldown / GunAttributes.FireSpeed);
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
