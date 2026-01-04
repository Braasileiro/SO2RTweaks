using Game;
using System;
using HarmonyLib;
using UnityEngine;

namespace SO2RTweaks.Patches
{
    internal class FieldCullingPatch
    {
        private static float MaxDistanceSq => Settings.fFieldCullingDistance.Value * Settings.fFieldCullingDistance.Value;

        private static bool IsAFuckingGrass(FieldCullingBase instance)
        {
            // Security checks
            if (Plugin.IsSceneTransitioning || instance == null || instance.gameObject == null)
            {
                return false;
            }

            // It's a fucking grass!!!
            if (instance is FieldGrass)
            {
                return true;
            }

            /*
             * I doubt that will happen, but it's better to check.
             * We will check individual parts that may be a fucking grass.
             */
            int layer = instance.gameObject.layer;

            // Ignore 20 (unknown layer from the game) and 5 (Unity UI default layer)
            if (layer == 20 || layer == 5)
            {
                return false;
            }

            // Name filtering
            string name = instance.name;

            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Ignore possible lights
            if (name.Contains("light", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // I noticed that almost everything that was a fucking grass began with the name 'ma_'
            return name.StartsWith("ma_", StringComparison.OrdinalIgnoreCase);
        }

        private static bool CanPatch(FieldCullingBase instance)
        {
            // Security checks
            if (Plugin.IsSceneTransitioning || instance == null)
            {
                return false;
            }

            // Current main camera
            Camera camera = GameCache.GetMainCamera();

            // Check if the current main camera exists and is valid
            if (camera == null || !camera.isActiveAndEnabled)
            {
                return false;
            }

            try
            {
                // Current transform
                var transform = instance.transform;

                if (transform != null && IsAFuckingGrass(instance))
                {
                    float distSq = Utils.SquareMagnitudeBetween(transform.position, camera.transform.position);

                    return distSq < MaxDistanceSq;
                }
            }
            catch (Exception e)
            {
                Plugin.Log.LogDebug($"Unhandled exception: {e.Message}");
            }

            return false;
        }

        [HarmonyPatch(typeof(FieldCullingBase), nameof(FieldCullingBase.SetVisible))]
        [HarmonyPrefix]
        public static void SetVisible(FieldCullingBase __instance, ref bool isVisible)
        {
            if (isVisible)
            {
                return;
            }

            if (CanPatch(__instance))
            {
                // Visible if below the max squared distance
                isVisible = true;
            }
        }

        [HarmonyPatch(typeof(FieldCullingBase), nameof(FieldCullingBase.SetEnableDither))]
        [HarmonyPrefix]
        public static void SetEnableDither(FieldCullingBase __instance, ref bool isEnable)
        {
            if (!isEnable)
            {
                return;
            }

            if (CanPatch(__instance))
            {
                // Disable dither if below the max squared distance
                isEnable = false;
            }
        }
    }
}
