using Game;
using Common;
using HarmonyLib;
using static Settings;

namespace SO2RTweaks.Patches
{
    internal class SkipIntroPatch
    {
        private static bool bSkippedIntroLogos = false;
        private static bool bSkippedIntroOpeningMovie = false;
        private static bool bSkippedTitlePresenter = false;

        [HarmonyPatch(typeof(GameSceneManager), nameof(GameSceneManager.CreateNextScene))]
        [HarmonyPrefix]
        public static void CreateNextScene(ref SceneType sceneType)
        {
            if (sceneType == SceneType.Logo)
            {
                if (bSkipLogos.Value && !bSkippedIntroLogos)
                {
                    if (bSkipOpeningMovie.Value)
                    {
                        // Logos + Opening Movie
                        sceneType = SceneType.Title;

                        bSkippedIntroOpeningMovie = true;
                    }
                    else
                    {
                        // Only Logos
                        sceneType = SceneType.OpeningMovie;
                    }

                    bSkippedIntroLogos = true;
                }
            }
            else if (sceneType == SceneType.OpeningMovie)
            {
                if (bSkipOpeningMovie.Value && !bSkippedIntroOpeningMovie)
                {
                    // Only Opening Movie
                    sceneType = SceneType.Title;

                    bSkippedIntroOpeningMovie = true;
                }
            }
        }

        [HarmonyPatch(typeof(UITitlePressAnyButtonSelector), nameof(UITitlePressAnyButtonSelector.Show))]
        [HarmonyPostfix]
        public static void PressAnyButtonSelectorShow(ref UITitlePressAnyButtonSelector __instance)
        {
            if (bSkipOpeningMovie.Value && !bSkippedTitlePresenter)
            {
                // Skip the short title animation on press any button screen
                __instance.titlePresenter.Skip();

                bSkippedTitlePresenter = true;
            }
        }
    }
}
