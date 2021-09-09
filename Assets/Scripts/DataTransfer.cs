using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataTransfer : MonoBehaviour
{
    [SerializeField] int chapter;
    [SerializeField] int stage;

    [SerializeField] int chapterIndexInCSV;
    [SerializeField] int stageIndexInCSV;
    [SerializeField] int spawnTimeIndexInCSV;
    [SerializeField] int maxEnemyCountIndexInCSV;
    [SerializeField] int enemyDataStartIndexInCSV;
    WaveManager waveManager;
    // Start is called before the first frame update
    void Start()
    {
        waveManager = GameObject.Find("GameManager").GetComponent<WaveManager>();

        SetStageData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetStageData()
    {
        string[] rawData = ReadStageData(chapter, stage);
        Debug.Log(rawData);
        List<Wave> waves = new List<Wave>();
        for (int i = 0; i < rawData.Length; i++)
        {
            Wave wave = ParseLines(rawData[i]);
            waves.Add(wave);
        }
        waveManager.setWaves(waves.ToArray());
    }

    string[] ReadStageData(int chapter, int stage)
    {
        var stringAll = Resources.Load("waveDesign") as TextAsset;
        string[] lines = stringAll.text.Split('\n');
        Debug.Log(lines.Length);
        List<string> stageDataStrings = new List<string>();
        for (int i = 1 /*범례 무시*/; i < lines.Length; i++)
        {
            if (lines[i].Length > 1) //빈줄 무시
            {
                string[] commaSplited = lines[i].Split(',');
                Debug.Log("다음 줄");
                for (int j = 0; j < commaSplited.Length; j++)
                {
                    Debug.Log(commaSplited[j]);
                }
                if (int.Parse(commaSplited[chapterIndexInCSV]) == chapter
                        && int.Parse(commaSplited[stageIndexInCSV]) == stage)
                {
                    stageDataStrings.Add(lines[i]);
                }
            }
        }
        return stageDataStrings.ToArray();
    }

    Wave ParseLines(string dataString)
    {
        string[] splitted = dataString.Split(',');
        Wave newWave = new Wave();
        newWave.spawnTime = float.Parse(splitted[spawnTimeIndexInCSV]);
        //newWave.maxEnemyCount = splitted[maxEnemyCountIndex];

        // csv 데이터 형식이 달라졌으면 여기서 변경
        for (int i = enemyDataStartIndexInCSV; i < splitted.Length; i += 2)
        {
            GameObject enemyPrefab = Resources.Load<GameObject>("EnemyPrefabs/" + splitted[i]);
            int enemyCount = int.Parse(splitted[i + 1]);
            newWave.enemyPrefabs = new GameObject[enemyCount];
            // MaxEnemy 값이 없으면 정상작동 안해서 임의로 넣어줌
            newWave.maxEnemyCount = 1000;
            for (int j = 0; j < enemyCount; j++)
            {
                newWave.enemyPrefabs[j] = enemyPrefab;
            }
        }

        return newWave;
    }



}
