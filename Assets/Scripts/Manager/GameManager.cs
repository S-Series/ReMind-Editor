using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

namespace GameData
{
    public enum GameMode { Line_4 = 4, Line_5 = 5, Line_6 = 6 }
}
public class GameManager : MonoBehaviour
{
    public static GameMode gameMode = GameMode.Line_4;
    public static List<T> FindAllObjects<T>() where T : Component
    {
        List<T> ret = new List<T>();
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                var rootObject = scene.GetRootGameObjects();
                for (int j = 0; j < rootObject.Length; j++)
                {
                    var go = rootObject[j];
                    ret.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        if (ret.Count == 0) { return null; }
        else { return ret; }
    }
    
    private void Awake()
    {
        UpdateGameMode(GameMode.Line_4);
    }
    public static void UpdateGameMode(GameMode mode)
    {
        gameMode = mode;
    }
}
