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
    
    public class JobDriver_TargetedInteractionSpree : JobDriver
    {
        private const TargetIndex TargetInd = TargetIndex.A;

        private Pawn Target => (Pawn)(Thing)pawn.CurJob.GetTarget(TargetIndex.A);

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
            yield return InsultingSpreeDelayToil();
            yield return Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
            Toil toil = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
            toil.socialMode = RandomSocialMode.Off;
            yield return toil;
            yield return InteractToil();
        }

        private Toil InteractToil()
        {
            return Toils_General.Do(delegate
            {
                if (pawn.interactions.TryInteractWith(Target, FA_InteractionDefOf.FA_TalkedAt) && pawn.MentalState is MentalState_TargetedInteractionSpree mentalState_TargetedInteractionSpree)
                {
                    mentalState_TargetedInteractionSpree.lastInteractionTicks = Find.TickManager.TicksGame;
                    if (mentalState_TargetedInteractionSpree.target == Target)
                    {
                        mentalState_TargetedInteractionSpree.interactedWithAtleastOnce = true;
                    }
                }
            });
        }

        private Toil InsultingSpreeDelayToil()
        {
            Action action = delegate
            {
                if (!(pawn.MentalState is MentalState_TargetedInteractionSpree mentalState_TargetedInteractionSpree) || Find.TickManager.TicksGame - mentalState_TargetedInteractionSpree.lastInteractionTicks >= 300)
                {
                    pawn.jobs.curDriver.ReadyForNextToil();
                }
            };
            Toil toil = ToilMaker.MakeToil("InsultingSpreeDelayToil");
            toil.initAction = action;
            toil.tickAction = action;
            toil.socialMode = RandomSocialMode.SuperActive;
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            return toil;
        }
    }
}
