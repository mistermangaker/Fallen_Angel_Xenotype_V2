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
    public class ObsessiveBondingUtility
    {
        private static List<Pawn> candiateTargets = new List<Pawn>();

        // the target colonist needs to atleast 16 years 
        // the initiator needs to be attracted to them
        // not be incest
        // if the initator is already in a relationship it should be them

        public static bool AttractedToGender(Pawn pawn, Gender gender)
        {
            Pawn_StoryTracker story = pawn.story;
            if (story != null && story.traits?.HasTrait(TraitDefOf.Asexual) == true)
            {
                return false;
            }
            Pawn_StoryTracker story2 = pawn.story;
            if (story2 != null && story2.traits?.HasTrait(TraitDefOf.Bisexual) == true)
            {
                return true;
            }
            Pawn_StoryTracker story3 = pawn.story;
            if (story3 != null && story3.traits?.HasTrait(TraitDefOf.Gay) == true)
            {
                return pawn.gender == gender;
            }
            return pawn.gender != gender;
        }
        private static bool Incestuous(Pawn one, Pawn two)
        {
            foreach (PawnRelationDef relation in one.GetRelations(two))
            {
                if (relation.romanceChanceFactor != 1f)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsElibibleForObsession(Pawn initiator, Pawn target)
        {
            if (initiator == target)
            {
                return false;
            }
            if (!AttractedToGender(initiator, target.gender))
                {
                return false;
            }
            if (target.relations.OpinionOf(initiator) <= 5)
            {
                return false;
            }
            if (Incestuous(initiator, target))
            {
                return false;
            }
            return true;
        }


        // get candiates for obsession()
        

        public static Pawn FindPawnToObsesseOver(Pawn pawn)
        {
            if (!pawn.Spawned)
            {
                return null;
            }
            candiateTargets.Clear();
            IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.FreeAdultColonistsSpawned;
            for (int i = 0; i < allPawnsSpawned.Count; i++)
            {
                Pawn pawn2 = allPawnsSpawned[i];
                


                if (IsElibibleForObsession(pawn, pawn2))
                {
                    candiateTargets.Add(pawn2);
                }
            }
            if (!candiateTargets.Any())
            {
                return null;
            }
            Pawn result = candiateTargets.RandomElement();
            candiateTargets.Clear();
            return result;
        }

        // iseligable for devouring()


    }
}
