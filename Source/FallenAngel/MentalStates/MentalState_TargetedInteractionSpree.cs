using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using Verse.AI;
using Verse;

namespace FallenAngel
{

    public class MentalState_TargetedInteractionSpree : MentalState_TargetedInteraction
    {
        private static List<Pawn> candidates = new List<Pawn>();
        public override string InspectLine => string.Format(def.baseInspectLine, target.LabelShort);

        protected override bool CanEndBeforeMaxDurationNow => interactedWithAtleastOnce;
        public override void MentalStateTick()
        {
            if (target != null && (!target.Spawned || !base.pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly) || !target.Awake()))
            {

                Pawn pawn = target;
                if (!TryFindNewTarget())
                {
                    Log.Message("ending state");
                    RecoverFromState();
                    return;
                }
                Messages.Message("MessageTargetedInteractionSpreeChangedTarget".Translate(base.pawn.LabelShort, pawn.Label, target.Label, base.pawn.Named("PAWN"), pawn.Named("OLDTARGET"), target.Named("TARGET")).AdjustedFor(base.pawn), base.pawn, MessageTypeDefOf.NegativeEvent);
                base.MentalStateTick();
            }
            else if (target == null || !InsultingSpreeMentalStateUtility.CanChaseAndInsult(base.pawn, target, skipReachabilityCheck: false, allowPrisoners: false))
            {
                RecoverFromState();
            }
            else
            {
                base.MentalStateTick();
            }
        }

        public override void PreStart()
        {
            
            base.PreStart();
            TryFindNewTarget();
        }
        private bool TryFindNewTarget()
        {
            
            InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, candidates, allowPrisoners: false);
            bool result = candidates.TryRandomElement(out target);
            


            candidates.Clear();
            return result;
        }

        public override void PostEnd()
        {
            
            base.PostEnd();
            if (pawn.health.hediffSet.HasHediff(FA_HediffDefOf.FA_AttentionCraving))
            {
                HealthUtility.AdjustSeverity(pawn, FA_HediffDefOf.FA_AttentionCraving, -0.1f);
            }


            if (target != null && PawnUtility.ShouldSendNotificationAbout(pawn))
            {
                Messages.Message("MessageNoLongerOnTargetedInteractionSpree".Translate(pawn.LabelShort, target.Label, pawn.Named("PAWN"), target.Named("TARGET")), pawn, MessageTypeDefOf.SituationResolved);
            }
        }
        public override TaggedString GetBeginLetterText()
        {
            if (target == null)
            {
                Log.Error("No target. This should have been checked in this mental state's worker.");
                return "";
            }
            return def.beginLetter.Formatted(pawn.NameShortColored, target.NameShortColored, pawn.Named("PAWN"), target.Named("TARGET")).AdjustedFor(pawn).Resolve()
                .CapitalizeFirst();
        }
    }
}
