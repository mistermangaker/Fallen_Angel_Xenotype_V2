using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{



    [Obsolete]
    public class InteractionWorker_AffectionChitChat : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (initiator.Inhumanized())
            {
                return 0f;
            }
            if (!(recipient.genes.HasActiveGene(FA_GeneDefOf.FA_Gene_Affection)|| initiator.genes.HasActiveGene(FA_GeneDefOf.FA_Gene_Affection)))
            {
                return 0f;
            }
            FallenAngel_Utility.OffsetAffectionSocial(initiator, 0.01f);
            FallenAngel_Utility.OffsetAffectionSocial(recipient, 0.01f);
            return 1f;
        }

        
    }

}
