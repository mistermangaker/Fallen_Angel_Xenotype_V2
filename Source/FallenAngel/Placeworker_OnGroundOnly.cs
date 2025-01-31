using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    
    public class Placeworker_OnGroundOnly : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            AcceptanceReport result = false;
            CellRect cellRect = GenAdj.OccupiedRect(loc, rot, checkingDef.Size);
            bool notblocked = true;
            bool outmap = false;
            foreach (IntVec3 item in cellRect)
            {
                IntVec3 c = item - rot.FacingCell;
                if (item.Impassable(map))
                {
                    result = "FA_Placeworker_OnGroundOnlyCantPlaceOnWall".Translate();
                    notblocked = false;
                }

                if ((item.InNoBuildEdgeArea(map) || c.InNoBuildEdgeArea(map)))
                {
                    result = "FA_Placeworker_OnGroundOnlyTooCloseToMapEdge".Translate();
                    outmap = true;
                }
            }
            if (notblocked && !outmap)
            {
                result = true;
            }
            return result;
        }
    }
}
