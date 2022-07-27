using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Inventory.Equipment
{
    public abstract class Equipment : Item.Item
    {
        public enum EquipmentRarity { COMMON, UNCOMMON, MAGIC, RARE, EPIC, LEGENDARY, MYTHIC, UNIQUE }

        [Header("Equipment Stats")]
        public EquipmentRarity rarity;
        public AnimationCurve rarityEmpowerCurve;
    }
}
