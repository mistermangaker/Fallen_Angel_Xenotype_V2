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
    public class JoyGiver_ObsessionMaster : JoyGiver
    {
        
        public override Job TryGiveJob(Pawn pawn)
        {
            Gene_ObsessiveBonding gene_ObsessiveBonding = pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>();
            if (gene_ObsessiveBonding == null || gene_ObsessiveBonding.bondedPawn == null)
            {
                return null;
            }
            Pawn target = gene_ObsessiveBonding.bondedPawn;
            if (target.Awake() && !target.InBed())
            {
                return JobMaker.MakeJob(FA_JobDefOf.FA_ObsessiveFollow, target);
            }
            if (target.InBed())
            {
                return JobMaker.MakeJob(FA_JobDefOf.FA_ObsessiveWatchInBed, target);
            }
            List<Pawn> rivals = ObsessiveBehaviors_Utility.GetAllObsessorsRivals(target, pawn);
            if (rivals != null)
            {
                // for actions to harm the obsessors rivals
                return null;
            }
            return null;
        }
    }
}
