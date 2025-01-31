using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace FallenAngel
{

    public class CompProperties_AbilityAffectionCost : CompProperties_AbilityEffect
    {
        public float affectionCost;

        public CompProperties_AbilityAffectionCost()
        {
            compClass = typeof(CompAbility_AffectionCost);
        }

        public override IEnumerable<string> ExtraStatSummary()
        {
            yield return (string)("AbilityaffectionCost".Translate() + ": ") + Mathf.RoundToInt(affectionCost * 100f);
        }
    }
}
