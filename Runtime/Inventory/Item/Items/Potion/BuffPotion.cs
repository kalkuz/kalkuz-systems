using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle;
using KalkuzSystems.Battle.BuffSystem;
using UnityEngine;

namespace KalkuzSystems.Inventory.Item
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Item/Consumable/Potion/Buff Potion", fileName = "new Buff Potion")]
    public class BuffPotion : Potion
    {
        public List<Buff> buffs;

        public override Item Clone()
        {
            var clone = Instantiate(this);
            clone.buffs = new List<Buff>(buffs);
            return clone;
        }

        public override bool Consume(CharacterData targetCharacter)
        {
            if (targetCharacter == null) return false;
            else return PotionBehaviour(targetCharacter);
        }

        public override bool PotionBehaviour(CharacterData targetCharacter)
        {
            foreach (Buff buff in buffs)
            {
                buff.Clone().Inflict(targetCharacter, null);
            }

            DecreaseStack(1);
            return true;
        }
    }
}
