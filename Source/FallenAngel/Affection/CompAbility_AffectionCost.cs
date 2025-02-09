using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{

    public class CompAbility_AffectionCost : CompAbilityEffect
    {
        public new CompProperties_AbilityAffectionCost Props => (CompProperties_AbilityAffectionCost)props;
        public List<AbilityComp> comps;
        private bool HasEnoughHemogen
        {
            get
            {
                Gene_Affection gene_Hemogen = parent.pawn.genes?.GetFirstGeneOfType<Gene_Affection>();
                if (gene_Hemogen == null || gene_Hemogen.Value < Props.affectionCost)
                {
                    return false;
                }
                return true;
            }
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            FallenAngel_Utility.OffsetAffection(parent.pawn, 0f - Props.affectionCost);
          
            //GeneUtility.OffsetHemogen(parent.pawn, 0f - Props.affectionCost);
        }

        public override bool GizmoDisabled(out string reason)
        {
            Gene_Affection gene_Hemogen = parent.pawn.genes?.GetFirstGeneOfType<Gene_Affection>();
            if (gene_Hemogen == null)
            {
                reason = "AbilityDisabledNoHemogenGene".Translate(parent.pawn);
                return true;
            }
            if (gene_Hemogen.Value < Props.affectionCost)
            {
                reason = "AbilityDisabledNoHemogen".Translate(parent.pawn);
                return true;
            }
            float num = TotalaffectionCostOfQueuedAbilities();
            float num2 = Props.affectionCost + num;
            if (Props.affectionCost > float.Epsilon && num2 > gene_Hemogen.Value)
            {
                reason = "AbilityDisabledNoHemogen".Translate(parent.pawn);
                return true;
            }
            reason = null;
            return false;
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return HasEnoughHemogen;
        }

        private float TotalaffectionCostOfQueuedAbilities()
        {
            float num = ((!(parent.pawn.jobs?.curJob?.verbToUse is Verb_CastAbility verb_CastAbility)) ? 0f : (affectionCost(verb_CastAbility)));
            if (parent.pawn.jobs != null)
            {
                for (int i = 0; i < parent.pawn.jobs.jobQueue.Count; i++)
                {
                    if (parent.pawn.jobs.jobQueue[i].job.verbToUse is Verb_CastAbility verb_CastAbility2)
                    {
                        num += affectionCost(verb_CastAbility2);
                    }
                }
            }
            return num;
        }
        public float affectionCost(Verb_CastAbility verb_CastAbility2)
        {
            if (comps != null)
            {
                foreach (AbilityComp comp in verb_CastAbility2.ability?.comps)
                {
                    if (comp is CompAbility_AffectionCost compAbilityEffect_affectionCost)
                    {
                        return compAbilityEffect_affectionCost.Props.affectionCost;
                    }
                }
            }
            return 0f;
        }

    }
}
