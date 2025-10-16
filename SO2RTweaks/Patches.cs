using Game;
using Common;
using HarmonyLib;
using UnityEngine;
using System.Threading.Tasks;
using static Settings;
using static Common.InputManager;

namespace SO2RTweaks
{
    internal class SkipIntroPatch
    {
        private static bool bSkippedIntroLogos = false;
        private static bool bSkippedIntroOpeningMovie = false;

        [HarmonyPatch(typeof(GameSceneManager), nameof(GameSceneManager.CreateNextScene))]
        [HarmonyPrefix]
        public static void CreateNextScene(ref SceneType nextSceneType)
        {
            if (nextSceneType == SceneType.Logo)
            {
                if (bSkipLogos.Value && !bSkippedIntroLogos)
                {
                    if (bSkipOpeningMovie.Value)
                    {
                        // Logos + Opening Movie
                        nextSceneType = SceneType.Title;

                        bSkippedIntroOpeningMovie = true;
                    }
                    else
                    {
                        // Only Logos
                        nextSceneType = SceneType.OpeningMovie;
                    }

                    bSkippedIntroLogos = true;
                }
            }
            else if (nextSceneType == SceneType.OpeningMovie)
            {
                if (bSkipOpeningMovie.Value && !bSkippedIntroOpeningMovie)
                {
                    // Only Opening Movie
                    nextSceneType = SceneType.Title;

                    bSkippedIntroOpeningMovie = true;
                }
            }
        }
    }

    internal class ButtonPromptsPatch
    {
        [HarmonyPatch(typeof(InputManager), nameof(InputManager.GetGamepadType))]
        [HarmonyPostfix]
        public static void GetGamepadType(ref GamepadType __result)
        {
            __result = (GamepadType)iButtonPrompts.Value;
        }
    }

    internal class FrameRateLimitPatch
    {
        private static volatile bool bGetFrameRateInitialized = false;

        [HarmonyPatch(typeof(SystemConfigParameter), nameof(SystemConfigParameter.GetFrameRate))]
        [HarmonyPostfix]
        public static void GetFrameRate()
        {
            if (!bGetFrameRateInitialized)
            {
                bGetFrameRateInitialized = true;
            }
            else
            {
                Application.targetFrameRate = iFrameRateLimit.Value;
            }
        }

        [HarmonyPatch(typeof(SystemConfigParameter), nameof(SystemConfigParameter.SetFrameRate))]
        [HarmonyPostfix]
        public static void SetFrameRate()
        {
            Application.targetFrameRate = iFrameRateLimit.Value;
        }

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.ChangeFrameRate))]
        [HarmonyPostfix]
        public static void ChangeFrameRate()
        {
            Application.targetFrameRate = iFrameRateLimit.Value;
        }

        public static async Task SetFrameRateLimitAsync()
        {
            while (!bGetFrameRateInitialized)
            {
                await Task.Delay(100);
            }

            Application.targetFrameRate = iFrameRateLimit.Value;

            Plugin.Log.LogInfo($"Application target framerate set to {Application.targetFrameRate}.");
        }
    }
}
