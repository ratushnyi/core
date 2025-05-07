using UnityEngine;

namespace TendedTarsier.Core.Modules.Menu
{
    [CreateAssetMenu(menuName = "Config/MenuConfig", fileName = "MenuConfig")]
    public class MenuConfig : ScriptableObject
    {
        [field: SerializeField] public MenuPanel MenuPanel { get; set; }
    }
}