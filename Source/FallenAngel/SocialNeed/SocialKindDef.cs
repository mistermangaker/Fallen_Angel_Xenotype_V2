using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    public class SocialKindDef : Def
    {

        public List<PawnRelationDef> requiredRelationshipDefs;
        public List<PawnRelationDef> BlackListedRelationshipDefs;

    }
}
