using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    public class CompProperties_OffsetSocialNeed : CompProperties_AbilityEffect
    {
       
        public float offsetAmountForInitator;
        public float offsetAmountForReciever;
        public ThoughtDef thoughtdefForInitator;
        public ThoughtDef thoughtdefForReciever;
        public CompProperties_OffsetSocialNeed()
        {
            compClass = typeof(CompAbility_OffsetSocialNeed);
        }
    }

   

   
    
}
