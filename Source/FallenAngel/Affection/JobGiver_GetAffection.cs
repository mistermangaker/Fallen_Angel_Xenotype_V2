using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.AI;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace FallenAngel
{
    public class JobGiver_GetAffection : ThinkNode_JobGiver
    {

    

    

        public override float GetPriority(Pawn pawn)
        {
            if (!ModsConfig.BiotechActive)
            {
                return 0f;
            }
            if (pawn.genes?.GetFirstGeneOfType<Gene_Affection>() == null)
            {
                return 0f;
            }
            return 9.1f;
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            
            
            if (!ModsConfig.BiotechActive)
            {
                return null;
            }
            Gene_Affection gene_Hemogen = pawn.genes?.GetFirstGeneOfType<Gene_Affection>();
            
            if (gene_Hemogen == null)
            {
                return null;
            }
            if (!gene_Hemogen.ShouldSeekAffectionNow())
            {
                
                return null;
            }
            if (gene_Hemogen.feedtoday())
            {
                
                return null;
            }
            Pawn prisoner = FallenAngel_Utility.ReturnFeedingListForInitator(pawn, true).RandomElement();
            if (prisoner != null)
            {

                return JobMaker.MakeJob(FA_JobDefOf.FA_GettingAffectionFrom, prisoner);
            }


            return null;
        }


       
        
    }

}
