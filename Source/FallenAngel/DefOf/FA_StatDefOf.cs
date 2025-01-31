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
    public static class FA_StatDefOf
    {
        static FA_StatDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FA_StatDefOf));
        }

        public static StatDef FA_AffectionGainFactor;
   


    }
}
