using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    

    public class HediffComp_SeverityFromAffection : HediffComp
    {
        private Gene_Affection cachedHemogenGene;

        public HediffCompProperties_SeverityFromAffection Props => (HediffCompProperties_SeverityFromAffection)props;

        public override bool CompShouldRemove => base.Pawn.genes?.GetFirstGeneOfType<Gene_Affection>() == null;

        private Gene_Affection Hemogen
        {
            get
            {
                if (cachedHemogenGene == null)
                {
                    cachedHemogenGene = base.Pawn.genes.GetFirstGeneOfType<Gene_Affection>();
                }
                return cachedHemogenGene;
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (Hemogen != null)
            {
                severityAdjustment += ((Hemogen.Value > 0f) ? Props.severityPerHourAffection : Props.severityPerHourEmpty) / 2500f;
            }
        }
    }

}
