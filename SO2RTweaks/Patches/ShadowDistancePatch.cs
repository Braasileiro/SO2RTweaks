using Game;
using HarmonyLib;
using UnityEngine;
using static Settings;

namespace SO2RTweaks.Patches;

internal class ShadowDistancePatch
{
    // Calculated
    private static readonly float _cascadeBorder;
    private static readonly Vector3 _cascade4Split;
    private static readonly float _shadowNormalBias;
    private static readonly float _shadowDepthBias;

    static ShadowDistancePatch()
    {
        // Interpolation factor based on the user-selected multiplier (scales from 2x up to 4x)
        float factor = Mathf.InverseLerp(2, 4, iShadowDistanceMultiplier.Value);

        // The cascade border defines the blend zone between different shadow resolutions
        _cascadeBorder = Mathf.Lerp(0.3f, 0.45f, factor);

        /*
         * Shadow Cascade Distribution (4)
         * 1st split (30%): Total distance with maximum resolution
         * 2nd split (45%): Maintains a good mid-range resolution
         * 3rd split (65% to 80%): Scales the last cascade towards the horizon
         */
        _cascade4Split = new(0.3f, 0.45f, Mathf.Lerp(0.65f, 0.80f, factor));

        // Adjusts shadow bias to prevent artifacts and shadows disconnected from the object
        _shadowNormalBias = Mathf.Lerp(0.6f, 1.0f, factor);
        _shadowDepthBias = Mathf.Lerp(0.6f, 0.9f, factor);

        Plugin.Log.LogInfo("Calculated pipeline asset shadow settings.");
        Plugin.Log.LogInfo("------------------------");
        Plugin.Log.LogInfo($"CascadeBorder: {_cascadeBorder}");
        Plugin.Log.LogInfo($"Cascade4Split: {_cascade4Split}");
        Plugin.Log.LogInfo($"NormalBias: {_shadowNormalBias}");
        Plugin.Log.LogInfo($"DepthBias: {_shadowDepthBias}");
        Plugin.Log.LogInfo("------------------------");
    }

    [HarmonyPatch(typeof(GameRenderManager), nameof(GameRenderManager.SetShadowDistance))]
    [HarmonyPrefix]
    public static void SetShadowDistancePrefix(ref float shadowDistance)
    {
        // User-defined multiplier
        shadowDistance *= iShadowDistanceMultiplier.Value;
    }

    [HarmonyPatch(typeof(GameRenderManager), nameof(GameRenderManager.SetShadowDistance))]
    [HarmonyPostfix]
    public static void SetShadowDistancePostfix(ref float shadowDistance)
    {
        var asset = GameRenderManager.PipelineAsset;

        if (asset != null)
        {
            // Apply settings
            asset.supportsSoftShadows = true;
            asset.shadowCascadeCount = 4;
            asset.cascadeBorder = _cascadeBorder;
            asset.cascade4Split = _cascade4Split;
            asset.shadowNormalBias = _shadowNormalBias;
            asset.shadowDepthBias = _shadowDepthBias;
        }
    }
}
