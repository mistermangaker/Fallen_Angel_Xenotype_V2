using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using UnityEngine;
using Verse;

namespace FallenAngel
{
   
    public class Hediff_ObsessiveBond_Receiver : Hediff_ObsessiveBond_Base
    {
        public override string LabelBase => ObsessiveBehaviors_Utility.IsLover((target as Pawn), pawn)? "beloved" + " (" + target?.LabelShortCap + ")" : base.LabelBase;
        public override void PostTick()
        {
            base.PostTick();
            {
                Severity = (ObsessorUnhappy() ? 0.1f : 1.0f);
            }
            
        }
        private bool ObsessorUnhappy()
        {
            if ((target as Pawn).needs.mood.CurLevelPercentage >= 0.5f)
            {
                return true;
            }
            return false;
        }
        public override void PostRemoved()
        {
            base.PostRemoved();
            
            
        }
    }
}
