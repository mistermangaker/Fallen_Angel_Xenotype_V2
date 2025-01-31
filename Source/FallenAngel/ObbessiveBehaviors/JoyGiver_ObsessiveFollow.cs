using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace FallenAngel
{
    public class JoyGiver_ObsessiveFollow : JoyGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            Gene_ObsessiveBonding gene_ObsessiveBonding = pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>();
            if (gene_ObsessiveBonding == null || gene_ObsessiveBonding.bondedPawn == null)
            {
                return null;
            }
            return JobMaker.MakeJob(FA_JobDefOf.FA_ObsessiveFollow, gene_ObsessiveBonding.bondedPawn);
        }
    }
}
