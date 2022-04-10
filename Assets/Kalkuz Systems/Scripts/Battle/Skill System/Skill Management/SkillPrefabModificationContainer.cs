using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [System.Serializable]
    public class SkillPrefabModificationContainer
    {
        public SkillUpgrade upgradeType;
        public GameObject prefabReplacement;
        public Sprite spriteReplacement;
    }
}

