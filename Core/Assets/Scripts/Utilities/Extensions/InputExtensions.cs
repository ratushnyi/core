using UnityEngine;
using UnityEngine.EventSystems;

namespace TendedTarsier.Core.Utilities.Extensions
{
    public static class InputExtensions
    {
        public static bool IsOverUI(int pointerId)
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(pointerId);
        }

        public static bool IsMouseKeyboardInput => !Application.isMobilePlatform && !Application.isConsolePlatform;
    }
}