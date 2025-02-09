using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    public class CompAbility_OffsetSocialNeed : CompAbilityEffect
    {
        public new CompProperties_OffsetSocialNeed Props => (CompProperties_OffsetSocialNeed)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn pawn = target.Pawn;
            if (pawn != null)
            {
                if (Props.thoughtdefForInitator != null)
                {
                    parent.pawn.needs?.mood?.thoughts?.memories?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(Props.thoughtdefForInitator), pawn);
                }
                if (Props.thoughtdefForReciever != null)
                {
                    pawn.needs?.mood?.thoughts?.memories?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(Props.thoughtdefForInitator), parent.pawn);
                }
                SocialNeed_Utility.OffsetSocialNeed(parent.pawn, Props.offsetAmountForInitator);
                SocialNeed_Utility.OffsetSocialNeed(pawn, Props.offsetAmountForReciever);
            }

        }
    }
}
