using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace FallenAngel
{
    public static class ObsessiveBehaviors_Utility
    {
        private static List<DirectPawnRelation> allloversList = new List<DirectPawnRelation>();

        
        public static List<DirectPawnRelation> GetAllLovers(Pawn target)
        {
            allloversList.Clear();
            List<DirectPawnRelation> lovers = target.relations.DirectRelations;
            for (int i = 0; i < lovers.Count; i++)
            {
                if (lovers[i].def == PawnRelationDefOf.Lover || lovers[i].def == PawnRelationDefOf.Spouse || lovers[i].def == PawnRelationDefOf.Fiance)
                {
                    allloversList.Add(lovers[i]);
                }
            }
            return allloversList;
        }

        public static bool IsLover(Pawn target, Pawn obsessor)
        {
            
            foreach (DirectPawnRelation relation in GetAllLovers(target))
            {
                if (relation.otherPawn == obsessor)
                {
                    return true;
                }
            }
            return false;

           
        }
        public static List<Pawn> GetAllObsessorsRivals(Pawn target,Pawn obsessor)
        {
            List<Pawn> list = new List<Pawn>();
            foreach (DirectPawnRelation relation in GetAllLovers(target))
            {
                if (!list.Contains(relation.otherPawn) && (target.Map == relation.otherPawn.Map) && (relation.otherPawn!= obsessor))
                {
                    list.Add(relation.otherPawn);
                }
            }
            return list;
            
        }
    }
}
