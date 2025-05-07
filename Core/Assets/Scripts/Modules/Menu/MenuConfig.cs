using NaughtyAttributes;
using TendedTarsier.Core.Modules.General;
using UnityEngine;

namespace TendedTarsier.Core.Modules.Menu
{
    [CreateAssetMenu(menuName = "Config/MenuConfig", fileName = "MenuConfig")]
    public class MenuConfig : ScriptableObject
    {
        [field: SerializeField] public LoadingPanel LoadingPanel { get; set; }
        [field: SerializeField] public MenuPanel MenuPanel { get; set; }
        [field: SerializeField, Scene] public string NewGameScene { get; set; }
    }
}