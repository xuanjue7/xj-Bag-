using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject item;
        [Range(0, 1)]
        public float weight;

    }

    public LootItem[] lootItems;

    public void Spawnloot()
    {
        float currentValvue = Random.value;
        for (int i = 0; i < lootItems.Length; i++)
        {
            if (currentValvue <= lootItems[i].weight)
            {
                GameObject obj = Instantiate(lootItems[i].item);
                obj.transform.position = transform.position + Vector3.up * 2;
                break;//掉落一件物品后退出循环
            }
        }
    }
}
