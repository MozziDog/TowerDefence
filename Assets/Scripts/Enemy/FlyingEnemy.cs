using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour, EnemyInterFace
{

    [Header("Enemy Info")]
   
    public float maxHP;
    public float currentHP;
    public float moveSpeed;
    public float hitDamage;
    public Transform headPos;

    [Header("Enemy State")]
    public bool isDie = false;
    public bool isHit = false;
    private bool isWalking = true;


    [Header("Animator and EnemyManager")]
    public Animator anim;
    public GameObject enemyManager;


   
    GameObject target;
    GameObject Player;
    Vector3 targetPositionForAir;
    Vector3 tempPos;

    public void SetUp()
    {

    }

    public bool CheckDead()
    {
        return isDie;
    }


    public float GetSpeed()
    {
        return moveSpeed;
    }

    public void SetSpeed(float ApplySpeed)
    {
        moveSpeed = ApplySpeed;
    }

    public Transform GetHeadPos()
    {
        return headPos;
    }




    void Start()
    {
        currentHP = maxHP;
        target = GameObject.Find("EndPoint");
        Player = GameObject.Find("Player1");
        enemyManager = GameObject.Find("SpawnPointGroup");
        targetPositionForAir = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
    }





    private void Update()
    {
        

        if (isWalking)
            AirMove();

    }

  
    public void AirMove()
    {
        transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
        transform.position = Vector3.MoveTowards(this.transform.position, targetPositionForAir, moveSpeed * Time.deltaTime);

    }

    public void RemoveObject()
    {
        isDie = true;
        enemyManager.GetComponent<EnemyManager>().CurrentEnemyList.Remove(gameObject);
        enemyManager.GetComponent<EnemyManager>().SpawnedAirEnemyCount--;
        enemyManager.GetComponent<EnemyManager>().enemyKilledCount++;
        Destroy(gameObject);

    }

    public void GetDamage(float Damage) //k
    {
        currentHP -= Damage;
        if (currentHP <= 0)
        {
            ReadyToDie();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (isDie)
            return;
            if (other.gameObject.CompareTag("Player"))
        {

            isWalking = false;
            StartCoroutine(HitPlayer());
        }

    }

    IEnumerator HitPlayer()
    {
        

        anim.SetBool("ContactPlayer", true);
        yield return new WaitForSeconds(0.35f);
        Player.GetComponent<Player>().StartGetHit(hitDamage);
        yield return new WaitForSeconds(0.65f);
        RemoveObject();
    }

    public void ReadyToDie()
    {
        if (isHit)
            return;
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        anim.SetBool("isDead", true);
        isWalking = false;
        yield return new WaitForSeconds(1.07f);
        RemoveObject();
    }






}
