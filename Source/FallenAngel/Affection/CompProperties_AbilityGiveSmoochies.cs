using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{

    public class CompProperties_AbilityGiveSmoochies : CompProperties_AbilityEffect
    {
        public float affectionGain;

        public float addWoozy = 0.5f;

        public ThoughtDef thoughtDefToGiveRecipient;

        public ThoughtDef opinionThoughtToGiveRecipient;

        public ThoughtDef thoughtDefToGiveInitiator;

        public ThoughtDef opinionThoughtDefToGiveInitiator;

        public ThoughtDef thoughtDefToGiveInitiator_LowSocialOpinion;

        public CompProperties_AbilityGiveSmoochies()
        {
            compClass = typeof(CompAbilityEffect_GiveSmoochies);
        }
        public override IEnumerable<string> ExtraStatSummary()
        {
            yield return "FA_AffectionGain".Translate() + ": " + (affectionGain * 100f).ToString("F0");
        }
    }
}
