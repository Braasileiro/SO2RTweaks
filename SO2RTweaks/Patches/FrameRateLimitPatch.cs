using Game;
using HarmonyLib;
using UnityEngine;
using System.Threading.Tasks;
using static Settings;

namespace SO2RTweaks.Patches
{
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
