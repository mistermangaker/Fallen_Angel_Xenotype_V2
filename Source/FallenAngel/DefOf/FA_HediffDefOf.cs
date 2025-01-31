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
    public static class FA_HediffDefOf
    {
        static FA_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FA_HediffDefOf));
        }

        public static HediffDef FA_AttentionCraving;
        public static HediffDef FA_FullBodyHug;
        public static HediffDef FA_FeelingWoozy;
        public static HediffDef FA_SmoochMarks;
        public static HediffDef FA_ObssesiveBondReceiver;
        
        public static HediffDef FA_ObssesiveBondGiver;
        public static HediffDef FA_ObssesiveBondGiver_BondBroken;
            


    }
}
