using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace FallenAngel
{
   
    public class Hediff_AffectionCraving : HediffWithComps
    {
        public override string SeverityLabel
        {
            get
            {
                if (Severity == 0f)
                {
                    return null;
                }
                return Severity.ToStringPercent();
            }
        }
    }

  
}
