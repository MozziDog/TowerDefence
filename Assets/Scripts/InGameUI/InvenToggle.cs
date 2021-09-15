using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenToggle : MonoBehaviour
{
    public GameObject towerPrefab;
    Toggle toggle;
    [SerializeField] Button infoButton;
    TowerInfoUI towerInfoUI;
    // Start is called before the first frame update
    void Start()
    {
        //infoButton = gameObject.GetComponentInChildren<Button>();
        toggle = gameObject.GetComponent<Toggle>();
        // towerInfoUI = GameObject.Find("TowerInfoUI")?.GetComponent<TowerInfoUI>();
        infoButton.onClick.AddListener(OnClickInfoButton);
        toggle.onValueChanged.AddListener(OnToggleSelected);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClickInfoButton()
    {
        GameObject infoUI = GameObject.Find("TowerInfoWindow");
        Debug.Log(infoUI);
        towerInfoUI = infoUI.GetComponentInChildren<TowerInfoUI>();
        Debug.Log(towerInfoUI);
        towerInfoUI.SetTowerInfoUIContents(towerPrefab);
        towerInfoUI.OpenTowerInfoUI();
    }

    void OnToggleSelected(bool isSelected)
    {
        infoButton.gameObject.SetActive(isSelected);
    }
}
