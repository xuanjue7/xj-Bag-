using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //专门实现拖拽的功能 使其所挂载的物体可以拖拽

    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;
    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        //记录原始数据 如果没有拖拽到合适的地方则复位，此处完成 存储信息的功能
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);

    }

    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标移动
        //todo:找到鼠标拖动时偏移的原因后将偏移修正 在item的预制体中将 image位置修正就好了
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品交换数据
        //松开鼠标时是否在ui上
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckInActionUI(eventData.position) ||
                InventoryManager.Instance.CheckInInventoryUI(eventData.position) ||
                InventoryManager.Instance.CheckInEquipmentUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    //只有获得了targetholder才能更新ui
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                else
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();

                //判断是否目标holder为我的原holder
                if (targetHolder != InventoryManager.Instance.currentDrag.originalHolder)
                    switch (targetHolder.slotType)
                    {
                        case SlotType.BAG:
                            SwapItem();
                            break;
                        case SlotType.WEAPON:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                                SwapItem();
                            break;
                        case SlotType.ARMOR:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                                SwapItem();
                            break;
                        case SlotType.ACTION:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Useable)
                                SwapItem();
                            break;
                    }
                currentHolder.UpdateUI();
                targetHolder.UpdateUI();
            }
        }
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        //更换格子发现 itemslot位移了
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];
        //判断是不是一个物品
        bool isSameItem = tempItem.itemData == targetItem.itemData;
        if (isSameItem && targetItem.itemData.stackable)
        {
            targetItem.Amount += tempItem.Amount;
            tempItem.itemData = null;
            tempItem.Amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
