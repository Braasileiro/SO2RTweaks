using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using static Settings;

namespace SO2RTweaks
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        internal static new ManualLogSource Log;
        internal static new ConfigFile Config;
        internal static Harmony HarmonyInstance;

        public override void Load()
        {
            // Globals
            Log = base.Log;
            Config = base.Config;
            HarmonyInstance = new(MyPluginInfo.PLUGIN_GUID);

            Log.LogInfo($"{MyPluginInfo.PLUGIN_NAME} loaded.");

            // Settings
            Settings.Load();
            
            if (iRunInBackground.Value != ERunInBackground.Auto)
            {
                Application.runInBackground = iRunInBackground.Value == ERunInBackground.Enabled;

                Log.LogInfo($"Application run in background mode set to '{iRunInBackground.Value}'.");
            }

            if (iAnisotropicFiltering.Value > 0)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                Texture.SetGlobalAnisotropicFilteringLimits(iAnisotropicFiltering.Value, iAnisotropicFiltering.Value);

                Log.LogInfo($"Anisotropic filtering set to {iAnisotropicFiltering.Value}x ({QualitySettings.anisotropicFiltering}).");
            }

            // Patches
            if (bSkipLogos.Value || bSkipOpeningMovie.Value)
            {
                HarmonyInstance.PatchAll(typeof(Patches.SkipIntroPatch));

                Log.LogInfo("Applied skip intro patch.");
            }

            if (iButtonPrompts.Value != EButtonPrompts.Auto)
            {
                HarmonyInstance.PatchAll(typeof(Patches.ButtonPromptsPatch));

                Log.LogInfo("Applied button prompts patch.");
            }

            if (iFrameRateLimit.Value >= 0)
            {
                HarmonyInstance.PatchAll(typeof(Patches.FrameRateLimitPatch));

                _ = Patches.FrameRateLimitPatch.SetFrameRateLimitAsync();

                Log.LogInfo("Applied framerate limit patch.");
            }

            if (iPostProcessAA.Value != EPostProcessAA.None)
            {
                HarmonyInstance.PatchAll(typeof(Patches.PostProcessAAPatch));

                Log.LogInfo("Applied post-process anti-aliasing patch.");
            }

            if (bDisableVignette.Value)
            {
                HarmonyInstance.PatchAll(typeof(Patches.DisableVignettePatch));

                Log.LogInfo("Applied disable vignette patch.");
            }
        }
    }
}
