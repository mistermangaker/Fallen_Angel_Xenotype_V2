using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{

    public class HediffCompProperties_SeverityFromAffection : HediffCompProperties
    {
        public float severityPerHourEmpty;

        public float severityPerHourAffection;

        public HediffCompProperties_SeverityFromAffection()
        {
            compClass = typeof(HediffComp_SeverityFromAffection);
        }
    }

}
