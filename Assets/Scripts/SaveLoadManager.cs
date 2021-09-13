using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public string path;

    // 타이틀 씬에서 Load하고 이후 save를 위해 DontDestroyOnLoad. 
    // 그러므로 스테이지 선택 씬이나 인게임 씬에서 테스트 돌릴 시 정상 작동 안될 수 있음
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        path = Path.Combine(Application.persistentDataPath, "save.json");
        Load();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
    public void Save()
    {
        UserProperty data = new UserProperty();
        data.gold = Global.userProperty.gold;
        data.ruby = Global.userProperty.ruby;
        // data.stamina = Global.userProperty.stamina;
        data.nextStaminaRegenTime = Global.userProperty.nextStaminaRegenTime;

        string jsonDataString = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, jsonDataString);

        Debug.Log("Saved");
    }
    public void Load()
    {
        if (File.Exists(path))
        {
            string loadedJsonDataString = File.ReadAllText(path);

            UserProperty data = JsonUtility.FromJson<UserProperty>(loadedJsonDataString);
            Debug.Log("gold: " + data.gold.ToString() + " | ruby: " + data.ruby.ToString());

            Global.userProperty.gold = data.gold;
            Global.userProperty.ruby = data.ruby;
            // Global.userProperty.stamina = data.stamina;
            Global.userProperty.nextStaminaRegenTime = data.nextStaminaRegenTime;

            GameObject selectStageManager = GameObject.Find("StageSelectManager");
            if (selectStageManager != null)
            {
                selectStageManager.GetComponent<StageSelectSceneManager>().RefreshUserPropertyData();
            }
        }
    }
    */

    public void Save()
    {
        PlayerPrefs.SetInt("gold", Global.userProperty.gold);
        PlayerPrefs.SetInt("ruby", Global.userProperty.ruby);
        PlayerPrefs.Save();
        Debug.Log("Saved");
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("gold"))
        {
            Global.userProperty.gold = PlayerPrefs.GetInt("gold");
            Global.userProperty.ruby = PlayerPrefs.GetInt("ruby");
            Debug.Log("Loaded");
        }
        else
        {
            // 초기 소지값
            Global.userProperty.gold = 1000;
            Global.userProperty.ruby = 0;
        }
    }
}
