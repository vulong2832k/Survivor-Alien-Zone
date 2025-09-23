using UnityEngine;

public class PlantBomCondition : MonoBehaviour, IWinCondition
{
    [SerializeField] private Transform _plantZone;
    [SerializeField] private ItemSO requiredItem;

    private bool isPlanted = false;

    public void StartCondition()
    {
        isPlanted = false;
    }

    public bool IsCompleted()
    {
        return isPlanted;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        InventorySystem inv = other.GetComponent<InventorySystem>();
        if (inv != null && HasItem(inv, requiredItem))
        {
            inv.RemoveItem(requiredItem, 1);
            isPlanted = true;
            Debug.Log("Đã đặt bom ở đây");
        }
    }

    private bool HasItem(InventorySystem inv, ItemSO item)
    {
        foreach (var slot in inv.slots)
        {
            if (!slot.IsEmpty && slot.item == item && slot.amount > 0)
                return true;
        }
        return false;
    }
}
