using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    [DefOf]
    public static class FA_JobDefOf
    {
        static FA_JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FA_JobDefOf));
        }

        public static JobDef FA_TalkAtTarget;
        public static JobDef FA_GettingAffectionFrom;
        public static JobDef FA_ObsessiveFollow;

        public static JobDef FA_ObsessiveWatchInBed;
        public static JobDef FA_ObsessiveSleepInOthersBed;
        

    }
}
