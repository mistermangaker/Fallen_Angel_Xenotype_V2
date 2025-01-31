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
    
    public class JobDriver_GetAffection : JobDriver
    {
        public const float BloodLoss = 0.4499f;

        public const int WaitTicks = 120;

        

        protected Pawn Prisoner => (Pawn)job.targetA.Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        public static Toil GoGetAffection(Pawn pawn, Pawn talkee)
        {
            Toil toil = ToilMaker.MakeToil("GoGetAffection");
            
            toil.initAction = delegate
            {
                pawn.pather.StartPath(talkee, PathEndMode.Touch);
            };
            toil.AddFailCondition(delegate
            {
                if (talkee.DestroyedOrNull())
                {
                    return true;
                }
                return false;
            });
            
            toil.socialMode = RandomSocialMode.Off;
            toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
            return toil;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {        
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOn(() => Prisoner.InAggroMentalState);
            yield return JobDriver_GetAffection.GoGetAffection(pawn, Prisoner);
            Gene_Affection pawngene = pawn.genes?.GetFirstGeneOfType<Gene_Affection>();
            
            yield return Toils_General.WaitWith(TargetIndex.A, 120, useProgressBar: true).PlaySustainerOrSound(SoundDefOf.Bloodfeed_Cast);
            yield return Toils_General.Do(delegate
            {
                FallenAngel_Utility.AbsorbAffection(pawn, Prisoner, 0.1f,0.2f, FA_ThoughtDefOf.FA_FedOnBy, FA_ThoughtDefOf.FA_FedOnBy_Social, FA_ThoughtDefOf.FA_GainedAffectionFrom, FA_ThoughtDefOf.FA_GainedAffectionFrom_Social, FA_HediffDefOf.FA_FullBodyHug);
            });
            pawngene.DoFeeding();
            yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
        }
    }

}
