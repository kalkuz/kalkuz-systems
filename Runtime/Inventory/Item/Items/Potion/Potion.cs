using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Inventory.Item
{
    public abstract class Potion : UsableItem, IPotion
    {
        public abstract bool PotionBehaviour(Battle.CharacterData targetCharacter);
    }
}
