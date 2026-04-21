using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance {get; private set;}
    public GameData gameData;

    public void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else{
            Destroy(gameObject);
        }

        if(gameData == null){
            gameData = new GameData();
        }
    }

    public void SaveGameData(){
        string json = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();
        Debug.Log("GameData Saved: " + json);
    }

    // ÉfÅ[É^Çì«Ç›çûÇ›
    public void LoadGameData()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            string json = PlayerPrefs.GetString("GameData");
            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("GameData Loaded: " + json);
        }
        else
        {
            Debug.Log("No saved GameData found. Initializing default values.");
            gameData = new GameData();
        }
    }
}
