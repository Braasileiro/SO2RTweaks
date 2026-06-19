using UnityEngine;

namespace SO2RTweaks;

internal static class GameCache
{
    private static Camera _mainCamera;

    public static Camera GetMainCamera()
    {
        // Get a new main camera
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        // Clear invalid camera instance
        if (_mainCamera != null && (!_mainCamera.isActiveAndEnabled || _mainCamera.gameObject == null))
        {
            _mainCamera = null;
        }

        return _mainCamera;
    }
}
