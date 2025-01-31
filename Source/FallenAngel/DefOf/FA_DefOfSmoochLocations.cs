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
    public static class FA_DefOfSmoochLocations
    {
        static FA_DefOfSmoochLocations()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FA_DefOfSmoochLocations));
        }

        public static BodyPartDef Neck;
        public static BodyPartDef Head;
        public static BodyPartDef Ear;
        public static BodyPartDef Nose;
        public static BodyPartDef Jaw;
        public static BodyPartDef Tongue;
        public static BodyPartDef Hand;
        public static BodyPartDef Clavicle;
        public static BodyPartDef Waist;



    }
}
