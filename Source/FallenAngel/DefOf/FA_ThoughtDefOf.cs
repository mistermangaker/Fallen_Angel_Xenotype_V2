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
    public static class FA_ThoughtDefOf
    {
        static FA_ThoughtDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FA_ThoughtDefOf));
        }

        public static ThoughtDef FA_GainedAffectionFrom;
        public static ThoughtDef FA_GainedAffectionFrom_Social;
        public static ThoughtDef FA_FedOnBy;
        public static ThoughtDef FA_FedOnBy_Social;
        public static ThoughtDef FA_ObsessiveBondReceiver;
        public static ThoughtDef FA_ObsessiveBondGiver;


    }
}
