using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    public class CompGeneUsable : CompUsable
    {
        public CompProperties_GeneUsable GeneProps => (CompProperties_GeneUsable)props;
        public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
        {
            if (GeneProps.requiredGene != null && !p.genes.HasActiveGene(GeneProps.requiredGene))
            {
                return "MustHaveHediff".Translate(GeneProps.requiredGene);
            }
            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }

    public class CompProperties_GeneUsable : CompProperties_Usable
    {
        public GeneDef requiredGene;
        public CompProperties_GeneUsable()
        {
            compClass = typeof(CompGeneUsable);
        }
    }

    public class CompUsableImplant : CompGeneUsable
    {

    }

}
