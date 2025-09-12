using UnityEngine;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private int _slotCount;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        slots.Clear();
        for (int i = 0; i < _slotCount; i++)
        {
            slots.Add(new InventorySlot());
        }
    }
    public int AddItem(ItemSO item, int amount)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.item == item && slot.amount < item.maxStack)
                {
                    int canAdd = Mathf.Min(amount, item.maxStack - slot.amount);
                    slot.amount += canAdd;
                    amount -= canAdd;
                    if (amount <= 0) return 0;
                }
            }
        }
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                int canAdd = Mathf.Min(amount, item.maxStack);
                slot.AssignItem(item, canAdd);
                amount -= canAdd;
                if (amount <= 0) return 0;
            }
        }

        return amount;
    }

    public void RemoveItem(ItemSO item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.amount -= amount;
                if(slot.amount <= 0) slot.Clear();
                return;
            }
        }
    }
    
}
