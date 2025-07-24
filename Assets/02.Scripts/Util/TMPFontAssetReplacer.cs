using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TMPFontAssetReplacer : EditorWindow
{
    private TMP_FontAsset newFontAsset;
    private string searchFolder = "Assets/03.Prefabs/UI"; // 검색할 폴더 경로

    [MenuItem("Tools/TMP Font Asset Replacer")]
    public static void ShowWindow()
    {
        GetWindow<TMPFontAssetReplacer>("TMP Font Asset Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("TextMeshPro Font Asset Replacer", EditorStyles.boldLabel);

        newFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font Asset", newFontAsset, typeof(TMP_FontAsset), false);
        searchFolder = EditorGUILayout.TextField("Search Folder", searchFolder);

        if (GUILayout.Button("Replace Fonts in Prefabs"))
        {
            ReplaceFontsInPrefabs();
        }
    }

    private void ReplaceFontsInPrefabs()
    {
        string[] prefabPaths = Directory.GetFiles(searchFolder, "*.prefab", SearchOption.AllDirectories);

        foreach (string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            bool changed = false;

            // 모든 하위 오브젝트에서 TMP 컴포넌트 찾기
            TextMeshProUGUI[] texts = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (var text in texts)
            {
                if (text.font != newFontAsset)
                {
                    Undo.RecordObject(text, "Replace TMP Font");
                    text.font = newFontAsset;
                    changed = true;
                }
            }

            if (changed)
            {
                EditorUtility.SetDirty(prefab);
                PrefabUtility.SavePrefabAsset(prefab);
                Debug.Log($"Font replaced in : {path}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
