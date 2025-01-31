using RimWorld.Planet;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FallenAngel
{
    
    public class Alert_LowAffection : Alert
    {
        private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

        private List<string> targetLabels = new List<string>();

        public Alert_LowAffection()
        {
            requireBiotech = true;
            defaultLabel = "AlertLowAffection".Translate();
        }

        public override string GetLabel()
        {
            string text = defaultLabel;
            if (targets.Count == 1)
            {
                text = text + ": " + targetLabels[0];
            }
            return text;
        }

        private void CalculateTargets()
        {
            targets.Clear();
            targetLabels.Clear();
            foreach (Pawn item in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
            {
                if (item.genes != null && item.RaceProps.Humanlike && item.Faction == Faction.OfPlayer)
                {
                    Gene_Affection firstGeneOfType = item.genes.GetFirstGeneOfType<Gene_Affection>();
                    if (firstGeneOfType != null && firstGeneOfType.Value < firstGeneOfType.MinLevelForAlert)
                    {
                        targets.Add(item);
                        targetLabels.Add(item.NameShortColored.Resolve());
                    }
                }
            }
        }

        public override TaggedString GetExplanation()
        {
            return "AlertLowAffectionDesc".Translate() + ":\n" + targetLabels.ToLineList("  - ");
        }

        public override AlertReport GetReport()
        {
            CalculateTargets();
            return AlertReport.CulpritsAre(targets);
        }
    }
}
