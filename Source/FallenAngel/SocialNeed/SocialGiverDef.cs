using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    public class SocialGiverDef : Def
    {
        public Type giverClass;

        public float baseChance;

        public JobDef jobDef;

        public List<ThingDef> thingDefs;

        public float pctPawnsEverDo = 1f;

        public bool canDoWhileInBed;

        public SocialKindDef socialKind;

        public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();


        private SocialGiver workerInt;

        public SocialGiver Worker
        {
            get
            {
                if (workerInt == null)
                {
                    workerInt = (SocialGiver)Activator.CreateInstance(giverClass);
                    workerInt.def = this;
                }
                return workerInt;
            }
        }


    }
}
