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
    public static ConfigEntry<int> iShadowDistanceMultiplier;
    public static ConfigEntry<float> fFieldGrassCullingDistance;

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
            "Select an arbitrary framerate limit, ignoring the game setting.\n" +
            "VSync must be disabled in-game.\n" +
            "Setting this to '-1' will use the game setting (framerate cap or refresh rate cap when VSync is on).\n" +
            "Setting this to '0' effectively unlocks the framerate (when VSync is off)."
        );

        iAnisotropicFiltering = Plugin.Config.Bind(
            "Graphics",
            "AnisotropicFiltering",
            16,
            new ConfigDescription(
                "Set the anisotropic filtering level, forced on all textures.\n" +
                "Improves clarity for textures viewed from a distance or at an angle.\n" +
                "Setting this to '0' will use the game setting.",
            new AcceptableValueRange<int>(0, 16))
        );

        iPostProcessAA = Plugin.Config.Bind(
            "Graphics",
            "PostProcessAA",
            EPostProcessAA.None,
            "Post-process anti-aliasing method to use.\n" +
            "By default, the game only uses MSAA, which can be configured in the menu. This setting will add a post-processing method on top of MSAA.\n" +
            "FXAA removes more jagged edges, but it is slightly more blurry.\n" +
            "SMAA produces a sharper image."
        );

        bDisableVignette = Plugin.Config.Bind(
            "Graphics",
            "DisableVignette",
            false,
            "Disable vignette effect.\n" +
            "Vignette is a visual effect that darkens the corners of the screen."
        );

        iShadowDistanceMultiplier = Plugin.Config.Bind(
            "Graphics",
            "ShadowDistanceMultiplier",
            1,
            new ConfigDescription(
                "Set the general shadow draw distance.\n" +
                "The rendering distance for shadows in the game is quite low, around 35 to 50 meters. This causes an effect similar to mesh swap pop-in, as objects stop to render shadows depending on distance.\n" +
                "I recommend leaving the multiplier at 2. This value will not significantly compromise the appearance of the shadows.\n" +
                "Above 2, shadows may appear softer where they would normally be more defined.\n" +
                "Don't forget to tweak the 'FieldGrassCullingDistance' setting for the best experience.\n" +
                "Using a high multiplier may cause performance degradation depending on your setup.\n" +
                "Setting this to '1' will use the game setting.",
            new AcceptableValueRange<int>(1, 4))
        );

        fFieldGrassCullingDistance = Plugin.Config.Bind(
            "Graphics",
            "FieldGrassCullingDistance",
            0f,
            new ConfigDescription(
                "Set the field bushes and plants culling distance.\n" +
                "The game uses a very low value, around 15 to 50 meters. This causes vegetation objects such as bushes and plants to disappear very close to the camera.\n" +
                "I recommend leaving the value at 300. In my tests, this was more than enough to stop objects from popping in.\n" +
                "Don't forget to set the 'Cull Distance' option in the game to 'Farthest' for the best experience.\n" +
                "This setting does not affect the terrain grass, which fade away smoothly as you walk away.\n" +
                "Using a high value may cause performance degradation depending on your setup.\n" +
                "Setting this to '0' will use the game setting.",
            new AcceptableValueRange<float>(0f, 1000f))
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
        Plugin.Log.LogInfo($"ShadowDistanceMultiplier: {iShadowDistanceMultiplier.Value}");
        Plugin.Log.LogInfo($"FieldGrassCullingDistance: {fFieldGrassCullingDistance.Value}");
        Plugin.Log.LogInfo("------------------------");
    }
}
