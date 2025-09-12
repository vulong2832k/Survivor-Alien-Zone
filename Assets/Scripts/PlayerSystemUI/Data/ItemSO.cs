using UnityEngine;
public enum ItemType
{
    Weapon,
    HeadArmor,
    Armor,
    Medicine,
    Grenade,
    Ammo,
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Inventory/ItemSO")]
public class ItemSO : ScriptableObject
{
    [Header("General Info")]
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [Header("Stacking")]
    public bool isStackable;
    public int maxStack;

    [Header("References")]
    public GunAttributes gunAttributes;

    private void OnValidate()
    {
        switch (itemType)
        {
            case ItemType.Weapon:
            case ItemType.HeadArmor:
            case ItemType.Armor:
                isStackable = false;
                maxStack = 1;
                break;
            case ItemType.Medicine:
            case ItemType.Grenade:
                isStackable = true;
                maxStack = 5;
                break;
            case ItemType.Ammo:
                isStackable = true;
                maxStack = 999;
                break;
        }
    }
}
