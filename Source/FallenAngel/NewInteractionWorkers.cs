using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    //id rather not have to overide these classes but i dont know how else to add new override methods to classes
    //remove if a better solution is found
    public class InteractionWorker_Chitchat : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (initiator.Inhumanized())
            {
                return 0f;
            }
            return 1f;
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            SocialNeed_Utility.OffsetSocialNeed(initiator, 0.05f);
            SocialNeed_Utility.OffsetSocialNeed(recipient, 0.05f);
            base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef, out lookTargets);
        }
    }

    public class InteractionWorker_DeepTalk : InteractionWorker
    {
        private const float BaseSelectionWeight = 0.075f;

        private static readonly SimpleCurve CompatibilityFactorCurve = new SimpleCurve
    {
        new CurvePoint(-1.5f, 0f),
        new CurvePoint(-0.5f, 0.1f),
        new CurvePoint(0.5f, 1f),
        new CurvePoint(1f, 1.8f),
        new CurvePoint(2f, 3f)
    };

        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (initiator.Inhumanized())
            {
                return 0f;
            }
            return 0.075f * CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            SocialNeed_Utility.OffsetSocialNeed(initiator, 0.2f);
            SocialNeed_Utility.OffsetSocialNeed(recipient, 0.2f);
            base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef, out lookTargets);
        }
    }

    public class InteractionWorker_KindWords : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (initiator.Inhumanized())
            {
                return 0f;
            }
            Trait trait = initiator.story.traits.GetTrait(TraitDefOf.Kind);
            if (trait != null && !trait.Suppressed)
            {
                return 0.01f;
            }
            return 0f;
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            SocialNeed_Utility.OffsetSocialNeed(initiator, 0.5f);
            SocialNeed_Utility.OffsetSocialNeed(recipient, 0.5f);
            base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef, out lookTargets);
        }
    }
}
