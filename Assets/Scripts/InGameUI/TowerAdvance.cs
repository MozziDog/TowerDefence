
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct TowerUpgradeList
{
    public string TYPE;
    public GameObject[] UpgradeList;
}
public enum AlphabetTypes
{
    F, I, L, N, P, T, U, V, W, X, Y, Z
}

//[ExecuteInEditMode]
public class TowerAdvance : MonoBehaviour
{
    //for AdvanceList SetUp
    public TowerSelectedUI _selectUI;
    public Inventory _inven;
    public Button _advanceButton;
    public TowerManager _towerManager;
    public SelectManager _selectManager;
    public Text UpgradeMessagePopUp;
    public float _numberOfTypes = 12;
    public int _numberOfLevels = 3;
    public List<TowerUpgradeList> _towerUpgradeLists = new List<TowerUpgradeList>();
    private GameObject[] tempContainer;

    //for compare
    private List<int> ingredientindex = new List<int>();
    private string compareType;
    private float compareLV;
    private bool canAdvance;

    public GameObject targetTowerAd;
    public GameObject AdvancedTower;

    private void SetUp()
    {
        for (int i = 0; i < _numberOfTypes; i++)
        {
            TowerUpgradeList container = new TowerUpgradeList();
            AlphabetTypes tempType = (AlphabetTypes)i;
            container.TYPE = tempType.ToString();
            tempContainer = new GameObject[_numberOfLevels-1];
            container.UpgradeList = tempContainer;
            _towerUpgradeLists.Add(container);


        }
    }

    public void OnClickNotInteractiveUpgrade()
    {
        if (!_advanceButton.interactable)
        {
            UpgradeMessagePopUp.text = "There's no Enough Ingredients to Upgrade!";
            StartCoroutine(AdvanceMsgPopUp());
            
        }
        if (_advanceButton.interactable)
        {
            UpgradeMessagePopUp.text = "You've Upgrade!";
            StartCoroutine(AdvanceMsgPopUp());
        }
    }

    IEnumerator AdvanceMsgPopUp()
    {
        UpgradeMessagePopUp.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        UpgradeMessagePopUp.transform.parent.gameObject.SetActive(false);
    }

    private void Reset()
    {
        ingredientindex.Clear();

    }

    public void SetAdvanceTarget(GameObject tower)
    {
        this.targetTowerAd = tower;
    }

    public void CheckAdvance()
    {
        
        compareLV = this.targetTowerAd.GetComponent<TowerBase>().LV;
        if (compareLV == _numberOfLevels)
        {
            _advanceButton.interactable = false;
            return;
        }
        compareType = this.targetTowerAd.GetComponent<TowerBase>().type;
        
        for (int i = 0; i < _inven._toggle.Count; i++)
        {
            _inven._toggle[i].interactable = false;
            if (compareType != _inven._tower[i].GetComponent<TowerBase>().type)
            {
                continue;
            }
           
            ingredientindex.Add(i);
        }

       

        if(_advanceButton.interactable = ingredientindex.Count > 0 ? true : false)
        {
            _inven._toggle[ingredientindex[0]].interactable = true;
            _inven._toggle[ingredientindex[0]].Select();
        }



    }

    public void DoAdvance()
    {

        _inven.DestoryToggle(ingredientindex[0]);
        _inven.DeleteSelectedTower(ingredientindex[0]);
        /*
        for (int i = 0; i < _inven._toggle.Count; i++)
        {
            ColorBlock cb = _inven._toggle[i].colors;
            cb.normalColor = Color.white;
        }
        */
        // _inven.DestoryToggle(ingredientindex[1]-1);
        //_inven.DeleteSelectedTower(ingredientindex[1]-1);
        Vector3 replacePos = this.targetTowerAd.transform.position;
        Quaternion replaceRot = this.targetTowerAd.transform.rotation;
        _towerManager.towerSpawned.Remove(this.targetTowerAd);
        Destroy(this.targetTowerAd);
        AlphabetTypes tempType = (AlphabetTypes)System.Enum.Parse(typeof(AlphabetTypes), compareType);
        AdvancedTower = Instantiate(_towerUpgradeLists[(int)tempType].UpgradeList[(int)compareLV - 1], replacePos, replaceRot);
        _towerManager.towerSpawned.Add(AdvancedTower);
        AdvancedTower.GetComponent<TowerBase>().ConfirmTowerPosition();
        _towerManager.OnTowerSelected( AdvancedTower);
        _selectManager.SetSelectedTower( AdvancedTower);
        Reset();
        CheckAdvance();


    }

    private void OnEnable()
    {
       // SetUp();
        Reset();
       _selectUI.CheckCanAdvance();

  
    }
   

    private void OnDisable()
    {
        for (int i = 0; i < _inven._toggle.Count; i++)
        {
            _inven._toggle[i].interactable = true;
        }
        
       
    }


    // Update is called once per frame
    void Update()
    {

    }
}
