using Game;
using HarmonyLib;
using static Settings;

namespace SO2RTweaks.Patches
{
    internal class ShadowDistancePatch
    {
        [HarmonyPatch(typeof(GameRenderManager), nameof(GameRenderManager.SetShadowDistance))]
        [HarmonyPrefix]
        public static void SetShadowDistance(ref float shadowDistance)
        {
            shadowDistance *= iShadowDistanceMultiplier.Value;
        }
    }
}
