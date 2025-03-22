using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InventoryData", menuName = "Inventory/InventoryData")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public void AddItem(ItemData_SO itemData,int amount)
    {
        bool found = false;
        if(itemData.stackable == true)
        { 
            foreach(InventoryItem item in items)
            {
                if (item.itemData!=null &&item.itemData.itemName == itemData.itemName)
                {
                    item.Amount += itemData.itemAmount;
                    found = true;
                    break;
                }
            }
        }
        
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].itemData==null&&!found)
                {
                    items[i].itemData = itemData;
                    items[i].Amount = itemData.itemAmount;
                    break;
                }
                
            }
        
    }
}
[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;
    public int Amount;
}