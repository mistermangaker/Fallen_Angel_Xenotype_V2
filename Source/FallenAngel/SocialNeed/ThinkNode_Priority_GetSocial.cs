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
  
    public class ThinkNode_Priority_GetSocial : ThinkNode_Priority
    {
        private const int GameStartNoJoyTicks = 5000;

        public override float GetPriority(Pawn pawn)
        {
            if (pawn.needs.joy == null)
            {
                return 0f;
            }
            if (Find.TickManager.TicksGame < 5000)
            {
                return 0f;
            }
            if (JoyUtility.LordPreventsGettingJoy(pawn))
            {
                return 0f;
            }
            Need_Social need = pawn?.needs?.TryGetNeed<Need_Social>();
            float curLevel = need .CurLevel;
            TimeAssignmentDef timeAssignmentDef = ((pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment);
            if (!timeAssignmentDef.allowJoy)
            {
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
            {
                if (curLevel < 0.35f)
                {
                    return 6f;
                }
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
            {
                if (curLevel < 0.95f)
                {
                    return 7f;
                }
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
            {
                if (curLevel < 0.95f)
                {
                    return 2f;
                }
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
            {
                return 0f;
            }
            throw new NotImplementedException();
        }
    }

}
