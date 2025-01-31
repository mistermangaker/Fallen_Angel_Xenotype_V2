
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using static RimWorld.Building_HoldingPlatform;

namespace FallenAngel
{

    public class Gene_ObsessiveBonding : Gene, IGeneResourceDrain
    {
        // genes that is based off of rimworlds pyschic bonding gene but for fallen angels
        // should go as follows
        // bonding to a new pawn should be caused by a mental break having both low mood and low affection
        // the obssessive bond should cause a hediff and though worker for the pawn that gives the giver a possive mood buff and passive affection buff 
        // the bonded pawn should be someone the giver is attracted to
        [Unsaved(false)]
        private Gene_Affection cachedGene_Affection;


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
        private const float MinAgeForDrain = 3f;
        public Gene_Resource Resource
        {
            get
            {
                if (cachedGene_Affection == null || !cachedGene_Affection.Active)
                {
                    cachedGene_Affection = pawn.genes.GetFirstGeneOfType<Gene_Affection>();
                }
                return cachedGene_Affection;
            }
        }
        public float ResourceLossPerDay
        {
            get
            {
                
                if (bondedPawn != null)
                {
                    //if bonded to another pawn this flips the reaourse lossrate from -2 ber day to plus 4
                    return def.resourceLossPerDay * -2;
                }
                return def.resourceLossPerDay;
                    
            }
        }


        public Pawn Pawn => pawn;
        public string DisplayLabel => Label + " (" + "Gene".Translate() + ")";
        public override void Tick()
        {
            base.Tick();

            FallenAngel_Utility.TickResourceDrain(this);
            //Gene_Affection.AffectionTick(this, ResourceLossPerDay);
            
            
            
        }
        



        public Pawn bondedPawn;

        public bool CanBondToNewPawn
        {
            get
            {
                if (bondedPawn != null)
                {
                    return false;
                }
                
                return true;
            }
        }

        public bool ReciverCanBeBondedTo(Pawn recieverPawn)
        {
            Gene_ObsessiveBonding gene_PsychicBonding = recieverPawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>();
            if (gene_PsychicBonding != null)
            {
                // cant bond to other obessive pawns would cause issues probably
                // i dont want to bother dealing with them
                return false;
            }
            Hediff firstHediffOfDef = recieverPawn.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_ObssesiveBondReceiver);
            if (firstHediffOfDef != null)
            {
                //cant bond if they have someone else bonded to them
                // again dont want to deal with any issues that would arise
                return false;
            }
            return true;
        }


        public override void PostAdd()
        {
            base.PostAdd();
            if (pawn.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_ObssesiveBondGiver) is Hediff_ObsessiveBond_Giver hediff_PsychicBond)
            {
                bondedPawn = (Pawn)hediff_PsychicBond.target;
            }

        }

        public override void PostRemove()
        {
            base.PostRemove();
            Notify_MyOrPartnersGeneRemoved();
        }


       public void TryBondTo ( Pawn newBond )
        {
            if (newBond == null)
            {
                Log.Message("no pawn to bond to");
                return;
            }
            if (bondedPawn != null)
            {
                Log.Error("Tried to bond to more than one pawn.");
                return;
            }
            if (!CanBondToNewPawn)
            {
                return;
            }
            if(!ReciverCanBeBondedTo(newBond))
            {
                return;
            }
            bondedPawn = newBond;

            // getting pawn the hediff obssessivebondgiver
            Hediff GiverHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_ObssesiveBondGiver);
            if (GiverHediffOfDef != null)
            {
                pawn.health.RemoveHediff(GiverHediffOfDef);
            }
            // setting the target for the hediff of obsessive bond giver
            Hediff_ObsessiveBond_Giver hediff_PsychicBond = (Hediff_ObsessiveBond_Giver)HediffMaker.MakeHediff(FA_HediffDefOf.FA_ObssesiveBondGiver, pawn);
            hediff_PsychicBond.target = newBond;
            // adding the hediff to this pawn
            pawn.health.AddHediff(hediff_PsychicBond);

            // getting the heddiff of obessivebondreciever to the new bond
            Hediff_ObsessiveBond_Receiver hediff_PsychicBond2 = (Hediff_ObsessiveBond_Receiver)HediffMaker.MakeHediff(FA_HediffDefOf.FA_ObssesiveBondReceiver, bondedPawn);
            if (hediff_PsychicBond2 != null)
            {
                bondedPawn.health.RemoveHediff(hediff_PsychicBond2);
            }
            // setting the target
            hediff_PsychicBond2.target = pawn;
            //adding the hediff to the new bond
            bondedPawn.health.AddHediff(hediff_PsychicBond2);
        }


        public void RemoveBond()
        {
            if (bondedPawn == null)
            {
                return;
            }

            Pawn pawn = bondedPawn;
            bondedPawn = null;
            if (base.pawn.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_ObssesiveBondGiver) is Hediff_ObsessiveBond_Giver hediff_PsychicBond && hediff_PsychicBond.target == pawn)
            {
                base.pawn.health.RemoveHediff(hediff_PsychicBond);
            }
            if (pawn.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_ObssesiveBondReceiver) is Hediff_ObsessiveBond_Receiver hediff)
            {
                pawn.health.RemoveHediff(hediff);
                pawn.health.AddHediff(FA_HediffDefOf.FA_ObssesiveBondGiver_BondBroken);
            }
            pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>()?.RemoveBond();
            if (!base.pawn.Dead)
            {
                
                if (DefDatabase<MentalBreakDef>.AllDefsListForReading.Where((MentalBreakDef d) => d.intensity == MentalBreakIntensity.Extreme && d.Worker.BreakCanOccur(base.pawn)).TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(base.pawn, moodCaused: true), out var result))
                {
                    result.Worker.TryStart(base.pawn, "MentalStateReason_BondedHumanDeath".Translate(pawn), causedByMood: false);
                }
            }
        }


        public void Notify_MyOrPartnersGeneRemoved()
        {
            if (bondedPawn != null && bondedPawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>() == null)
            {
                Pawn pawn = bondedPawn;
                bondedPawn = null;
                if (base.pawn.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_ObssesiveBondGiver) is Hediff_ObsessiveBond_Giver hediff_PsychicBond && hediff_PsychicBond.target == pawn)
                {
                    base.pawn.health.RemoveHediff(hediff_PsychicBond);
                }
                if (pawn.health.hediffSet.GetFirstHediffOfDef(FA_HediffDefOf.FA_ObssesiveBondReceiver) is Hediff_ObsessiveBond_Receiver hediff)
                {
                    pawn.health.RemoveHediff(hediff);
                    
                }
                pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>()?.Notify_MyOrPartnersGeneRemoved();
            }
        }
        public override IEnumerable<Gizmo> GetGizmos()
        {
            
            if (!DebugSettings.ShowDevGizmos  || !pawn.Spawned)
            {
                yield break;
            }
            if (CanBondToNewPawn)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Bond to random pawn",
                    action = delegate
                    {
                        Pawn result = ObsessiveBondingUtility.FindPawnToObsesseOver(pawn);
                        if (result != null)
                        {
                            TryBondTo(result);
                        }
                        else if ((from x in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
                                  where x.RaceProps.Humanlike && x != pawn
                                  select x).TryRandomElement(out var resulttwo))

                        {
                            TryBondTo(resulttwo);
                        }


                    }

                };
            }
            if (bondedPawn != null)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "DEV: break bond";
                command_Action.action = delegate
                {
                    base.pawn.genes?.GetFirstGeneOfType<Gene_ObsessiveBonding>()?.RemoveBond();
                };
                yield return command_Action;
                Command_Action command_Action2 = new Command_Action();
                command_Action2.defaultLabel = "DEV: make spouse";
                command_Action2.action = delegate
                {

                    pawn.relations.AddDirectRelation(PawnRelationDefOf.Spouse, bondedPawn);
                    bondedPawn.relations.AddDirectRelation(PawnRelationDefOf.Spouse, pawn);
                };
                yield return command_Action2;

                
            }
            
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref bondedPawn, "bondedPawn");
            
        }

    }
}
