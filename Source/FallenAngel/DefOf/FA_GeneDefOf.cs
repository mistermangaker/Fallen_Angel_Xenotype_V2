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
    public static class FA_GeneDefOf
    {
        static FA_GeneDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FA_GeneDefOf));
        }

        public static GeneDef FA_Gene_Affection;
        public static GeneDef Gene_ObsessiveBonding;
        public static GeneDef Gene_Loving;


    }
}
