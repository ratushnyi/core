using UnityEngine;

namespace TendedTarsier.Core.Utilities.Extensions
{
    public static class LayerExtensions
    {
        public static void SetLayerRecursively(this GameObject gameObject, int newLayer)
        {
            if (gameObject == null)
            {
                return;
            }

            gameObject.layer = newLayer;

            foreach (Transform child in gameObject.transform)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}