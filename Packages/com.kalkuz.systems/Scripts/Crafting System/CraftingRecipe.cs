using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Inventory.Item;
using UnityEngine;

namespace KalkuzSystems.Crafting
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Crafting/Crafting Recipe", fileName = "new Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        [SerializeField] private List<CraftingInput> inputs;
        [SerializeField] private List<CraftingOutput> outputs;

        public bool Process(Inventory.Inventory inventory)
        {
            if (!CanCraft(inventory)) return false;
            if (!inventory.HasSpace(outputs.Count)) return false;

            foreach (CraftingInput ci in inputs)
            {
                inventory.SubtractItem(ci.item, ci.amount);
            }

            foreach (CraftingOutput co in outputs)
            {
                Item i = Instantiate(co.item);
                i.currentStacks = Mathf.Min(i.stackLimit, co.amount);
                inventory.AddItem(i);
            }

            return true;
        }

        public bool CanCraft(Inventory.Inventory inventory)
        {
            foreach (CraftingInput ci in inputs)
            {
                if (!inventory.HasItem(ci.item, ci.amount)) return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class CraftingInput
    {
        public CraftingIngredient item;
        public int amount;
    }
    [System.Serializable]
    public class CraftingOutput
    {
        public Item item;
        public int amount;
    }
}
