using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static HarmonyLib.Code;

namespace FallenAngel
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            
            new Harmony("FallenAngelPatch").PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    

    [HarmonyPatch(typeof(JobDriver_Lovin), "GenerateRandomMinTicksToNextLovin")]
    public static class GenerateRandomMinTicksToNextLovin_Patch
    {
        [HarmonyPostfix]
        private static void Postfix(Pawn pawn)
        {
            SocialNeed_Utility.OffsetSocialNeed(pawn, 1f);
            Gene gene = SocialNeed_Utility.GetSocializerGene(pawn);
            if (gene != null)
            {
                SocialNeed_Utility.GivePychicEffectInRange(pawn, 10.9f, FA_HediffDefOf.FA_FeelingWoozy,0.1f, FA_ThoughtDefOf.FA_PychicBlast);
                if (!FallenAngel_ModSettings.disableAlertForLovingPsychicBlast)
                {
                    Messages.Message("FeltPsychicBlast".Translate(pawn.Named("PAWN")), MessageTypeDefOf.NegativeEvent, historical: false);
                }
               
            }
            
        }
    }




    


}


