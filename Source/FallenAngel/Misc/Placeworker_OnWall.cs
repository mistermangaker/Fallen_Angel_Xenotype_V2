using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    
    public class Placeworker_OnWall : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            AcceptanceReport result = false;
            if (loc.Impassable(map))
            {
                result = true;
            }
            if (!loc.Impassable(map))
            {
                return "FA_PlaceWorker_OnWall".Translate();
            }
            return result;
        }
    }
}
