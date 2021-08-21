using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Tower;
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject _enemypoint;
    [SerializeField] GameObject _endpoint;

    [SerializeField] GameObject _floor;

    public float _scaleFactor = 1f;
    public NavMeshSurface surface;
    public NavMeshAgent agent;
    public NavMeshPath navMeshPath;
    Transform spawnPos;

    GameObject tower;
    public GameObject[] _selected;
    public GameObject _range;

    public bool pathAvailable;

    public void BakeNav()
    {
        surface.BuildNavMesh();
    }

    private void Awake()
    {
        SetResolution();
    }
    private void Start()
    {
        navMeshPath = new NavMeshPath();
    }

    private void Update()
    {
        CheckTile();
        _floor.GetComponent<Renderer>().material.SetFloat("_GridScaleFactor", _scaleFactor);
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
                {
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.tag != "Tower")
                    {
                        for (int i = 0; i < Tower.GetComponent<TowerInfo>()._myposition.Length; i++)
                        {
                            if (Physics.CheckSphere(Tower.GetComponent<TowerInfo>()._myposition[i], 0.4f))
                            {

                            }
                        }

                        tower = Instantiate(Tower, new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0.5f, Mathf.Floor(hit.point.z) + 0.5f), Quaternion.identity);
                        BakeNav();
                        
                    }
                }
            }
        }
    }

    private void CheckTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        /*if (CalculateNewPath() == true)
        {
            pathAvailable = true;
            print("Path available");
        }
        else
        {
            pathAvailable = false;
            print("Path not available");
        }*/
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Floor")
            {
                if(hit.transform.tag == "Tower")
                {
                    _selected[0].transform.position = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0f, Mathf.Floor(hit.point.z) + 0.5f);
                    _selected[0].SetActive(true);
                }
                else
                {
                    _selected[1].transform.position = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0f, Mathf.Floor(hit.point.z) + 0.5f);
                    _selected[1].SetActive(true);
                }
                    

                _range.transform.position = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0f, Mathf.Floor(hit.point.z) + 0.5f);
                _range.SetActive(true);
            }
            //else _bg.SetActive(false);
        }
    }

    public void StartSpawn()
    {
        _enemypoint.GetComponent<NavMeshAgent>().enabled = false;
        StartCoroutine(EnemySpawner());
    }

    IEnumerator EnemySpawner()
    {
        Instantiate(Enemy, _enemypoint.transform);
        yield return new WaitForSeconds(1f);
        StartCoroutine(EnemySpawner());
    }

    bool CalculateNewPath()
    {
        agent.CalculatePath(_endpoint.transform.position, navMeshPath);
        print("New path calculated");
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void SetResolution()
    {
        Camera cam = Camera.main;
        Rect rect = cam.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)18 / 9); // (���� / ����)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        cam.rect = rect;
    }

}
