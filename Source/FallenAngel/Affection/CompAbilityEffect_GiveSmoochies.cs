using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
     public class CompAbilityEffect_GiveSmoochies : CompAbilityEffect
    {
        public new CompProperties_AbilityGiveSmoochies Props => (CompProperties_AbilityGiveSmoochies)props;
        private float affectioGain => Props.affectionGain * FallenAngel_ModSettings.affectionKissGainMultiplyer;
        private float addwoozy => Props.addWoozy;

        private bool showKissPopUpForLowOpinion => FallenAngel_ModSettings.disableOpinionPopUpForKissing;
        private string warning = "FA_Warning".Translate().Colorize(ColoredText.ThreatColor);


        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            
            Pawn pawn = target.Pawn;
            

            if (pawn != null)
            {
                ThoughtDef tempthought = (FallenAngel_Utility.InitiatorComfortableKissingRecipient(parent.pawn, pawn) ? Props.opinionThoughtDefToGiveInitiator : Props.opinionThoughtDefToGiveInitiator);
                FallenAngel_Utility.AbsorbAffection(parent.pawn, pawn, affectioGain, addwoozy, Props.thoughtDefToGiveRecipient, Props.opinionThoughtToGiveRecipient, Props.thoughtDefToGiveInitiator, tempthought, null);
                FallenAngel_Utility.DoKisses(parent.pawn, pawn);
                parent.pawn.AffectionFeeding();

            }
        }

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return Valid(target);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            Pawn pawn = target.Pawn;
            if (pawn == null)
            {
                return false;
            }
            if (!AbilityUtility.ValidateMustBeHumanOrWildMan(pawn, throwMessages, parent))
            {
                return false;
            }
            if (pawn.Faction != null)
            {
                if (pawn.Faction.HostileTo(parent.pawn.Faction))
                {
                    if (!pawn.Downed)
                    {
                        if (throwMessages)
                        {
                            Messages.Message("MessageCantUseOnResistingPerson".Translate(parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                        }
                        return false;
                    }
                }
                else if (pawn.IsQuestLodger() || pawn.Faction != parent.pawn.Faction)
                {
                    if (throwMessages)
                    {
                        Messages.Message("MessageCannotUseOnOtherFactions".Translate(parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                    }
                    return false;
                }
            }
            if (pawn.IsWildMan())
            {
                if (throwMessages)
                {
                    Messages.Message("MessageCantUseOnResistingPerson".Translate(parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if (pawn.InMentalState || PrisonBreakUtility.IsPrisonBreaking(pawn))
            {
                if (throwMessages)
                {
                    Messages.Message("MessageCantUseOnResistingPerson".Translate(parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if (ModsConfig.AnomalyActive && pawn.IsMutant && !pawn.mutant.Def.canBleed)
            {
                if (throwMessages)
                {
                    Messages.Message("MessageCannotUseOnNonBleeder".Translate(parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            return true;
        }

        public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
        {
            Pawn pawn = target.Pawn;
            if (pawn != null)
            {
                string additional = (FallenAngel_Utility.PreAplicationAffectionValue(parent.pawn, pawn, affectioGain,true)*100).ToString();
                //string additional = "uwu";
                string text = null;
                
                if (FallenAngel_Utility.WoodBeLifeThreateningToApply(pawn, addwoozy))
                {
                    text += "FA_DangerousToKissPerson".Translate(parent.def.Named("ABILITY"), pawn.Named("RECIPIENT"), warning.Named("WARNING"));
                }
                if (pawn.HostileTo(parent.pawn) && !pawn.Downed)
                {
                    text += "MessageCantUseOnResistingPerson".Translate(parent.def.Named("ABILITY"));
                }
                if (!FallenAngel_Utility.InitiatorComfortableKissingRecipient(parent.pawn, pawn))
                {
                    text += "FA_EmbarrassedWithKissingThisPerson".Translate(pawn.Named("RECIPIENT"), parent.pawn.Named("INITIATOR")) + additional ;
                }
                else
                {
                    text += "FA_OkayWithKissingThisPerson".Translate(pawn.Named("RECIPIENT"), parent.pawn.Named("INITIATOR")) + additional ;
                }
                    return text;
            }
            return base.ExtraLabelMouseAttachment(target);
        }

        public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
        {
            Pawn pawn = target.Pawn;
            if (pawn != null)
            {

                if (FallenAngel_Utility.WoodBeLifeThreateningToApply(pawn, addwoozy))
                {
                    return Dialog_MessageBox.CreateConfirmation("FA_WarningDangerousToKissWarningMessage".Translate(pawn.Named("RECIPIENT"), parent.def.Named("ABILITY"), warning.Named("WARNING")), confirmAction, destructive: true);
                }

                if (!FallenAngel_Utility.InitiatorComfortableKissingRecipient(parent.pawn, pawn)&&!showKissPopUpForLowOpinion)
                {
                    return Dialog_MessageBox.CreateConfirmation("FA_WarningPawnWillBeEmbarrassedToKissRecipientMessage".Translate(pawn.Named("RECIPIENT"),parent.pawn.Named("INITIATOR"), warning.Named("WARNING")), confirmAction, destructive: true);
                }
                
            }
            return null;
        }

        
    }
}
