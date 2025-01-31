using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace FallenAngel
{

    public class JobGiver_TargetedInteractionSpree : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (!(pawn.MentalState is MentalState_TargetedInteractionSpree mentalState_TargetedInteractionSpree) || !pawn.CanReach(mentalState_TargetedInteractionSpree.target, PathEndMode.Touch, Danger.Deadly))
            {
                Log.Message("ohnyo");
                return null;
            }
            if (!InteractionUtility.BestInteractableCell(pawn, mentalState_TargetedInteractionSpree.target).IsValid)
            {
                return null;
            }

            return JobMaker.MakeJob(FA_JobDefOf.FA_TalkAtTarget, mentalState_TargetedInteractionSpree.target);
        }
    }
}
