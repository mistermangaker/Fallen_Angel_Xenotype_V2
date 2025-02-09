using RimWorld;
using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using FallenAngel;

namespace FallenAngel
{
    public enum SocialCategory : byte
    {
        Empty,
        VeryLow,
        Low,
        Satisfied,
        High,
        Extreme
    }
    public class Need_Social : Need
    {
     
        public SocialCategory CurCategory
        {
            get
            {
                if (CurLevel < 0.01f)
                {
                    return SocialCategory.Empty;
                }
                if (CurLevel < 0.15f)
                {
                    return SocialCategory.VeryLow;
                }
                if (CurLevel < 0.3f)
                {
                    return SocialCategory.Low;
                }
                if (CurLevel < 0.7f)
                {
                    return SocialCategory.Satisfied;
                }
                if (CurLevel < 0.85f)
                {
                    return SocialCategory.High;
                }
                return SocialCategory.Extreme;
            }
        }




        public override bool ShowOnNeedList
        {
            get
            {
                if ((float)pawn.ageTracker.AgeBiologicalYears < 13f)
                {
                    return false;
                }

                return true;
            }
        }

        protected override bool IsFrozen
        {
            get
            {
                if ((float)pawn.ageTracker.AgeBiologicalYears < 13f)
                {
                    return true;
                }
                return base.IsFrozen;
            }
        }
        private float FallPerDay()
        {
            if (!IsFrozen)
            {
                return 0.001f * pawn.GetStatValue(FA_StatDefOf.FA_SocialNeedFallRateFActor);

            }
            return 0f;
        }

        public override void NeedInterval()
        {
            
             CurLevel -= FallPerDay();

            
        }

        public override void SetInitialLevel()
        {
            base.CurLevelPercentage = 1f;
        }

        public Need_Social(Pawn pawn)
            : base(pawn)
        {
            threshPercents = new List<float> { 0.15f, 0.3f, 0.7f, 0.85f };
        }

    }
  

       


       
    }





