﻿using UnityEngine;
using UnityEngine.UI;

public class InventoryContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject rightClickMenu;
    public Button splitBtn;
    public Button dropBtn;

    private InventorySlotUI _currentSlot;
    [SerializeField] private Canvas _canvas;

    public static InventoryContextMenu Instance;

    private void Awake()
    {
        Instance = this;
        rightClickMenu.SetActive(false);

        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
        {
            _canvas = FindAnyObjectByType<Canvas>();
        }
    }

    public void Show(InventorySlotUI slotUI, Vector3 screenPos)
    {
        _currentSlot = slotUI;

        if (_canvas == null) return;

        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            _canvas.worldCamera,
            out Vector2 localPoint
        );

        rightClickMenu.transform.SetParent(_canvas.transform, false);
        rightClickMenu.GetComponent<RectTransform>().localPosition = localPoint;
        rightClickMenu.SetActive(true);

        splitBtn.onClick.RemoveAllListeners();
        dropBtn.onClick.RemoveAllListeners();

        splitBtn.onClick.AddListener(SplitStack);
        dropBtn.onClick.AddListener(DropItem);
    }

    public void Hide()
    {
        rightClickMenu.SetActive(false);
    }

    private void SplitStack()
    {
        if (_currentSlot == null) return;
        var slot = _currentSlot.GetSlot();
        if (slot == null || slot.IsEmpty || slot.amount < 2) return;

        _currentSlot.OnSplitClicked();
        Hide();
    }

    private void DropItem()
    {
        if (_currentSlot == null) return;
        var slot = _currentSlot.GetSlot();
        if (slot == null || slot.IsEmpty) return;

        slot.Clear();
        _currentSlot.UpdateUI();
        Hide();
    }
}
