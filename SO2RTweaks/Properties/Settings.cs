using SO2RTweaks;
using BepInEx.Configuration;

internal class Settings
{
    // General
    public static ConfigEntry<bool> bRunInBackground;
    public static ConfigEntry<EButtonPrompts> iButtonPrompts;

    // SkipIntro
    public static ConfigEntry<bool> bSkipLogos;
    public static ConfigEntry<bool> bSkipOpeningMovie;

    // Graphics
    public static ConfigEntry<int> iFrameRateLimit;
    public static ConfigEntry<int> iAnisotropicFiltering;
    public static ConfigEntry<EPostProcessAA> iPostProcessAA;
    public static ConfigEntry<bool> bDisableVignette;

    public static void Load()
    {
        // General
        bRunInBackground = Plugin.Config.Bind(
            "General",
            "RunInBackground",
            true,
            "The game runs in the background by default.\nYou can disable this if you wish."
        );

        iButtonPrompts = Plugin.Config.Bind(
            "General",
            "ButtonPrompts",
            EButtonPrompts.Auto,
            "Button prompts you want to use."
        );

        // SkipIntro
        bSkipLogos = Plugin.Config.Bind(
            "SkipIntro",
            "SkipLogos",
            false,
            "Skip intro logos."
        );

        bSkipOpeningMovie = Plugin.Config.Bind(
            "SkipIntro",
            "SkipOpeningMovie",
            false,
            "Skip intro opening movie."
        );

        // Graphics
        iFrameRateLimit = Plugin.Config.Bind(
            "Graphics",
            "FrameRateLimit",
            -1,
            "Select an arbitrary framerate limit, ignoring the game setting.\nVSync must be disabled in-game.\nSetting this to '-1' will use the game setting (framerate cap or refresh rate cap when VSync is on).\nSetting this to '0' effectively unlocks the framerate (when VSync is off)."
        );

        iAnisotropicFiltering = Plugin.Config.Bind(
            "Graphics",
            "AnisotropicFiltering",
            16,
            new ConfigDescription($"Set the anisotropic filtering level, forced on all textures.\nImproves clarity for textures viewed from a distance or at an angle.\nSetting this to '0' will use the game setting.",
            new AcceptableValueRange<int>(0, 16))
        );

        iPostProcessAA = Plugin.Config.Bind(
            "Graphics",
            "PostProcessAA",
            EPostProcessAA.None,
            "Post-process anti-aliasing method to use.\nBy default, the game only uses MSAA, which can be configured in the menu. This setting will add a post-processing method on top of MSAA.\nFXAA removes more jagged edges, but it is slightly more blurry.\nSMAA produces a sharper image."
        );

        bDisableVignette = Plugin.Config.Bind(
            "Graphics",
            "DisableVignette",
            false,
            "Disable vignette effect.\nVignette is a visual effect that darkens the corners of the screen."
        );

        Plugin.Log.LogInfo("------------------------");
        Plugin.Log.LogInfo($"RunInBackground: {bRunInBackground.Value}");
        Plugin.Log.LogInfo($"ButtonPrompts: {iButtonPrompts.Value}");
        Plugin.Log.LogInfo($"SkipLogos: {bSkipLogos.Value}");
        Plugin.Log.LogInfo($"SkipOpeningMovie: {bSkipOpeningMovie.Value}");
        Plugin.Log.LogInfo($"FrameRateLimit: {iFrameRateLimit.Value}");
        Plugin.Log.LogInfo($"AnisotropicFiltering: {iAnisotropicFiltering.Value}");
        Plugin.Log.LogInfo($"PostProcessAA: {iPostProcessAA.Value}");
        Plugin.Log.LogInfo($"DisableVignette: {bDisableVignette.Value}");
        Plugin.Log.LogInfo("------------------------");
    }
}
