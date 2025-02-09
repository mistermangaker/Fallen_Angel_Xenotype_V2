using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using static HarmonyLib.Code;

namespace FallenAngel
{
    public static class SocialNeed_Utility
    {
        public static void OffsetSocialNeed(Pawn pawn, float offset)
        {
            Need_Social need = pawn?.needs?.TryGetNeed<Need_Social>();
            if (need != null)
            {
                need.CurLevel += offset;
            }
        }

        public static Gene GetSocializerGene(Pawn initator)
        {
            return initator.genes?.GetGene(FA_GeneDefOf.Gene_Loving);
                
        }
        public static bool PawnInDistanceOf(Pawn initiator, Pawn recipient, float distance)
        {
            return (initiator.Position.DistanceTo(recipient.Position) <= distance);
        }
        public static void GetListOfPawnsInDistance(Pawn pawn, float distance, out List<Pawn> list)
        {
            list = null;
            IReadOnlyList<Pawn> readOnlyList = pawn.Map.mapPawns.AllPawnsSpawned;
            foreach (Pawn item in readOnlyList)
            {
                if (item.RaceProps.Humanlike || !item.Dead || item.health != null || item != pawn || item.Position.DistanceTo(pawn.Position) <= distance)
                {
                    list.Add(item);
                }
            }
            return;
        }

        public static void GivePychicEffectInRange(Pawn pawn, float distance, HediffDef hediff = null,float initialSeverity = 1, ThoughtDef thoughtdef = null)
        {
            if (!pawn.Awake() || pawn.health == null || pawn.health.InPainShock || !pawn.Spawned)
            {
                return;
            }
            IReadOnlyList<Pawn> readOnlyList = pawn.Map.mapPawns.AllPawnsSpawned;
            foreach (Pawn item in readOnlyList)
            {
                if (!item.RaceProps.Humanlike || item.Dead || item.health == null || item == pawn || !(item.Position.DistanceTo(pawn.Position) <= distance))
                {
                    continue;
                }
                if (hediff != null)
                {
                    Hediff hediff2 = item.health.hediffSet.GetFirstHediffOfDef(hediff);
                    if (hediff2 == null)
                    {
                        hediff2 = item.health.AddHediff(hediff, item.health.hediffSet.GetBrain());
                        hediff2.Severity = initialSeverity;
                        
                    }
                }
                if (thoughtdef != null)
                {
                    item.needs?.mood?.thoughts.memories.TryGainMemory(thoughtdef);
                }


            }


        }


    }
}
