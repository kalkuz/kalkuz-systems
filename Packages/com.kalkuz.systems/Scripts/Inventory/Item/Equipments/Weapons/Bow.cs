using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Inventory.Equipment
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Item/Equipment/Weapon/Bow", fileName = "new Bow")]
    public class Bow : Weapon
    {
        public override Item.Item Clone()
        {
            return Instantiate(this);
        }
    }
}
