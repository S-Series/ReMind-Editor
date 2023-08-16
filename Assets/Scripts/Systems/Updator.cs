using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Updator : MonoBehaviour
{
    struct GameData
    {
        public int VersionOrder;
        public double Version;
    }

    [SerializeField] GameObject[] PopUpObjects;
    [SerializeField][TextArea(1, 5)] string jsonDataURL;

    public void CheckUpdateVersion()
    {
        InputManager.EnableInput(false);
    }
    private IEnumerator CheckingVersion()
    {
        var data = Resources.Load("Version.json");

        GameData gameData, downloadData;
        gameData = JsonUtility.FromJson<GameData>
            ((Resources.Load("Version.json") as TextAsset).ToString());

        UnityWebRequest request;
        request = UnityWebRequest.Get(jsonDataURL);
        request.disposeDownloadHandlerOnDispose = true;
        request.timeout = 60;

        yield return request.SendWebRequest();

        if (request.isDone)
        {
            if (request.result != UnityWebRequest.Result.ConnectionError)
            { 
                downloadData = JsonUtility.FromJson<GameData>(request.downloadHandler.text);
            }
            else { UpdatePopUp(null); }
        }
        else { UpdatePopUp(null); }
    }
    private void UpdatePopUp(bool? isLastestVersion)
    {
        //$ Failed
        if (isLastestVersion == null)
        {
            PopUpObjects[0].SetActive(true);
        }

        //$ Lastest
        if (isLastestVersion == true)
        {
            PopUpObjects[1].SetActive(true);
        }
        //$ Old
        else { PopUpObjects[2].SetActive(true); }
    }

    [ContextMenu("Save Version data")]
    private void SaveVersionData()
    {
        GameData gameData;
        gameData = new GameData();

        gameData.VersionOrder = 0;
        gameData.Version = 1.0;

        string jsonData;
        jsonData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(Application.dataPath + @"\Resources\Version.json", jsonData);
    }
}
