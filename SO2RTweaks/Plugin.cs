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

                Log.LogInfo($"Anisotropic filtering mode set to '{QualitySettings.anisotropicFiltering}'.");
                Log.LogInfo($"Anisotropic filtering level set to '{iAnisotropicFiltering.Value}'.");
            }

            // Patches
            if (iButtonPrompts.Value != EButtonPrompts.Auto)
            {
                HarmonyInstance.PatchAll(typeof(ButtonPromptsPatch));

                Log.LogInfo("Applied button prompts patch.");
            }

            if (iFrameRateLimit.Value > 0 || iFrameRateLimit.Value == -1)
            {
                HarmonyInstance.PatchAll(typeof(FrameRateLimitPatch));

                _ = FrameRateLimitPatch.SetFrameRateLimitAsync();

                Log.LogInfo("Applied framerate limit patch.");
            }
        }
    }
}
