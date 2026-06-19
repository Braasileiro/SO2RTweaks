using Game;
using System;
using HarmonyLib;
using UnityEngine;

namespace SO2RTweaks.Patches;

internal class FieldGrassCullingPatch
{
    private static readonly float _maxDistanceSq = Settings.fFieldGrassCullingDistance.Value * Settings.fFieldGrassCullingDistance.Value;

    private static bool IsFuckingGrass(FieldCullingBase instance)
    {
        string className = instance.GetIl2CppType().Name;

        // Only for grass
        return className == "FieldGrass";
    }

    private static bool CanPatch(FieldCullingBase instance)
    {
        // Security checks
        if (Plugin.IsSceneTransitioning || instance == null || instance.gameObject == null)
        {
            return false;
        }

        // Is a fucking grass?!
        if (IsFuckingGrass(instance))
        {
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

                if (transform != null)
                {
                    float distSq = Utils.SquareMagnitudeBetween(transform.position, camera.transform.position);

                    return distSq < _maxDistanceSq;
                }
            }
            catch (Exception e)
            {
                Plugin.Log.LogDebug($"Unhandled exception: {e.Message}");
            }
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
