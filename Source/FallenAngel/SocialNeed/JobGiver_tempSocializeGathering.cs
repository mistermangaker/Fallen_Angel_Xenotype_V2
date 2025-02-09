using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using System.Security.Cryptography;
using UnityEngine;

namespace FallenAngel.SocialNeed
{
  
    public class JobGiver_TargetedInteractionSpree : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Need_Social need = pawn?.needs?.TryGetNeed<Need_Social>();
            if (need != null&& need.CurLevelPercentage <= 0.4)
            {
                return TryGiveJobInt(pawn, null);
            }
            return null;
        }
 


    private static List<CompGatherSpot> workingSpots = new List<CompGatherSpot>();



    private static readonly int NumRadiusCells = GenRadial.NumCellsInRadius(3.9f);

    private static readonly List<IntVec3> RadialPatternMiddleOutward = (from c in GenRadial.RadialPattern.Take(NumRadiusCells)
                                                                        orderby Mathf.Abs((c - IntVec3.Zero).LengthHorizontal - 1.95f)
                                                                        select c).ToList();


    public Job TryGiveJobInGatheringAreatwo(Pawn pawn, IntVec3 gatheringSpot, float maxRadius = -1f)
    {
        return TryGiveJobInt(pawn, (CompGatherSpot x) => GatheringsUtility.InGatheringArea(x.parent.Position, gatheringSpot, pawn.Map) && (maxRadius < 0f || x.parent.Position.InHorDistOf(gatheringSpot, maxRadius)));
    }

    private Job TryGiveJobInt(Pawn pawn, Predicate<CompGatherSpot> gatherSpotValidator)
    {
        if (pawn.Map.gatherSpotLister.activeSpots.Count == 0)
        {
            return null;
        }
        workingSpots.Clear();
        for (int i = 0; i < pawn.Map.gatherSpotLister.activeSpots.Count; i++)
        {
            workingSpots.Add(pawn.Map.gatherSpotLister.activeSpots[i]);
        }
        CompGatherSpot result;
        do
        {
            if (!workingSpots.TryRandomElement(out result))
            {
                return null;
            }
            workingSpots.Remove(result);
        }
        while (result.parent.IsForbidden(pawn) || !pawn.CanReach(result.parent, PathEndMode.Touch, Danger.None) || !result.parent.IsSociallyProper(pawn) || !result.parent.IsPoliticallyProper(pawn) || (gatherSpotValidator != null && !gatherSpotValidator(result)));
        Job job;
        Thing chair2;
        if (result.parent.def.surfaceType == SurfaceType.Eat)
        {
            if (!TryFindChairBesideTable(result.parent, pawn, out var chair))
            {
                return null;
            }
            //job = JobMaker.MakeJob(def.jobDef, result.parent, chair);
                job = JobMaker.MakeJob(FA_JobDefOf.FA_SocializeGather, result.parent, chair);
            }
        else if (TryFindChairNear(result.parent.Position, pawn, out chair2))
        {
            //job = JobMaker.MakeJob(def.jobDef, result.parent, chair2);
                job = JobMaker.MakeJob(FA_JobDefOf.FA_SocializeGather, result.parent, chair2);
            }
        else
        {
            if (!TryFindSitSpotOnGroundNear(result.parent.Position, pawn, out var result2))
            {
                return null;
            }
            //job = JobMaker.MakeJob(def.jobDef, result.parent, result2);
                job = JobMaker.MakeJob(FA_JobDefOf.FA_SocializeGather, result.parent, result2);
            }

        Pawn socialAblePawn;
        if (!TryFindSocializeablePawns(pawn, out socialAblePawn))
        {
            return null;
        }
        job.targetC = socialAblePawn;

        return job;
    }

    private static bool TryFindSocializeablePawns(Pawn sitter, out Pawn socialAblePawn)
    {

        List<Pawn> pawns;
        SocialNeed_Utility.GetListOfPawnsInDistance(sitter, 10.9f, out pawns);
        if (pawns != null)
        {
            socialAblePawn = pawns.FirstOrDefault();
            return true;
        }

        socialAblePawn = null;
        return false;



    }

    private static bool TryFindChairBesideTable(Thing table, Pawn sitter, out Thing chair)
    {
        for (int i = 0; i < 30; i++)
        {
            Building edifice = table.RandomAdjacentCellCardinal().GetEdifice(table.Map);
            CompAssignableToPawn_Throne compAssignableToPawn_Throne = edifice.TryGetComp<CompAssignableToPawn_Throne>();
            if (edifice != null && edifice.def.building.isSittable && !edifice.IsForbidden(sitter) && Toils_Ingest.TryFindFreeSittingSpotOnThing(edifice, sitter, out var _) && (compAssignableToPawn_Throne == null || compAssignableToPawn_Throne.AssignedPawns.Contains(sitter)))
            {
                chair = edifice;
                return true;
            }
        }
        chair = null;
        return false;
    }

    private static bool TryFindChairNear(IntVec3 center, Pawn sitter, out Thing chair)
    {
        for (int i = 0; i < RadialPatternMiddleOutward.Count; i++)
        {
            Building edifice = (center + RadialPatternMiddleOutward[i]).GetEdifice(sitter.Map);
            CompAssignableToPawn_Throne compAssignableToPawn_Throne = edifice.TryGetComp<CompAssignableToPawn_Throne>();
            if (edifice != null && edifice.def.building.isSittable && !edifice.IsForbidden(sitter) && Toils_Ingest.TryFindFreeSittingSpotOnThing(edifice, sitter, out var _) && GenSight.LineOfSight(center, edifice.Position, sitter.Map, skipFirstCell: true) && (compAssignableToPawn_Throne == null || compAssignableToPawn_Throne.AssignedPawns.Contains(sitter)))
            {
                chair = edifice;
                return true;
            }
        }
        chair = null;
        return false;
    }

    private static bool TryFindSitSpotOnGroundNear(IntVec3 center, Pawn sitter, out IntVec3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            IntVec3 intVec = center + GenRadial.RadialPattern[Rand.Range(1, NumRadiusCells)];
            if (sitter.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.None) && intVec.GetEdifice(sitter.Map) == null && GenSight.LineOfSight(center, intVec, sitter.Map, skipFirstCell: true))
            {
                result = intVec;
                return true;
            }
        }
        result = IntVec3.Invalid;
        return false;
    }

}
}
