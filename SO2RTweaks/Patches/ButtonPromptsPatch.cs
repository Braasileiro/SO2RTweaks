using Common;
using HarmonyLib;
using static Settings;
using static Common.InputManager;

namespace SO2RTweaks.Patches
{
    internal class ButtonPromptsPatch
    {
        [HarmonyPatch(typeof(InputManager), nameof(InputManager.GetGamepadType))]
        [HarmonyPostfix]
        public static void GetGamepadType(ref GamepadType __result)
        {
            __result = (GamepadType)iButtonPrompts.Value;
        }
    }
}
