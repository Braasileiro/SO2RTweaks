using Game;
using HarmonyLib;
using UnityEngine;
using static Settings;

namespace SO2RTweaks.Patches
{
    internal class ShadowDistancePatch
    {
        // States
        private static bool _notified = false;

        // Calculated
        private static readonly float _cascadeBorder;
        private static readonly Vector3 _cascade4Split;
        private static readonly float _shadowNormalBias;
        private static readonly float _shadowDepthBias;

        static ShadowDistancePatch()
        {
            // Interpolation factor based on the user-selected multiplier (scales from 2x up to 4x)
            float factor = Mathf.InverseLerp(
                MinShadowDistanceMultiplier,
                MaxShadowDistanceMultiplier,
                iShadowDistanceMultiplier.Value
            );

            // The cascade border defines the blend zone between different shadow resolutions
            // Starting at 30% ensures that transition seams are good enough at 2x distance
            _cascadeBorder = Mathf.Lerp(0.3f, 0.45f, factor);

            /*
             * Shadow Cascade Distribution (4)
             * 1st split (30%): Total distance with maximum resolution
             * 2nd split (45%): Maintains a good mid-range resolution
             * 3rd split (65% to 80%): Scales the last cascade towards the horizon
             */
            _cascade4Split = new(0.3f, 0.45f, Mathf.Lerp(0.65f, 0.80f, factor));

            // Adjusts shadow bias to prevent artifacts and shadows disconnected from the object
            // Adjusted for scaling from 2x to 4x
            _shadowNormalBias = Mathf.Lerp(0.6f, 1.2f, factor);
            _shadowDepthBias = Mathf.Lerp(0.6f, 0.9f, factor);
        }

        [HarmonyPatch(typeof(GameRenderManager), nameof(GameRenderManager.SetShadowDistance))]
        [HarmonyPrefix]
        public static void SetShadowDistancePrefix(ref float shadowDistance)
        {
            // User-defined multiplier
            shadowDistance *= iShadowDistanceMultiplier.Value;
        }

        [HarmonyPatch(typeof(GameRenderManager), nameof(GameRenderManager.SetShadowDistance))]
        [HarmonyPrefix]
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

                if (!_notified)
                {
                    Plugin.Log.LogInfo("Calculated pipeline asset shadow settings.");
                    Plugin.Log.LogInfo("------------------------");
                    Plugin.Log.LogInfo($"DistanceMultiplier: {iShadowDistanceMultiplier.Value}x");
                    Plugin.Log.LogInfo($"SoftShadows: {asset.supportsSoftShadows}");
                    Plugin.Log.LogInfo($"CascadeCount: {asset.shadowCascadeCount}");
                    Plugin.Log.LogInfo($"CascadeBorder: {asset.cascadeBorder:F2}");
                    Plugin.Log.LogInfo($"Cascade4Split: {asset.cascade4Split}");
                    Plugin.Log.LogInfo($"NormalBias: {asset.shadowNormalBias:F2}");
                    Plugin.Log.LogInfo($"DepthBias: {asset.shadowDepthBias:F2}");
                    Plugin.Log.LogInfo("------------------------");

                    _notified = true;
                }
            }
        }
    }
}
