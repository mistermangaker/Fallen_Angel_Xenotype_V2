using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    public class CompProperties_HasCorrectGene : CompProperties_AbilityEffect
    {
        public GeneDef requiredGene;
        public CompProperties_HasCorrectGene()
        {
            compClass = typeof(CompAbility_HasCorrectGene);
        }
    }

    public class CompAbility_HasCorrectGene : CompAbilityEffect
    {
        public new CompProperties_HasCorrectGene Props => (CompProperties_HasCorrectGene)props;

        public override bool GizmoDisabled(out string reason)
        {
            if (Props.requiredGene != null && !parent.pawn.genes.HasActiveGene(Props.requiredGene))
            {
                reason = "AbilityDisabledNoHemogenGene".Translate(parent.pawn);
                return false;
            }
            
            
            reason = null;
            return false;
        }
    }
}
