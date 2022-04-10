using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Inventory.Item
{
    public abstract class UsableItem : Item, IConsumable
    {
        public abstract bool Consume(Battle.CharacterData targetCharacter);
    }
}
