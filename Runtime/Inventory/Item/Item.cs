using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace KalkuzSystems.Inventory.Item
{
    public abstract class Item : ScriptableObject
    {
        [Delayed]
        public uint itemID;
        public ItemCategory itemCategory;
        public string itemName;
        [TextArea]
        public string description;
        public Sprite itemIcon;
        public int stackLimit;
        [HideInInspector] public int currentStacks;

        public int StackTogether(Item other)
        {
            if (other.itemID == itemID)
            {
                int total = currentStacks + other.currentStacks;
                currentStacks = Mathf.Min(total, stackLimit);
                return Mathf.Max(0, total - currentStacks);
            }
            else return other.currentStacks;
        }

        public int DecreaseStack(int amount)
        {
            if (amount <= currentStacks)
            {
                currentStacks -= amount;
                return 0;
            }
            else
            {
                int difference = amount - currentStacks;
                currentStacks = 0;
                return difference;
            }
        }

        public bool HasCapacity()
        {
            return currentStacks < stackLimit;
        }

        public override string ToString()
        {
            return $"Item {itemName} id: {itemID}\nDescription: {description}\nStack limit: {stackLimit}\nCurrent Stacks: {currentStacks}";
        }

        private void OnValidate()
        {
            if (!AssetDatabase.Contains(this)) return; //Means it is the script itself. We require the assets to perform this

            List<uint> itemIDs = new List<uint>();
            string[] guids = AssetDatabase.FindAssets("t:Item", new string[] { "Assets/Kalkuz Systems/Item Database" });

            foreach (string s in guids)
            {
                Item i = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(s));
                if (i.GetInstanceID() != GetInstanceID()) itemIDs.Add(i.itemID);
            }

            itemIDs.Sort();

            if (itemIDs.Contains(itemID))
            {
                for (int index = 0; index < itemIDs.Count; index++)
                {
                    if (itemIDs[index] != index)
                    {
                        itemID = (uint)index;
                        Debug.Log("Item ID Already Exists. New ID generated: " + index);
                        break;
                    }
                }
            }
        }
        public abstract Item Clone();
    }
    public enum ItemCategory
    {
        WEAPON, ARMOR, ACCESSORY, CONSUMABLES, MATERIALS, MISC, QUEST
    }
}