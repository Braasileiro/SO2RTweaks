using HarmonyLib;
using UnityEngine.Rendering.Universal;
using static Settings;

namespace SO2RTweaks.Patches
{
    internal class PostProcessAAPatch
    {
        [HarmonyPatch(typeof(UniversalAdditionalCameraData), nameof(UniversalAdditionalCameraData.OnAfterDeserialize))]
        [HarmonyPostfix]
        public static void OnAfterDeserialize(UniversalAdditionalCameraData __instance)
        {
            if (__instance.gameObject.name == "MainCamera")
            {
                __instance.antialiasing = (AntialiasingMode)iPostProcessAA.Value;
                __instance.antialiasingQuality = AntialiasingQuality.High;

                Plugin.Log.LogInfo($"Enabled {iPostProcessAA.Value} ({__instance.antialiasingQuality}) on MainCamera.");
            }
        }
    }
}
