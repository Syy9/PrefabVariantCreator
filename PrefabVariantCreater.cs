using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Syy.Tools
{
    public class PrefabVariantCreater : EditorWindow
    {
        [MenuItem("Assets/Create/Prefab Variant (Output)", priority = 210)]
        static void Create()
        {
            var prefab = Selection.activeGameObject;
            if (prefab == null)
            {
                return;
            }

            var i = GetWindow<PrefabVariantCreater>();
            i.prefab = prefab;
            i.newPrefabName = prefab.name;
        }

        [MenuItem("Assets/Create/Prefab Variant (Output)", validate = true, priority = 210)]
        static bool CreateValidate()
        {
            return Selection.activeGameObject != null;
        }

        GameObject prefab;
        string newPrefabName;
        DefaultAsset outputFolder;

        void OnGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
                if (check.changed)
                {
                    newPrefabName = prefab?.name;
                }
            }
            newPrefabName = EditorGUILayout.TextField("New Prefab Name", newPrefabName);
            outputFolder = (DefaultAsset)EditorGUILayout.ObjectField("Output Folder", outputFolder, typeof(DefaultAsset), false);

            using (new EditorGUI.DisabledScope(prefab == null || outputFolder == null))
            {
                if (GUILayout.Button("Create"))
                {
                    var assetPath = $"{AssetDatabase.GetAssetPath(outputFolder)}/{newPrefabName}.prefab";
                    assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
                    var instanceRoot = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    PrefabUtility.SaveAsPrefabAssetAndConnect(instanceRoot, assetPath, InteractionMode.AutomatedAction);
                    GameObject.DestroyImmediate(instanceRoot);
                    EditorUtility.DisplayDialog("Complete", $"Create prefab variant.\n{assetPath}", "Close");
                }
            }
        }
    }
}
