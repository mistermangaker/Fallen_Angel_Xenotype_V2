using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.Noise;
using Verse;

namespace FallenAngel
{
    public class Gene_Affection : Gene_Resource, IGeneResourceDrain
    {
        
        public int tickssincelastjob = -9999;

        public void DoFeeding()
        {
            tickssincelastjob = Find.TickManager.TicksGame;
        }
        public bool feedtoday()
        {
            if ((Find.TickManager.TicksGame - tickssincelastjob) >= 6000)
            {
                tickssincelastjob = 0;
                return false;
            }

            return true;
        }

        public Gene_Resource Resource => this;

        public bool CanGetAffectionFromColonists = true;

        public bool CanGetAffectionFromPrisoners = true;

        public bool CanGetAffectionFromSlaves = true;

        public Pawn Pawn => pawn;

        public bool CanOffset
        {
            get
            {
                if (Active)
                {
                    return true;
                }
                return false;
            }
        }

        public string DisplayLabel => Label + " (" + "Gene".Translate() + ")";

        public float ResourceLossPerDay => def.resourceLossPerDay;

        public override float InitialResourceMax => 1f;

        public override float MinLevelForAlert => 0.15f;

        public override float MaxLevelOffset => 0.1f;

        protected override Color BarColor => new ColorInt(138, 3, 3).ToColor;

        protected override Color BarHighlightColor => new ColorInt(145, 42, 42).ToColor;

        public override void PostAdd()
        {

            base.PostAdd();
            Reset();

        }

        


        public override void Tick()
        {
            base.Tick();
            FallenAngel_Utility.TickResourceDrain(this);
            
        }

       


        public bool ShouldSeekAffectionNow()
        {
            return Value < targetValue;
        }

        public override void SetTargetValuePct(float val)
        {
            targetValue = Mathf.Clamp(val * Max, 0f, Max - MaxLevelOffset);
        }



        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (!Active)
            {
                yield break;
            }
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            foreach (Gizmo resourceDrainGizmo in GeneResourceDrainUtility.GetResourceDrainGizmos(this))
            {
                yield return resourceDrainGizmo;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref CanGetAffectionFromColonists, "Can Get Affection From Colonists", defaultValue: true);
            Scribe_Values.Look(ref CanGetAffectionFromColonists, "Can Get Affection From Prisoners", defaultValue: true);
            Scribe_Values.Look(ref CanGetAffectionFromColonists, "Can Get Affection From Slaves", defaultValue: true);


        }




    }


}
