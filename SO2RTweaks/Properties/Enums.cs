using static Common.InputManager;

internal enum ERunInBackground
{
    Auto,
    Enabled,
    Disabled
}

internal enum EButtonPrompts
{
    Auto = GamepadType.Invalid,
    PS4 = GamepadType.PS4,
    PS5 = GamepadType.PS5,
    Switch = GamepadType.Switch,
    Xbox360 = GamepadType.PC
}
