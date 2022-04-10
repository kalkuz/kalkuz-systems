using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle.SkillSystem;
using UnityEngine;

namespace KalkuzSystems.Inventory.Equipment
{
    public abstract class Weapon : Equipment
    {
        [Space]
        public SkillManager basicAttack;
    }
}
