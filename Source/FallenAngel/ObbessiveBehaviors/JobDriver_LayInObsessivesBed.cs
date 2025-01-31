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
    public class JobDriver_LayInObsessivesBed : JobDriver
    {

        private int tickstimer = 0;
        private const TargetIndex bedlocation = TargetIndex.A;
        private Pawn TargetPawn => base.TargetThingB as Pawn;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }


        protected  IEnumerable<Toil> MakeNewToilsss()
        {
            this.FailOn(() => TargetPawn.InBed() || TargetPawn.Awake());
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);

            Toil toil = (job.forceSleep ? Toils_LayDown.LayDown(bedlocation, hasBed: false, lookForOtherJobs: false) : ToilMaker.MakeToil("MakeNewToils"));
            toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
            {

                base.Map.pawnDestinationReservationManager.Reserve(pawn, job, pawn.Position);
                pawn.pather?.StopDead();

            });
            
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            yield return toil;
        }

        protected IEnumerable<Toil> LayInBed()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
        }


        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOn(() => TargetPawn.InBed());
            IntVec3 destination = TargetPawn.ownership.OwnedBed.Position;
            yield return Toils_Goto.GotoThing(bedlocation, PathEndMode.OnCell);
            Toil stareAtTarget = ToilMaker.MakeToil("MakeNewToils");
            stareAtTarget.tickAction = delegate
            {
                pawn.rotationTracker.FaceCell(TargetPawn.Position);
            };
            stareAtTarget.handlingFacing = true;
            stareAtTarget.socialMode = RandomSocialMode.Off;
            stareAtTarget.defaultCompleteMode = ToilCompleteMode.Delay;
            stareAtTarget.defaultDuration = job.def.joyDuration;

            yield return stareAtTarget;

        }

    }

    



}
