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
    
    public abstract class MentalState_TargetedInteraction : MentalState
    {
        public Pawn target;

        public bool interactedWithAtleastOnce;

        public int lastInteractionTicks = -999999;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref target, "target");
            Scribe_Values.Look(ref interactedWithAtleastOnce, "interactedWithAtleastOnce", defaultValue: false);
            Scribe_Values.Look(ref lastInteractionTicks, "lastInteractionTicks", 0);
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Normal;
        }
    }
}
