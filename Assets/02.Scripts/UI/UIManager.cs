using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class UIManager : Singleton<UIManager>
{
    public BattleUIManager battleUIManager;
    public BattleDialogueManager battleDialogueManager;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            battleUIManager = FindObjectOfType<BattleUIManager>();
            battleDialogueManager = FindObjectOfType<BattleDialogueManager>();
        }
    }

}
