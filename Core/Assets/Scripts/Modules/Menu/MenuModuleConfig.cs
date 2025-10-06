using NaughtyAttributes;
using TendedTarsier.Core.Services.Modules;
using UnityEngine;

namespace TendedTarsier.Core.Modules.Menu
{
    [CreateAssetMenu(menuName = "Config/MenuModuleConfig", fileName = "MenuModuleConfig")]
    public class MenuModuleConfig : ModuleConfigBase
    {
        [field: SerializeField] public MenuPanel MenuPanel { get; set; }
    }
}