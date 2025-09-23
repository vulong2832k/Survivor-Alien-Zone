using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDItemSp : MonoBehaviour
{
    [SerializeField] private Image _imageItemMedicine;
    [SerializeField] private Image _imageItemGrenade;
    [SerializeField] private TextMeshProUGUI _textItemMedicine;
    [SerializeField] private TextMeshProUGUI _textItemGrenade;

    [SerializeField] private Sprite _originImageItemMedicine;
    [SerializeField] private Sprite _originImageItemGrenade;

    private void Awake()
    {
        if (_imageItemMedicine == null || _imageItemGrenade == null || _textItemMedicine == null || _textItemGrenade == null)
        {
            _imageItemMedicine = GameObject.Find("ImageItemMedkit").GetComponent<Image>();
            _imageItemGrenade = GameObject.Find("ImageItemGrenade").GetComponent<Image>();
            _textItemMedicine = GameObject.Find("TextItemMedkit").GetComponent<TextMeshProUGUI>();
            _textItemGrenade = GameObject.Find("TextItemGrenade").GetComponent<TextMeshProUGUI>();
        }
    }
    private void Start()
    {
        var medicineSlot = EquipmentSystem.Instance.GetSlot(ItemType.Medicine);
        var grenadeSlot = EquipmentSystem.Instance.GetSlot(ItemType.Grenade);

        if (medicineSlot != null)
            medicineSlot.OnSlotChanged += UpdateMedicineHUD;

        if (grenadeSlot != null)
            grenadeSlot.OnSlotChanged += UpdateGrenadeHUD;
    }
    private void UpdateMedicineHUD(ItemSO item, int amount)
    {
        if (item == null || amount <= 0)
        {
            _imageItemMedicine.enabled = true;
            _imageItemMedicine.sprite = _originImageItemMedicine;
            _textItemMedicine.text = "0";
        }
        else
        {
            _imageItemMedicine.enabled = true;
            _imageItemMedicine.sprite = item.icon;
            _textItemMedicine.text = amount.ToString();
        }
    }

    private void UpdateGrenadeHUD(ItemSO item, int amount)
    {
        if (item == null || amount <= 0)
        {
            _imageItemGrenade.enabled = true;
            _imageItemGrenade.sprite = _originImageItemGrenade;
            _textItemGrenade.text = "0";
        }
        else
        {
            _imageItemGrenade.enabled = true;
            _imageItemGrenade.sprite = item.icon;
            _textItemGrenade.text = amount.ToString();
        }
    }
}
