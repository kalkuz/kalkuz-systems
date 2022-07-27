using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Inventory
{
    public interface IPotion
    {
        bool PotionBehaviour(Battle.CharacterData targetCharacter);
    }
}
