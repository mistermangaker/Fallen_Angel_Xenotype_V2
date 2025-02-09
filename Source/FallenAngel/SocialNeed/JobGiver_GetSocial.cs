using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.AI;
using Verse;

namespace FallenAngel
{

    public class JobGiver_GetSocial : ThinkNode_JobGiver
    {
        [Unsaved(false)]
        private DefMap<SocialGiverDef, float> joyGiverChances;

        private const float JoyBuffer = 0.99f;

        protected virtual bool CanDoDuringMedicalRest => false;

        protected virtual bool JoyGiverAllowed(SocialGiverDef def)
        {
            return true;
        }

        protected virtual Job TryGiveJobFromJoyGiverDefDirect(SocialGiverDef def, Pawn pawn)
        {
            return def.Worker.TryGiveJob(pawn);
        }

        public override void ResolveReferences()
        {
            joyGiverChances = new DefMap<SocialGiverDef, float>();
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (!CanDoDuringMedicalRest && pawn.InBed() && HealthAIUtility.ShouldSeekMedicalRest(pawn))
            {
                return null;
            }
            if (pawn.needs.joy.CurLevel >= 0.99f)
            {
                return null;
            }
            List<SocialGiverDef> allDefsListForReading = DefDatabase<SocialGiverDef>.AllDefsListForReading;
            
            for (int i = 0; i < allDefsListForReading.Count; i++)
            {
                SocialGiverDef joyGiverDef = allDefsListForReading[i];
                joyGiverChances[joyGiverDef] = 0f;
                if (!JoyGiverAllowed(joyGiverDef) || !joyGiverDef.Worker.CanBeGivenTo(pawn))
                {
                    continue;
                }
                if (joyGiverDef.pctPawnsEverDo < 1f)
                {
                    Rand.PushState(pawn.thingIDNumber ^ 0x3C49C49);
                    if (Rand.Value >= joyGiverDef.pctPawnsEverDo)
                    {
                        Rand.PopState();
                        continue;
                    }
                    Rand.PopState();
                }
                
                joyGiverChances[joyGiverDef] = joyGiverDef.Worker.GetChance(pawn);
            }
            for (int j = 0; j < joyGiverChances.Count; j++)
            {
                if (!allDefsListForReading.TryRandomElementByWeight((SocialGiverDef d) => joyGiverChances[d], out var result))
                {
                    break;
                }
                Job job = TryGiveJobFromJoyGiverDefDirect(result, pawn);
                if (job != null)
                {
                    return job;
                }
                joyGiverChances[result] = 0f;
            }
            return null;
        }
    }



}
