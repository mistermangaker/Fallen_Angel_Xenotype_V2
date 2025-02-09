using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace FallenAngel
{
    public class NoColorApparel : Apparel
    {
        private Graphic newGraphic;

        public override Color DrawColor => Color.white;

        private Graphic NewGraphic
        {
            get
            {
                if (newGraphic == null)
                {
                    if (def.graphicData == null)
                    {
                        return BaseContent.BadGraphic;
                    }
                    newGraphic = def.graphicData.Graphic;
                }
                return newGraphic;
            }
        }

        public override Graphic Graphic => NewGraphic;
    }
}
