using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//k all
public enum WeaponState { SearchTarget, AttackToTarget }

public class ShootTower : MonoBehaviour,TowerInterFace
{
    private WeaponState weaponState = WeaponState.SearchTarget;
    BulletInfo bulletinfo = new BulletInfo();

    [Header("tower body points and bullet")]
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public Transform RotatingBody;
    [Header("tower info")]
    public float LV;
    public string type;
    public float bulletSpeed;
    public float bulletDamage ;
    public float attackRate ;
    public float attackRange ;
    
    [Header("variables for In-Game watch")]
    public Transform attackTarget = null;
    public GameObject SpawnPoint;
    public List<GameObject> enemyList;
    private Transform homeY;


    private bool lockOn = false;
    

    public void SetUp(TowerInfo towerinfo)
    {
        
        this.LV= towerinfo.LV;
        this.type = towerinfo.type;
        this.attackRate = towerinfo.attackRate;
        this.attackRange = towerinfo.attackRange;
        this.bulletSpeed = towerinfo.bulletSpeed;
        this.bulletDamage = towerinfo.bulletDamage;

        bulletinfo.bulletSpeed = towerinfo.bulletSpeed;
        bulletinfo.bulletDamage = towerinfo.bulletDamage;

        bulletinfo.bombRange = towerinfo.bombRange;

        bulletinfo.maxChainCount = towerinfo.maxChainCount;
        bulletinfo.chainRadius = towerinfo.chainRadius;

        bulletinfo.poisonDamage = towerinfo.poisonDamage;
        bulletinfo.poisonDuration = towerinfo.poisonDuration;
        bulletinfo.poisonRate = towerinfo.poisonRate;

        bulletinfo.mujeockTime = towerinfo.mujeockTime;
        bulletinfo.stunDuration = towerinfo.stunDuration;
       

        

    }

    public void ChangeState(WeaponState newState) //적에 대한  탐색, 공격  모드의 코루틴 전환
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }





    private void RotateToTarget() //적을 바라봄
    {
        if (attackTarget)
        {

            Vector3 dir = attackTarget.transform.position - RotatingBody.transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            RotatingBody.transform.rotation = Quaternion.Slerp(RotatingBody.transform.rotation, rot, 3f * Time.deltaTime);
        }
    }

    
    private IEnumerator SearchTarget() //적 탐색
    {
        while (true)
        {
            

            float closestDistSqr = Mathf.Infinity;
            for (int i = 0; i < enemyList.Count; ++i)
            {
                if (enemyList[i] == null)
                    continue;
                if (BulletPrefab.tag == "BombBullet" && enemyList[i].tag == "FlyingEnemy")
                    continue;
                if (enemyList[i].GetComponent<EnemyInterFace>().CheckDead())
                    continue;

                float distance = Vector3.Distance(enemyList[i].transform.position, transform.position);
                if (distance <= attackRange && distance < closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemyList[i].transform;
                }
            }

            if (attackTarget != null && !attackTarget.GetComponent<EnemyInterFace>().CheckDead())
            {
                lockOn = true;
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }

    private IEnumerator AttackToTarget() //적 공격
    {
        yield return new WaitForSeconds(1.25f);
        while (true)
        {

            

            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                lockOn = false;
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

           
            
            SpawnBullet();
            yield return new WaitForSeconds(attackRate);
            

            

        }
    }


    private void SpawnBullet() //발사체 생성
    {
        bulletinfo.attackTarget = this.attackTarget;
        GameObject clone = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
        BulletInterFace bullet = clone.GetComponent<BulletInterFace>();
        bullet.SetUp(bulletinfo);
    }

  

    void CheckTarget()
    {

        if (!attackTarget || attackTarget.GetComponent<EnemyInterFace>().CheckDead())
        {
            lockOn = false;
            attackTarget = null;
            ChangeState(WeaponState.SearchTarget);
            return;
        }

    }




    void Start()
    {
       
        SpawnPoint = GameObject.Find("SpawnPointGroup");
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList;
       
    }

    private void OnEnable()
    {
        ChangeState(WeaponState.SearchTarget);
    }



    // Update is called once per frame
   
    void Update()
    {
        CheckTarget();
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList; //매 프레임마다 적 리스트 갱신
        if (lockOn)
            RotateToTarget();

    }

}
