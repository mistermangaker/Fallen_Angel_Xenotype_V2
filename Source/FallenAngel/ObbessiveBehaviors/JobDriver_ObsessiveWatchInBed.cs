using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace FallenAngel
{
    internal class JobDriver_ObsessiveWatchInBed : JobDriver
    {
        private Pawn Actor => GetActor();

        private Pawn TargetPawn => base.TargetThingA as Pawn;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOn(() => !TargetPawn.InBed() || TargetPawn.Awake());
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
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
