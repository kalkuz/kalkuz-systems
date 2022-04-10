using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    /// <summary>
    /// The Attachment that is used to improve several <see cref="UsableSkill"/>'s capability.
    /// </summary>
    public abstract class SkillUpgrade : Skill
    {
        /// <summary>
        /// Applies the improvement onto the given <paramref name="skill"/>. It may behave multi-purpose depending on how it was implemented.
        /// </summary>
        /// <param name="skill">The skill to be enhanced</param>
        public abstract void ApplyUpgrade(UsableSkill skill);
    }
}
