using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    
    public class StatWorker_FallenAngel : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            if (!base.ShouldShowFor(req))
            {
                return false;
            }
            if (req.Thing != null && req.Thing is Pawn pawn)
            {
                return FallenAngel_Utility.HasAffectionGene(pawn);
                    
            }
            return false;
        }
    }
}
