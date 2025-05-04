using NaughtyAttributes;
using UnityEngine;

namespace TendedTarsier.Core.Modules.General
{
    [CreateAssetMenu(menuName = "Config/GeneralConfig", fileName = "GeneralConfig")]
    public class GeneralConfig : ScriptableObject
    {
        [field: SerializeField, Scene]
        public string GameplayScene { get; set; }
    }
}