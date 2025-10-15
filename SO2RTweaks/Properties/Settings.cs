using SO2RTweaks;
using BepInEx.Configuration;

internal class Settings
{
    // General
    public static ConfigEntry<ERunInBackground> iRunInBackground;
    public static ConfigEntry<EButtonPrompts> iButtonPrompts;

    // Graphics
    public static ConfigEntry<int> iFrameRateLimit;
    public static ConfigEntry<int> iAnisotropicFiltering;

    public static void Load()
    {
        iRunInBackground = Plugin.Config.Bind(
            "General",
            "RunInBackground",
            ERunInBackground.Auto,
            "The game runs in the background by default.\nYou can disable this if you wish."
        );

        iButtonPrompts = Plugin.Config.Bind(
            "General",
            "ButtonPrompts",
            EButtonPrompts.Auto,
            "Button prompts you want to use."
        );

        iFrameRateLimit = Plugin.Config.Bind(
            "Graphics",
            "FrameRateLimit",
            0,
            "Select an arbitrary framerate limit, ignoring the game setting.\nVSync must be disabled in-game.\nSetting this to '0' will use the game setting (fps cap or refresh rate cap when VSync is on).\nSetting this to '-1' effectively unlocks the framerate (when VSync is off)."
        );

        iAnisotropicFiltering = Plugin.Config.Bind(
            "Graphics",
            "AnisotropicFiltering",
            16,
            new ConfigDescription($"Set the anisotropic filtering level, forced on all textures.\nImproves clarity for textures viewed from a distance or at an angle.\nSetting this to '0' will use the game setting.",
            new AcceptableValueRange<int>(0, 16))
        );

        Plugin.Log.LogInfo("----------------------");
        Plugin.Log.LogInfo($"RunInBackground: {iRunInBackground.Value}");
        Plugin.Log.LogInfo($"ButtonPrompts: {iButtonPrompts.Value}");
        Plugin.Log.LogInfo($"FrameRateLimit: {iFrameRateLimit.Value}");
        Plugin.Log.LogInfo($"AnisotropicFiltering: {iAnisotropicFiltering.Value}");
        Plugin.Log.LogInfo("----------------------");
    }
}
