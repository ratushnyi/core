#if UNITY_EDITOR
using System.IO;
using System.Linq;
using TendedTarsier.Core.Services.Profile;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace TendedTarsier.Core.Utilities
{
    [InitializeOnLoad]
    public static class MenuUtilities
    {
        private const string RootMenu = "TendedTarsier/";
        private const string ProfileMenu = RootMenu + "User Profile/";
        
        static MenuUtilities()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }
        
        static void OnToolbarGUI()
        {
            if (GUILayout.Button(new GUIContent("Load first scene", "Play first enabled scene")))
            {
                StartLoading();
            }
        }

        private static void StartLoading()
        {
            EditorApplication.isPlaying = false;
            EditorSceneManager.OpenScene(EditorBuildSettings.scenes.First(s => s.enabled).path);
            EditorApplication.isPlaying = true;
        }

        [MenuItem(ProfileMenu + "Clean Profiles", false, 1)]
        private static void CleanProfiles()
        {
            if (Directory.Exists(ProfileService.ProfilesDirectory))
            {
                Directory.Delete(ProfileService.ProfilesDirectory, true);
            }
        }
        
        [MenuItem(RootMenu + "Replace Selected With Prefab", false, 1)]
        public static void ReplaceSelected()
        {
            var prefab = Selection.objects.FirstOrDefault(t => t != null && PrefabUtility.IsPartOfPrefabAsset(t));
            if (prefab == null)
            {
                return;
            }

            foreach (GameObject original in Selection.gameObjects)
            {
                if (PrefabUtility.IsPartOfPrefabAsset(original)) continue;

                Transform parent = original.transform.parent;
                int siblingIndex = original.transform.GetSiblingIndex();

                GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                newInstance.transform.localPosition = original.transform.localPosition;
                newInstance.transform.localRotation = original.transform.localRotation;
                newInstance.transform.localScale = original.transform.localScale;
                newInstance.transform.SetSiblingIndex(siblingIndex);

                copyComponentOverrides(original, newInstance);
                newInstance.name = original.name;

                Undo.RegisterCreatedObjectUndo(newInstance, "Replace with Prefab");
                Undo.DestroyObjectImmediate(original);
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            
            static void copyComponentOverrides(GameObject source, GameObject target)
            {
                var sourceComponents = source.GetComponents<Component>();
                var targetComponents = target.GetComponents<Component>();

                for (int i = 0; i < sourceComponents.Length && i < targetComponents.Length; i++)
                {
                    if (sourceComponents[i] == null || targetComponents[i] == null)
                        continue;

                    var soSource = new SerializedObject(sourceComponents[i]);
                    var soTarget = new SerializedObject(targetComponents[i]);

                    var prop = soSource.GetIterator();
                    if (prop.NextVisible(true))
                    {
                        do
                        {
                            if (prop.name == "m_Script") continue;
                            soTarget.CopyFromSerializedProperty(prop);
                        }
                        while (prop.NextVisible(false));
                    }
                    soTarget.ApplyModifiedProperties();
                }
            }
        }
    }
}
#endif