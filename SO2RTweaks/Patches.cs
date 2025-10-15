using Game;
using Common;
using HarmonyLib;
using UnityEngine;
using System.Threading.Tasks;
using static Common.InputManager;
using static Settings;

namespace SO2RTweaks
{
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
        private static volatile bool _isInitialized = false;

        [HarmonyPatch(typeof(SystemConfigParameter), nameof(SystemConfigParameter.GetFrameRate))]
        [HarmonyPostfix]
        public static void GetFrameRate()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
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
            while (!_isInitialized)
            {
                await Task.Delay(100);
            }

            Application.targetFrameRate = iFrameRateLimit.Value;

            Plugin.Log.LogInfo($"Application target framerate set to {Application.targetFrameRate}.");
        }
    }
}
