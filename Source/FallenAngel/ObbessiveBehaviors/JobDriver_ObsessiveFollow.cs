using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Sprites;
using Verse;
using Verse.AI;

namespace FallenAngel
{
    public class JobDriver_ObsessiveFollow : JobDriver
    {
        private const TargetIndex FolloweeInd = TargetIndex.A;
        private const int CheckPathIntervalTicks = 30;

        private Pawn Actor => GetActor();

        private Pawn TargetPawn => base.TargetThingA as Pawn;


       
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        public override RandomSocialMode DesiredSocialMode()
        {
            return RandomSocialMode.SuperActive;
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
            

            Toil followTarget = ToilMaker.MakeToil("DateFollowPartner");
            followTarget.defaultCompleteMode = ToilCompleteMode.Delay;
            followTarget.initAction = delegate
            {
                ticksLeftThisToil = 200;
                Actor.pather.StartPath(TargetPawn, PathEndMode.Touch);
            };
            followTarget.tickAction = delegate
                {
                    if (Actor.needs.joy == null || !JoyUtility.JoyTickCheckEnd(Actor, JoyTickFullJoyAction.None))
                    {
                        Need_Joy joy = Actor.needs.joy;
                        if (joy != null && joy.CurLevelPercentage > 0.9999f)
                        {
                           
                            EndJobWith(JobCondition.Succeeded);
                            
                        }
                    }
                    
                };
            
            for (int i = 0; i < 100; i++)
            {
                yield return followTarget;
            }

        }
    }
}
