using NaughtyAttributes;
using TendedTarsier.Core.Services.Modules;
using UnityEngine;

namespace TendedTarsier.Core.Modules.Project
{
    [CreateAssetMenu(menuName = "Config/ProjectConfig", fileName = "ProjectConfig")]
    public class ProjectConfig : ConfigBase
    {
        [field: SerializeField, Scene] public string MenuScene { get; private set; } = "Menu";
        [field: SerializeField, Scene] public string GameplayScene { get; private set; } = "Gameplay";
    }
}