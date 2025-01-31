using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    
    public class Hediff_ObsessiveBond_Base : HediffWithTarget
    {
        public override string LabelBase => base.LabelBase + " (" + target?.LabelShortCap + ")";

        public override bool ShouldRemove
        {
            get
            {
                if (!base.ShouldRemove)
                {
                    return pawn.Dead;
                }
                return true;
            }
        }
       
        public override void PostRemoved()
        {
            
            Gene_ObsessiveBonding gene_PsychicBonding = base.pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>();
            if (gene_PsychicBonding != null)
            {
                
                gene_PsychicBonding.RemoveBond();
            }
            else if (target != null && target is Pawn pawn)
            {
                
                pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>()?.RemoveBond();
            }
        }

    }
}
