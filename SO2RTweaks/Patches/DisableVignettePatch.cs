using HarmonyLib;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SO2RTweaks.Patches
{
    internal class DisableVignettePatch
    {
        [HarmonyPatch(typeof(Volume), nameof(Volume.OnEnable))]
        [HarmonyPostfix]
        public static void OnEnable(Volume __instance)
        {
            __instance.profile.TryGet(out Vignette vignette);

            if (vignette)
            {
                vignette.active = false;

                Plugin.Log.LogDebug($"Disbabled vignette on game object '{__instance.gameObject.name}'.");
            }
        }
    }
}
