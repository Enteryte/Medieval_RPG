using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "New Inventory Base", menuName = "Scriptable Objects/Inventory/Inventory Base")]
public class InventoryBaseProfile : ScriptableObject, ISerializationCallbackReceiver
{
    [Tooltip("Database of Items")] public ItemDatabaseBase database;
    [Tooltip("List of all Inventory Slots")] public List<InventorySlot> slots = new List<InventorySlot>();

    /// <summary>
    /// Standart Unity Funktion
    /// </summary>
    public void OnEnable()
    {
        //#if UNITY_EDITOR
        //        database = (ItemDatabaseBase)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseBase));
        //#else
        //database = Resources.Load<ItemDatabaseBase>("Database");
        //#endif

        if (database == null)
        {
            Debug.Log("ItemDatabase of inventory is null!");
        }
    }

    /// <summary>
    /// Adds item's to your Inventory
    /// </summary>
    /// <param name="item"> Item </param>
    /// <param name="amount"> Amount of the Item</param>
    public void AddItem(ItemBaseProfile item, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemBase == item)
            {
                if (slots[i].itemBase.stackable)
                {
                    slots[i].AddAmount(amount);
                    return;
                }
            }
        }

        slots.Add(new InventorySlot(database.GetID[item], item, amount));
    }

    /// <summary>
    /// Removes an item from your Inventory
    /// </summary>
    /// <param name="item"> Item </param>
    /// <param name="amount"> Amount of the Item </param>
    public void RemoveItem(ItemBaseProfile item, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemBase == item)
            {
                if (slots[i].itemBase.stackable)
                {
                    slots[i].RemoveAmount(amount);

                    if (slots[i].itemAmount == 0)
                    {
                        slots.Remove(slots[i]);
                    }

                    return;
                }
            }
        }

        slots.Remove(new InventorySlot(database.GetID[item], item, amount));
    }

    /// <summary>
    /// Standart Unity Funktion
    /// </summary>
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].itemBase = database.GetItem[slots[i].itemID];
        }
    }

    /// <summary>
    /// Standart Unity Funktion
    /// </summary>
    public void OnBeforeSerialize()
    {

    }
}

[System.Serializable]
public class InventorySlot : IComparable
{
    [Tooltip("Index of the Item")] public int itemID;
    [Tooltip("Base Item")] public ItemBaseProfile itemBase;
    [Tooltip("Amount of the Item")] public int itemAmount;

    /// <summary>
    /// Konstruktor for an InventorySlot
    /// </summary>
    /// <param name="id"> Index of the Item </param>
    /// <param name="item"> Itembase </param>
    /// <param name="amount"> Amount of the Item </param>
    public InventorySlot(int id, ItemBaseProfile item, int amount)
    {
        itemID = id;
        itemBase = item;
        itemAmount = amount;
    }

    /// <summary>
    /// Adding Items to your Inventory
    /// </summary>
    /// <param name="value"> Amount </param>
    public void AddAmount(int value)
    {
        itemAmount += value;
    }

    /// <summary>
    /// Removes Items from your Inventory
    /// </summary>
    /// <param name="value"> Amount </param>
    public void RemoveAmount(int value)
    {
        itemAmount -= value;
    }

    /// <summary>
    /// Compares 2 items and sorts them
    /// </summary>
    /// <param name="obj"> Object you want to compare with </param>
    public int CompareTo(object obj)
    {
        if (obj is InventorySlot)
        {
            return this.itemBase.itemType.CompareTo((obj as InventorySlot).itemBase.itemType);
        }

        throw new ArgumentException("Object is not an item.");
    }
}

