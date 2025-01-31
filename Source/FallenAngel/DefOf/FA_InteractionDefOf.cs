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
    public static class FA_InteractionDefOf
    {
        static FA_InteractionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FA_InteractionDefOf));
        }

        
        public static InteractionDef FA_TalkedAt;


    }
}
