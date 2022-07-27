using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle;
using UnityEngine;

namespace KalkuzSystems.Inventory.Item
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Item/Consumable/Potion/Health Potion", fileName = "new Health Potion")]
    public class HealthPotion : Potion
    {
        public Battle.DamageApplicationType healingType;
        public float healAmount;

        public override Item Clone()
        {
            return Instantiate(this);
        }

        public override bool Consume(CharacterData targetCharacter)
        {
            if (targetCharacter == null) return false;
            return PotionBehaviour(targetCharacter);
        }

        public override bool PotionBehaviour(CharacterData targetCharacter)
        {
            if (targetCharacter.Stats.CurrentStats.GetResource(ResourceType.HEALTH).Amount < targetCharacter.Stats.BaseStats.GetResource(ResourceType.HEALTH).Amount)
            {
                targetCharacter.Stats.Heal(healAmount, healingType);
                DecreaseStack(1);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
