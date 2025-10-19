using Common;
using UnityEngine.Rendering.Universal;

internal enum ERunInBackground
{
    Auto,
    Enabled,
    Disabled
}

internal enum EButtonPrompts
{
    Auto = InputManager.GamepadType.Invalid,
    PS4 = InputManager.GamepadType.PS4,
    PS5 = InputManager.GamepadType.PS5,
    Switch = InputManager.GamepadType.Switch,
    Xbox360 = InputManager.GamepadType.PC
}

internal enum EPostProcessAA
{
    None = AntialiasingMode.None,
    FXAA = AntialiasingMode.FastApproximateAntialiasing,
    SMAA = AntialiasingMode.SubpixelMorphologicalAntiAliasing
}
