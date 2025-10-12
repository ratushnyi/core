using UnityEngine.EventSystems;

namespace TendedTarsier.Core.Utilities.Extensions
{
    public static class UIExtensions
    {
        public static bool IsOverUI(int pointerId)
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(pointerId);
        }
    }
}