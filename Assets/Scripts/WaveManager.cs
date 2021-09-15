using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Wave
{

    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private EnemyManager enemySpawner;
    [SerializeField]
    private ObstacleManager obstacleManager;
    public Player player;
    private int currentWaveIndex = -1;
    private static bool isWaveProceeding = false;
    public bool allWaveClear = false;

    public WaveData[] waveData;
    public bool isInGame = false;

    [SerializeField] GameObject ShopUI;
    [SerializeField] GameObject ClearUI;
    [SerializeField] TowerShop _towerShop;
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] GameObject _waveText;
    [SerializeField] GameObject _Heal;
    // Start is called before the first frame update

    public void StartWave()
    {
        if (enemySpawner.CurrentEnemyList.Count == 0 && currentWaveIndex < waves.Length - 1) //���̺� ����
        {
            isInGame = true;
            currentWaveIndex++;
            obstacleManager.WayObstacleActiveSwitch();
            enemySpawner.StartWave(waves[currentWaveIndex]);
            isWaveProceeding = true;
        }
    }

    public bool isWaveClear()
    {

        if (enemySpawner.enemySpawnCount == waves[currentWaveIndex].maxEnemyCount
            && player.currentHP > 0
            && enemySpawner.enemyKilledCount >= waves[currentWaveIndex].maxEnemyCount
            && enemySpawner.CurrentEnemyList.Count == 0)
        {
            isInGame = false;
            return true;
        }

        else
        {
            return false;
        }

    }

    public void MidTermReward()
    {
        Debug.LogWarning("WaveDone");//�� ���̺� �ϼ� �� ����
        isWaveProceeding = false;
        _towerShop.MakeShoppingList();
        moneyManager.AddMoney(100);
        if (!ShopUI.activeInHierarchy)
            ShopUI.SetActive(true);
    }

    public void FinalReward()
    {
        if (_Heal != null)
            _Heal.GetComponent<Heal>()._usage = 1;
        ClearUI.SetActive(true);
        allWaveClear = true; //��ü ���̺� �ϼ� �� ����ȭ�� �̵� , allWaveDone ��  GameManager �� ����� ����
    }

    private void Start()
    {
        if (GameObject.Find("Heal"))
            _Heal = GameObject.Find("Heal");
        Array.Resize(ref waves, waveData.Length);
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].spawnTime = waveData[i].SpawnTime;
            waves[i].maxEnemyCount = waveData[i].MaxEnemyCount;
            waves[i].enemyPrefabs = waveData[i].EnemyPrefabs;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _waveText.GetComponent<Text>().text = (currentWaveIndex + 1).ToString() + "/" + waves.Length;
        if (Input.GetKeyDown(KeyCode.S)) StartWave(); //���̺� ��ŸƮ

        if (currentWaveIndex != -1)
            if (isInGame && isWaveClear())
                switch (currentWaveIndex == waves.Length - 1)
                {
                    case false:
                        MidTermReward();
                        // StartWave();
                        // 바로 다음 웨이브 시작 x
                        break;
                    case true:
                        FinalReward();
                        break;
                }

    }

    public void setWaves(Wave[] waveData)
    {
        this.waves = waveData;
    }

    public static bool isWaveOn()
    {
        return isWaveProceeding;
    }
}

