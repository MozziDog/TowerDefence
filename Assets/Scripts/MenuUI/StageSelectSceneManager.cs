using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectSceneManager : MonoBehaviour
{
    public GameObject fader;
    public Text goldText;
    public Text diamondText;
    public Text keyText;

    public int MAX_STAGE;

    private int selectedStage = 0;


    // Start is called before the first frame update
    void Start()
    {
        LoadSaveData();
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadSaveData()
    {
        // TODO: Save & Load 구현
        Global.property_gold = 1000;
        Global.property_diamond = 0;
        Global.property_key = 5;
        RefreshUserPropertyData();
    }

    void RefreshUserPropertyData()
    {
        goldText.text = Global.property_gold.ToString();
        diamondText.text = Global.property_diamond.ToString();
        keyText.text = Global.property_key.ToString();
    }

    Coroutine fadeInCoroutine = null;
    public void FadeIn()
    {
        fader.SetActive(true);
        IEnumerator _Fade()
        {
            for (int i = 5; i >= 0; i--)
            {
                fader.GetComponent<Image>().color = new Color(0, 0, 0, i / 5f);
                yield return new WaitForSeconds(0.05f);
            }
            fadeInCoroutine = null;
            fader.SetActive(false);
        }
        if (fadeInCoroutine != null)
            StopCoroutine(fadeInCoroutine);
        fadeInCoroutine = StartCoroutine(_Fade());
    }

    Coroutine fadeOutCoroutine = null;
    public void FadeOut()
    {
        fader.SetActive(true);
        IEnumerator _Fade()
        {
            for (int i = 0; i < 6; i++)
            {
                fader.GetComponent<Image>().color = new Color(0, 0, 0, i / 5f);
                yield return new WaitForSeconds(0.05f);
            }
            fadeOutCoroutine = null;
        }
        if (fadeOutCoroutine != null)
            StopCoroutine(fadeInCoroutine);
        fadeOutCoroutine = StartCoroutine(_Fade());
    }

    public void StartStage()
    {
        int stageCode = GetSelectedStage();
        //SceneManager.LoadScene("Stage" + stageCode);
        SceneManager.LoadScene("SampleScene");
    }

    int GetSelectedStage()
    {
        // TODO: GetSelectedStage 구현
        return selectedStage;
    }

    public void ChangeSelectedStage_Next()
    {
        // TODO: ChangeSelectedStage_Next 구현
        if (selectedStage < MAX_STAGE)
        {
            selectedStage++;
            // 다음 스테이지 선택된 연출
            Debug.Log("다음 스테이지 선택");
        }
        else
        {
            Debug.Log("마지막 스테이지입니다!");
        }
    }

    public void ChangeSelectedStage_Before()
    {
        // TODO: ChangeSelectedStage_Before 구현
        if (selectedStage > 0)
        {
            selectedStage--;
            // 이전 스테이지 선택된 연출
            Debug.Log("이전 스테이지 선택");
        }
        else
        {
            Debug.Log("첫 스테이지입니다!");
        }
    }
}