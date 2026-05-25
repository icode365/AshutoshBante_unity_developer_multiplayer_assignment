using UnityEngine;

namespace Assignment.Scripts.Multiplayer.Managers
{
    public static class CursorManager
    {
        public static void SetGameplayMode()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public static void SetUIMode()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}