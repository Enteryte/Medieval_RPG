using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database Base", menuName = "Scriptable Objects/Database/Item Database Base")]
public class ItemDatabaseBase : ScriptableObject, ISerializationCallbackReceiver
{
    [Tooltip("Array of all Items")] public ItemBaseProfile[] items;
    [Tooltip("Connection between Items and their corrosponding ID")] public Dictionary<ItemBaseProfile, int> GetID = new Dictionary<ItemBaseProfile, int>();
    [Tooltip("Connection between ID and their corrosponding Item")] public Dictionary<int, ItemBaseProfile> GetItem = new Dictionary<int, ItemBaseProfile>();

    public void OnAfterDeserialize()
    {
        GetID = new Dictionary<ItemBaseProfile, int>();
        GetItem = new Dictionary<int, ItemBaseProfile>();

        for (int i = 0; i < items.Length; i++)
        {
            GetID.Add(items[i], i);
            GetItem.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {

    }
}
