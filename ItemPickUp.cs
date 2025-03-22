using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //todo: add item to bag
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //×°±¸ÎäÆ÷
            //GameManager.Instance.playerStats.EquipWeapon(itemData);
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName, itemData.itemAmount);
            Destroy(gameObject);
        }
    }
}
