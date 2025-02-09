using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.AI;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace FallenAngel
{
    public class JobDriver_SocializeGathering : JobDriver
    {
        /// <summary>
        /// go to spot and chat
        /// randomly get up and wander around
        /// 
        /// </summary>
        private Pawn CurrentTarget => (job.GetTarget(TargetIndex.C).Thing) as Pawn;
        private Pawn currentTarget; 

        private int interactionTick = 0;

        private const TargetIndex GatherSpotParentInd = TargetIndex.A;

        private const TargetIndex ChairOrSpotInd = TargetIndex.B;

        private const TargetIndex socialablePawn = TargetIndex.C;

        private Thing GatherSpotParent => job.GetTarget(TargetIndex.A).Thing;
        private bool HasChair => job.GetTarget(TargetIndex.B).HasThing;

        

        private IntVec3 ClosestGatherSpotParentCell => GatherSpotParent.OccupiedRect().ClosestCellTo(pawn.Position);
        private IntVec3 socialablePawnPosition => (job.GetTarget(TargetIndex.C).Thing).Position;



        public override RandomSocialMode DesiredSocialMode()
        {
            return RandomSocialMode.SuperActive;
        }
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {

            if (!HasChair || !Toils_Ingest.TryFindFreeSittingSpotOnThing(job.GetTarget(TargetIndex.B).Thing, pawn, out var cell))
            {
                cell = job.GetTarget(TargetIndex.B).Cell;
            }
            if (!pawn.ReserveSittableOrSpot(cell, pawn.CurJob))
            {
                return false;
            }
            


            return true;
        }



        protected override IEnumerable<Toil> MakeNewToils()
        {
            currentTarget = CurrentTarget;
            this.EndOnDespawnedOrNull(TargetIndex.A);

            yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
            Toil toil = ToilMaker.MakeToil("MakeNewToils");
            toil.defaultDuration = job.def.joyDuration;
            SitAndBeSocial();
            currentTarget = null;
            FindNewTarget();
            if (currentTarget != null)
            {
                SitAndBeSocial();
            }

            yield return toil;

        }




        protected void FindNewTarget()
        {
            List<Pawn> pawns;
            SocialNeed_Utility.GetListOfPawnsInDistance(pawn, 10.9f, out pawns);
            if (pawns != null)
            {
                currentTarget = pawns.FirstOrDefault();
                
            }

        }




        protected IEnumerable<Toil> SitAndBeSocial()
        {
            this.FailOn(() => !SocialNeed_Utility.PawnInDistanceOf(pawn,currentTarget,7.9f));
            this.EndOnDespawnedOrNull(socialablePawn);
            if (HasChair)
            {
                this.EndOnDespawnedOrNull(TargetIndex.B);
            }
            yield return Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
            Toil toil = ToilMaker.MakeToil("MakeNewToils");
            
            toil.tickAction = delegate
            {
                pawn.rotationTracker.FaceCell(currentTarget.Position);
                
                    if (pawn.interactions.TryInteractWith(currentTarget, InteractionDefOf.Chitchat))
                    {
                        interactionTick = Find.TickManager.TicksGame;
                        if (Find.TickManager.TicksGame - interactionTick >= 300)
                        {
                            SocialNeed_Utility.OffsetSocialNeed(pawn, 0.005f);
                            SocialNeed_Utility.OffsetSocialNeed(currentTarget, 0.005f);

                        pawn.jobs.curDriver.ReadyForNextToil();
                        }
                    }
                Need_Social need = pawn?.needs?.TryGetNeed<Need_Social>();
                if (need != null && need.CurLevelPercentage > 0.9999f)
                {

                    EndJobWith(JobCondition.Succeeded);

                }


            };
            
            toil.socialMode = RandomSocialMode.SuperActive;
            toil.defaultCompleteMode = ToilCompleteMode.Never;

        }


       

    }
}
