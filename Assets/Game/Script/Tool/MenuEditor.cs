using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MenuEditor : MonoBehaviour
{
    [MenuItem("Tools/Scenes/Loading")]
    static void GetLoadingScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/0.Init.unity");
    }

    [MenuItem("Tools/Scenes/Game")]
    static void GetGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/1.Game.unity");
    }

    [MenuItem("Tools/Scenes/Tool Editor")]
    static void GetToolScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Tool.unity");
    }
}
