using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    public class ThoughtWorker_ObsessiveHappiness : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            
            if (!ModsConfig.BiotechActive)
            {
                return ThoughtState.Inactive;
            }
            Hediff_ObsessiveBond_Receiver hediff_PsychicBond = p.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_ObssesiveBondReceiver) as Hediff_ObsessiveBond_Receiver;
            if (hediff_PsychicBond?.target == null)
            {
                return ThoughtState.Inactive;
            }
            Pawn bondedPawn = hediff_PsychicBond?.target as Pawn;
            if (ObsessiveBehaviors_Utility.IsLover(bondedPawn, p))
            {
                return ThoughtState.Inactive;
            }
            if (bondedPawn.needs.mood.CurLevelPercentage <= 0.1f)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            if (bondedPawn.needs.mood.CurLevelPercentage <= 0.3f)
            {
                return ThoughtState.ActiveAtStage(1);
            }
            if (bondedPawn.needs.mood.CurLevelPercentage <= 0.5f)
            {
                return ThoughtState.ActiveAtStage(2);
            }
            return ThoughtState.ActiveAtStage(3);
        }
        
    }
}
