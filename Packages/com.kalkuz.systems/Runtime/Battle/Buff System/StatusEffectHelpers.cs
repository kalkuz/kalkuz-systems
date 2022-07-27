using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.BuffSystem
{
    public class StatusEffectHelpers
    {
        public static List<StatusEffect> movementPreventers = new List<StatusEffect>() {
            StatusEffect.CHANNELING,
            StatusEffect.ENTANGLED,
            StatusEffect.FROZEN,
            StatusEffect.PINNED,
            StatusEffect.STUNNED
        };
        public static List<StatusEffect> actionPreventers = new List<StatusEffect>() {
            StatusEffect.FROZEN,
            StatusEffect.STUNNED
        };
    }
}
