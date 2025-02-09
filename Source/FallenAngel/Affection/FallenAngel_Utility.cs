using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Noise;
using static UnityEngine.GraphicsBuffer;

namespace FallenAngel
{
    public static class FallenAngel_Utility
    {

        

        public static bool HasAffectionGene(Pawn pawn)
        {
            if (ModsConfig.BiotechActive && pawn.Faction.IsPlayerSafe())
            {
                Gene gene = GetAffectionGene(pawn);
                if (gene != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static int minOpinionToBeComfortableKissing => FallenAngel_ModSettings.defaultOpinionForNegativeMoodFromKissing;
        private static int affectionOpinionGainBuff => FallenAngel_ModSettings.defaultAffectionGainBuff;

        private static float affectionGeneralGainMultiplyer => FallenAngel_ModSettings.affectionGeneralGainMultiplyer;


        public static BodyPartRecord ComfortableKissLocations(Pawn initiator, Pawn recipient)
        {
            BodyPartRecord bodyPartRecord = null;

            if (InitiatorComfortableKissingRecipient(initiator, recipient))
            {

                bodyPartRecord = (from x in recipient.health.hediffSet.GetNotMissingParts()
                                  where (x.def == FA_DefOfSmoochLocations.Nose || x.def == FA_DefOfSmoochLocations.Head || 
                                  x.def == FA_DefOfSmoochLocations.Jaw || x.def == FA_DefOfSmoochLocations.Hand)
                                  orderby x.def.executionPartPriority descending
                                  select x).RandomElement();
            }
            else
            {
                bodyPartRecord = (from x in recipient.health.hediffSet.GetNotMissingParts()
                                  where (x.def == FA_DefOfSmoochLocations.Neck || x.def == FA_DefOfSmoochLocations.Head || 
                                  x.def == FA_DefOfSmoochLocations.Ear || x.def == FA_DefOfSmoochLocations.Nose || 
                                  x.def == FA_DefOfSmoochLocations.Jaw || x.def == FA_DefOfSmoochLocations.Hand || 
                                  x.def == FA_DefOfSmoochLocations.Clavicle || x.def == FA_DefOfSmoochLocations.Hand || 
                                  (initiator.relations.OpinionOf(recipient) >= 50) && x.def == FA_DefOfSmoochLocations.Tongue || 
                                  (initiator.relations.OpinionOf(recipient) >= 50) && x.def == FA_DefOfSmoochLocations.Waist)
                                   orderby x.def.executionPartPriority descending
                                  select x).RandomElement();
            }

            if (bodyPartRecord != null)
            {
                return bodyPartRecord;
            }

            Log.Error("No good execution cut part found for " + recipient);
            return recipient.health.hediffSet.GetNotMissingParts().RandomElementByWeight((BodyPartRecord x) => x.coverageAbsWithChildren);
        }


        public static void DoKisses(Pawn initiator, Pawn recipient)
        {
            int numOfKisses = (InitiatorComfortableKissingRecipient(initiator, recipient)? 1 : Rand.Range(2, 5));
            int num = 0;
            while(num < numOfKisses)
            {
                recipient.health.AddHediff(FA_HediffDefOf.FA_SmoochMarks, ComfortableKissLocations(initiator, recipient));
                num++;
            }
        }

        public static AcceptanceReport IsSafeToFeedOnPawn(Pawn bloodfeeder, Pawn prisoner)
        {
            if (prisoner.WoodBeLifeThreateningToApply(0.5f))
            {
                return "FA_CanNotConsumeAffectionLifeThreatening".Translate(prisoner.Named("PAWN"));
            }
            if (prisoner.IsForbidden(bloodfeeder) || !bloodfeeder.CanReserveAndReach(prisoner, PathEndMode.OnCell, bloodfeeder.NormalMaxDanger()) || prisoner.InAggroMentalState)
            {
                return false;
            }
            return true;
        }

        public static bool WoodBeLifeThreateningToApply(this Pawn pawn, float severity)
        {

            float num = severity;
            Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_FeelingWoozy);
            if (firstHediffOfDef != null)
            {
                num += firstHediffOfDef.Severity;
            }

            if (FA_HediffDefOf.FA_FeelingWoozy.stages[FA_HediffDefOf.FA_FeelingWoozy.StageAtSeverity(num)].lifeThreatening)
            {
                return true;
            }

            return false;
        }

        public static Gene_Affection GetAffectionGene(Pawn initator)
        {
            return initator.genes?.GetFirstGeneOfType<Gene_Affection>();
        }

        public static void AffectionFeeding(this Pawn initator)
        {
            Gene_Affection gene = GetAffectionGene(initator);
            gene.DoFeeding();
        }

        public static List<Pawn> ReturnFeedingListForInitator(Pawn initator, bool removeInitator)
        {
            Gene_Affection gene = GetAffectionGene(initator);
       
            List<Pawn> list = new List<Pawn>();
            if (gene.CanGetAffectionFromPrisoners) 
            {
                foreach(Pawn pawn in initator.Map.mapPawns.PrisonersOfColonySpawned)
                {
                    if(IsSafeToFeedOnPawn(initator, pawn))
                    {
                        list.Add(pawn);
                    }
                }

            }
            if (gene.CanGetAffectionFromColonists)
            {
                foreach (Pawn pawn in initator.Map.mapPawns.FreeColonistsSpawned)
                {
                    if (IsSafeToFeedOnPawn(initator, pawn))
                    {
                        list.Add(pawn);
                    }
                }
                
            }
            if (gene.CanGetAffectionFromSlaves)
            {
                foreach (Pawn pawn in initator.Map.mapPawns.SlavesOfColonySpawned)
                {
                    if (IsSafeToFeedOnPawn(initator, pawn))
                    {
                        list.Add(pawn);
                    }
                }
                
            }
            if(list.Contains(initator)&& removeInitator)
            {
                list.Remove(initator);
            }

            return list;
        }

        

        public static bool InitiatorComfortableKissingRecipient(Pawn initiator, Pawn recipient)
        {
            
            return initiator.relations.OpinionOf(recipient) >= minOpinionToBeComfortableKissing;

        }
       
        public static float SetOffsetByOpinion(Pawn initiator, Pawn recipient, float offset)
        {
            float InitiatorOpinionOfRecipient = initiator.relations.OpinionOf(recipient);
            float RecipientOpinionOfInitiator = recipient.relations.OpinionOf(initiator);
            float newoffset = offset + (offset*((InitiatorOpinionOfRecipient + RecipientOpinionOfInitiator) / 100));
            return newoffset;
        }
        
        


        public static void SetAffectionGain(Pawn pawn, float offset)
        {
            
            Gene_Affection gene_Hemogen = pawn.genes?.GetFirstGeneOfType<Gene_Affection>();
            if (gene_Hemogen != null)
            {
                
                OffsetAffection(pawn, offset);
                
            }
        }
        public static void OffsetAffection(Pawn pawn, float offset)
        {

            Gene_Affection gene_Hemogen = pawn.genes?.GetFirstGeneOfType<Gene_Affection>();

            if (gene_Hemogen != null)
            {
                gene_Hemogen.Value += offset;
            }
            
        }

        public static void TickResourceDrain(IGeneResourceDrain drain)
        {
            if (drain.CanOffset && drain.Resource != null)
            {
                OffsetResource(drain, (0f - drain.ResourceLossPerDay) / 60000f);
            }
        }

        public static void OffsetResource(IGeneResourceDrain drain, float offset)
        {
            if (drain.Resource != null)
            {
                float value = drain.Resource.Value;
                drain.Resource.Value += offset;
                PostResourceOffset(drain, value);
            }
        }

        public static void PostResourceOffset(IGeneResourceDrain drain, float oldValue)
        {
            if (oldValue > 0f && drain.Resource.Value <= 0f)
            {
                Pawn pawn = drain.Pawn;
                if (!pawn.health.hediffSet.HasHediff(FA_HediffDefOf.FA_AttentionCraving))
                {
                    pawn.health.AddHediff(FA_HediffDefOf.FA_AttentionCraving);
                }
            }
        }


        public static void OffsetAffectionSocial(Pawn pawn, float offset)
        {

            Gene_Affection gene_Hemogen = pawn.genes?.GetFirstGeneOfType<Gene_Affection>();

            if (gene_Hemogen != null)
            {
                gene_Hemogen.Value += (offset* pawn.GetStatValue(FA_StatDefOf.FA_AffectionGainFactor));
            }

        }



        public static float PreAplicationAffectionValue(Pawn initiator, Pawn recipient, float offset, bool applyStatFactor = true)
        {
           
            float InitiatorOpinionOfRecipient = initiator.relations.OpinionOf(recipient);
            float RecipientOpinionOfInitiator = recipient.relations.OpinionOf(initiator);
            float num1 = (InitiatorOpinionOfRecipient + RecipientOpinionOfInitiator + affectionOpinionGainBuff) / 100;
            // the code below prevents anything from sending the original value below -0.4 or above 2
            num1 = Mathf.Clamp(num1, -0.4f, 2);
            float num2 = 1 + num1;
            offset *= (num2 * affectionGeneralGainMultiplyer);
            if (offset > 0f && applyStatFactor)
            {
                offset *= initiator.GetStatValue(FA_StatDefOf.FA_AffectionGainFactor);
            }
            return offset;
        }

        public static void AbsorbAffection(Pawn initiator, Pawn recipient, float offset, float addWoozy, ThoughtDef thoughtDefToGiverecipient , ThoughtDef opinionThoughtToGiverecipient , ThoughtDef thoughtDefToGiveinitiator , ThoughtDef opinionThoughtToGiveinitiator, HediffDef hediffToGiveRecipient)
        {
            if (!ModsConfig.BiotechActive)
            {
                return;
            }
            if (initiator == null)
            {
                Log.Message("no initiator");
                return;
            }
            if (recipient == null)
            {
                Log.Message("no recipient");
                return;
            }
            //float newoffset = offset;
            float newoffset = PreAplicationAffectionValue(initiator, recipient,offset,true);
            

            SetAffectionGain(initiator, newoffset);
            SetAffectionGain(recipient, newoffset * -1.5f);
            if (thoughtDefToGiverecipient != null)
            {
                recipient.needs?.mood?.thoughts?.memories?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(thoughtDefToGiverecipient), initiator);
            }
            if (opinionThoughtToGiverecipient != null)
            {
                recipient.needs?.mood?.thoughts?.memories?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(opinionThoughtToGiverecipient), initiator);
            }
            if (thoughtDefToGiverecipient != null)
            {
                initiator.needs?.mood?.thoughts?.memories?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(thoughtDefToGiveinitiator), recipient);
            }
            if (opinionThoughtToGiverecipient != null)
            {
                initiator.needs?.mood?.thoughts?.memories?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(opinionThoughtToGiveinitiator), recipient);
            }
            if (hediffToGiveRecipient != null)
            {
                recipient.health.AddHediff(hediffToGiveRecipient);
            }
            if (addWoozy > 0f)
            {
                Hediff hediff = HediffMaker.MakeHediff(FA_HediffDefOf.FA_FeelingWoozy, recipient);
                hediff.Severity = addWoozy;
                recipient.health.AddHediff(hediff);
            }
        }
    }
}
