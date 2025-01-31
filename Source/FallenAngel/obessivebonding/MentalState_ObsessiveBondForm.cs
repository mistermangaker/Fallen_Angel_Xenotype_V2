using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using static UnityEngine.GraphicsBuffer;

namespace FallenAngel
{
   

    public class MentalBreakWorker_ObsessiveBondForm : MentalBreakWorker
    {
        public override bool BreakCanOccur(Pawn pawn)
        {
            
            if (pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>() == null)
            {
                return false;
            }
            Gene_ObsessiveBonding gene_ObsessiveBonding = pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>();
            Hediff affectionCravingHediff = pawn.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_AttentionCraving);
            if (ObsessiveBondingUtility.FindPawnToObsesseOver(pawn)==null)
            {
                return false;
            }
            Pawn target = ObsessiveBondingUtility.FindPawnToObsesseOver(pawn);
            if (pawn.IsColonist && pawn.Spawned && gene_ObsessiveBonding !=null && affectionCravingHediff.Severity >= 0.7 && target!=null)
            {
                return base.BreakCanOccur(pawn);
            }
            return false;
        }

        public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
        {
            Pawn target = ObsessiveBondingUtility.FindPawnToObsesseOver(pawn);
            Gene_ObsessiveBonding gene_ObsessiveBonding = pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>();
            gene_ObsessiveBonding.TryBondTo(target);
            TrySendLetter(pawn, "FA_LetterObssessiveBondFormed", reason);
            return true;
        }
    }
}
