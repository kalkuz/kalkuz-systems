using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Inventory
{
    public interface IConsumable
    {
        bool Consume(Battle.CharacterData targetCharacter);
    }
}
