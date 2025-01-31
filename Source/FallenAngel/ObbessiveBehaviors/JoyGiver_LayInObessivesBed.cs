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

    
    public class JoyGiver_LayInObessivesBed : JoyGiver
    {
        
        //public IntVec3 destination;
       
        public override Job TryGiveJob(Pawn pawn)
        {
            Gene_ObsessiveBonding gene_ObsessiveBonding = pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>();
            if (gene_ObsessiveBonding == null || gene_ObsessiveBonding.bondedPawn == null)
            {
                return null;
            }
            Pawn target = gene_ObsessiveBonding.bondedPawn;
            if (target.ownership.OwnedBed != null&& target.Awake())
            {
                Thing destination = target.ownership.OwnedBed;
                //return JobMaker.MakeJob(FA_JobDefOf.FA_ObsessiveSleepInOthersBed, destination);
                return JobMaker.MakeJob(def.jobDef, destination,target);
            }
            return null;
        }
    }
}
