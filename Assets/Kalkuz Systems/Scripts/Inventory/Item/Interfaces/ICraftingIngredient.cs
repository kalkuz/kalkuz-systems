using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Inventory.Item
{
    public interface ICraftingIngredient
    {
        IngredientType IngredientType
        {
            get;
            set;
        }

        bool IsSufficient(int amount);
    }
}
