using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KalkuzSystems.Inventory
{
    [System.Serializable]
    public class Inventory : MonoBehaviour
    {
        public Item.Item[] items;
        public int sizeX, sizeY;

        [Header("Playground")]
        public int _x;
        public int _y;
        public Item.Item _item;
        public int itemStacks;
        public Crafting.CraftingRecipe recipe;

        private void Awake()
        {
            items = new Item.Item[sizeX * sizeY];
        }

        public bool SetItem(int x, int y, Item.Item item)
        {
            if (x < 0 || x > sizeX || y < 0 || y > sizeY) return false;
            else
            {
                items[x + y * sizeX] = item == null ? null : item.Clone();
                return true;
            }
        }
        public Item.Item GetItem(int x, int y)
        {
            if (x < 0 || x > sizeX || y < 0 || y > sizeY) throw new System.IndexOutOfRangeException();
            else return items[x + y * sizeX];
        }

        public Item.Item PopItem(int x, int y)
        {
            if (x < 0 || x > sizeX || y < 0 || y > sizeY) throw new System.IndexOutOfRangeException();

            Item.Item result = GetItem(x, y);
            DeleteItem(x, y);
            return result;
        }

        public Item.Item ReplaceItem(int x, int y, Item.Item item)
        {
            if (x < 0 || x > sizeX || y < 0 || y > sizeY) throw new System.IndexOutOfRangeException();

            Item.Item toBeReplaced = GetItem(x, y);
            if (toBeReplaced != null && item != null && item.itemID == toBeReplaced.itemID)
            {
                int remaining = toBeReplaced.StackTogether(item);
                if (remaining == 0) return null;
                else
                {
                    item.currentStacks = remaining;
                    return item;
                }
            }
            else
            {
                Item.Item pop = PopItem(x, y);
                SetItem(x, y, item);
                return pop;
            }
        }

        public bool DeleteItem(int x, int y)
        {
            if (x < 0 || x > sizeX || y < 0 || y > sizeY) throw new System.IndexOutOfRangeException();
            return SetItem(x, y, null);
        }

        public bool SubtractItem(Item.Item item, int amount)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (items[x + y * sizeX] != null && items[x + y * sizeX].itemID == item.itemID)
                    {
                        amount = items[x + y * sizeX].DecreaseStack(amount);
                        if (amount == 0) return true;
                        else DeleteItem(x, y);
                    }
                }
            }
            return false;
        }

        public bool AddItem(Item.Item item)
        {
            item = item.Clone();
            Vector2Int firstEmptySpace = Vector2Int.one * -1;
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (items[x + y * sizeX] == null)
                    {
                        if (firstEmptySpace == Vector2.one * -1) firstEmptySpace = new Vector2Int(x, y);
                    }
                    else if (items[x + y * sizeX].itemID == item.itemID && items[x + y * sizeX].HasCapacity())
                    {
                        int remaining = items[x + y * sizeX].StackTogether(item);
                        if (remaining == 0) return true;
                        else
                        {
                            item.currentStacks = remaining;
                            continue;
                        }
                    }
                }
            }
            return SetItem(firstEmptySpace.x, firstEmptySpace.y, item);
        }

        public bool HasItem(Item.Item item, int amount)
        {
            int totalAmount = 0;
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (items[x + y * sizeX] != null && items[x + y * sizeX].itemID == item.itemID)
                    {
                        totalAmount += items[x + y * sizeX].currentStacks;
                        if (totalAmount >= amount) return true;
                    }
                }
            }
            return false;
        }

        public bool HasSpace(int amount)
        {
            int count = 0;
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (items[x + y * sizeX] == null) count++;
                    if (count >= amount) return true;
                }
            }
            return false;
        }

        public void SortInventory(bool descending = false)
        {
            if (descending)
            {
                items = items.OrderBy(item => item == null)
                .ThenByDescending(item => item != null ? item.itemCategory : Item.ItemCategory.QUEST + 1)
                .ThenByDescending(item => item != null ? item.itemName : string.Empty).ToArray();
            }
            else
            {
                items = items.OrderBy(item => item == null)
                .ThenBy(item => item != null ? item.itemCategory : Item.ItemCategory.QUEST + 1)
                .ThenBy(item => item != null ? item.itemName : string.Empty).ToArray();
            }
        }
    }
}
