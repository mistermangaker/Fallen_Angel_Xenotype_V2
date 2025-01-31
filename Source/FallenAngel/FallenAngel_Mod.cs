using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace FallenAngel
{
    internal class FallenAngel_Mod : Mod
    {
        public static FallenAngel_ModSettings settings;
        public FallenAngel_Mod(ModContentPack content)
        : base(content)
        {
            settings = GetSettings<FallenAngel_ModSettings>();
        }

        


        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.Label("mod settings");
            listing_Standard.Label("modifiers");
            listing_Standard.Label("affection general gain modifier: " + FallenAngel_ModSettings.affectionGeneralGainMultiplyer);
            FallenAngel_ModSettings.affectionGeneralGainMultiplyer = (float)listing_Standard.Slider(FallenAngel_ModSettings.affectionGeneralGainMultiplyer, 0f, 5f);
            listing_Standard.Label("affection kiss gain modifier: " + FallenAngel_ModSettings.affectionKissGainMultiplyer);
            FallenAngel_ModSettings.affectionKissGainMultiplyer = (float)listing_Standard.Slider(FallenAngel_ModSettings.affectionKissGainMultiplyer, 0f, 5f);
            listing_Standard.Label("feeding cool down modifier: " + FallenAngel_ModSettings.feedAgainDuractionCoolDownMultiplyer);
            FallenAngel_ModSettings.feedAgainDuractionCoolDownMultiplyer = (float)listing_Standard.Slider(FallenAngel_ModSettings.feedAgainDuractionCoolDownMultiplyer, 0.1f, 5f);
            listing_Standard.Label("resource drain rate modifier: " + FallenAngel_ModSettings.resourceDrainRatePerdayMultiplier);
            FallenAngel_ModSettings.resourceDrainRatePerdayMultiplier = (float)listing_Standard.Slider(FallenAngel_ModSettings.resourceDrainRatePerdayMultiplier, 0f, 5f);
            listing_Standard.Label("modifiers");
            listing_Standard.Label("opinion threshold for comfortable kiss: " + FallenAngel_ModSettings.defaultOpinionForNegativeMoodFromKissing);
            FallenAngel_ModSettings.defaultOpinionForNegativeMoodFromKissing = (int)listing_Standard.Slider(FallenAngel_ModSettings.defaultOpinionForNegativeMoodFromKissing, -100, 100);
            listing_Standard.CheckboxLabeled("Disable Low opinion Warning pop up for kissing", ref FallenAngel_ModSettings.disableOpinionPopUpForKissing);
            listing_Standard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "FA_Title".Translate();
        }
    }

    public class FallenAngel_ModSettings : ModSettings
    {
        public static float affectionGeneralGainMultiplyer = 1f;
        public static float affectionKissGainMultiplyer = 1f;
        public static float feedAgainDuractionCoolDownMultiplyer = 1f;
        public static float resourceDrainRatePerdayMultiplier = 1f;
        public static int defaultOpinionForNegativeMoodFromKissing = 20;
        public static int defaultAffectionGainBuff = 0;
        public static bool disableOpinionPopUpForKissing = false;




        public override void ExposeData()
        {
            Scribe_Values.Look(ref affectionKissGainMultiplyer, "affectionKissGainMultiplyer", 1f);
            Scribe_Values.Look(ref affectionGeneralGainMultiplyer, "affectionGeneralGainMultiplyer", 1f);
            Scribe_Values.Look(ref feedAgainDuractionCoolDownMultiplyer, "feedAgainDuractionCoolDownMultiplyer", 1f);
            Scribe_Values.Look(ref resourceDrainRatePerdayMultiplier, "resourceDrainRatePerdayMultiplier", 1f);
            Scribe_Values.Look(ref defaultOpinionForNegativeMoodFromKissing, "defaultOpinionForNegativeMoodFromKissing", 20);
            Scribe_Values.Look(ref defaultAffectionGainBuff, "defaultAffectionGainBuff", 0);
            Scribe_Values.Look(ref disableOpinionPopUpForKissing, "disableOpinionPopUpForKissing", false);
            base.ExposeData();
        }
    }
}
