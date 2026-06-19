using SO2RTweaks;
using BepInEx.Configuration;

internal class Settings
{
    // General
    public static ConfigEntry<bool> bRunInBackground;
    public static ConfigEntry<EButtonPrompts> iButtonPrompts;
    public static ConfigEntry<bool> bSkipLogos;
    public static ConfigEntry<bool> bSkipOpeningMovie;

    // Graphics
    public static ConfigEntry<int> iFrameRateLimit;
    public static ConfigEntry<int> iAnisotropicFiltering;
    public static ConfigEntry<EPostProcessAA> iPostProcessAA;
    public static ConfigEntry<bool> bDisableVignette;
    public static ConfigEntry<float> fFieldGrassCullingDistance;
    public static ConfigEntry<int> iShadowDistanceMultiplier;

    public static void Load()
    {
        // General
        bRunInBackground = Plugin.Config.Bind(
            "General",
            "RunInBackground",
            true,
            "The game runs in the background by default.\n" +
            "You can disable this if you want the game to pause when unfocused."
        );

        iButtonPrompts = Plugin.Config.Bind(
            "General",
            "ButtonPrompts",
            EButtonPrompts.Auto,
            "Button prompts you want to use."
        );

        bSkipLogos = Plugin.Config.Bind(
            "General",
            "SkipLogos",
            false,
            "Skip intro logos."
        );

        bSkipOpeningMovie = Plugin.Config.Bind(
            "General",
            "SkipOpeningMovie",
            false,
            "Skip the opening movie."
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
            EPostProcessAA.SMAA,
            "Post-process anti-aliasing method to use.\n" +
            "By default, the game only uses MSAA, which can be configured in the menu. This setting will add a post-processing method on top of MSAA.\n" +
            "FXAA removes more jagged edges, but it is slightly more blurry.\n" +
            "SMAA produces a sharper image."
        );

        bDisableVignette = Plugin.Config.Bind(
            "Graphics",
            "DisableVignette",
            false,
            "Disable the vignette effect.\n" +
            "Vignette is a visual effect that darkens the corners of the screen."
        );

        fFieldGrassCullingDistance = Plugin.Config.Bind(
            "Graphics",
            "FieldGrassCullingDistance",
            300f,
            new ConfigDescription(
                "Set the field bushes and plants culling distance.\n" +
                "The game uses a very low value, around 15 to 50 meters. This causes vegetation objects such as bushes and plants to disappear very close to the camera.\n" +
                "I recommend leaving the value at 300. In my tests, this was more than enough to stop objects from popping in.\n" +
                "Don't forget to set the 'Cull Distance' option in the game to 'Farthest' for the best experience.\n" +
                "This setting does not affect the terrain grass, which fades in smoothly as you walk.\n" +
                "Using a high value may impact performance depending on your configuration.\n" +
                "Setting this to '0' will use the game setting.",
            new AcceptableValueRange<float>(0f, 1000f))
        );

        iShadowDistanceMultiplier = Plugin.Config.Bind(
            "Graphics",
            "ShadowDistanceMultiplier",
            4,
            new ConfigDescription(
                "Set the general shadow draw distance.\n" +
                "The rendering distance for shadows in the game is quite low, around 35 to 50 meters. This causes an effect similar to mesh swap pop-in, as objects stop rendering shadows depending on distance.\n" +
                "I recommend leaving the multiplier at 4.\n" +
                "At 2x, shadows will be sharper up close, but it doesn't cover a large area.\n" +
                "At 4x, you get a smoother result, and it covers a larger area. I think it's worth the tradeoff.\n" +
                "Don't forget to set the 'Shadow Quality' option in the game to 'Ultra' for the best experience.\n" +
                "May impact performance depending on your configuration.\n" +
                "Setting this to '1' will use the game setting.",
            new AcceptableValueRange<int>(1, 4))
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
        Plugin.Log.LogInfo($"FieldGrassCullingDistance: {fFieldGrassCullingDistance.Value}");
        Plugin.Log.LogInfo($"ShadowDistanceMultiplier: {iShadowDistanceMultiplier.Value}");
        Plugin.Log.LogInfo("------------------------");
    }
}
