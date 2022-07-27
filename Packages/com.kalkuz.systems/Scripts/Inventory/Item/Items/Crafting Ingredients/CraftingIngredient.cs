using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Inventory.Item
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Item/Crafting Ingredient", fileName = "new Crafting Ingredient")]
    public class CraftingIngredient : Item, ICraftingIngredient
    {
        [Header("Properties")]
        [SerializeField] private IngredientType ingredientType;

        public IngredientType IngredientType
        {
            get { return ingredientType; }
            set { ingredientType = value; }
        }

        public override Item Clone()
        {
            return Instantiate(this);
        }

        public bool IsSufficient(int amount)
        {
            return amount <= currentStacks;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nIngredient Type: {IngredientType}";
        }
    }

    public enum IngredientType { WOOD, WOODEN_STICK, IRON_BAR, IRON_ORE }
}
