using NaughtyAttributes;
using UnityEngine;

namespace TendedTarsier.Core.Modules.Project
{
    [CreateAssetMenu(menuName = "Config/ProjectConfig", fileName = "ProjectConfig")]
    public class ProjectConfig : ScriptableObject
    {
        [field: SerializeField, Scene] public string MenuScene { get; private set; } = "Menu";
        [field: SerializeField, Scene] public string GameplayScene { get; private set; } = "Gameplay";
    }
}