using System.Collections;
using KalkuzSystems.Analysis.Debugging;
using KalkuzSystems.Battle;
using KalkuzSystems.UI.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace KalkuzSystems.UI.Resources
{
    public sealed class HealthBar : ResourceBar
    {
        [SerializeField] private Image fill;
        
        public override void Initialize(CharacterData characterData, Vector3 followOffset)
        {
            if (characterData != null) bondCharacter = characterData;
            this.followOffset = followOffset;

            bondCharacter.OnResourceChanged.AddListener(UpdateBar);
            bondCharacter.OnDeath.AddListener((charData) =>
            {
                bondCharacter.OnResourceChanged.RemoveListener(UpdateBar);
                Destroy(gameObject);
            });
            
            UpdateBar(bondCharacter, ResourceType.HEALTH);
            KalkuzLogger.Info($"Character Health is: <color=cyan>{bondCharacter.Stats.CurrentStats.GetResource(ResourceType.HEALTH).Amount}</color>");
        }

        public override void UpdateBar(CharacterData charData, ResourceType resourceType)
        {
            if (resourceType != ResourceType.HEALTH) return;
            if (slideProcedure != null) StopCoroutine(slideProcedure);

            var stats = charData.Stats;
            var cHealth = stats.CurrentStats.GetResource(ResourceType.HEALTH);
            var bHealth = stats.BaseStats.GetResource(ResourceType.HEALTH);

            if (cHealth == null || bHealth == null)
            {
                KalkuzLogger.Warning("Stats have unregistered Health field. Make sure character has health in resources.");
                return;
            }

            if (bHealth.Amount == 0)
            {
                KalkuzLogger.Error("Base Health is zero. Can not update Health Bar.");
                return;
            }

            slideProcedure = StartCoroutine(SlideProcedure(cHealth.Amount / bHealth.Amount));
        }

        protected override IEnumerator SlideProcedure(float percent)
        {
            int iterator = 0;
            while (iterator < updateIterationMax)
            {
                fill.fillAmount = Mathf.Lerp(fill.fillAmount, percent, Mathf.InverseLerp(0, updateIterationMax, iterator));

                if (timeBetweenIterations == 0) yield return null;
                else yield return new WaitForSeconds(timeBetweenIterations);

                iterator++;
            }

            fill.fillAmount = percent;
        }
    }
}
