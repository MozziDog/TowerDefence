using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float moveSpeed;
    public Animator anim;
    public GameObject enemyManager;
    public float currentHP;
    private bool isWalking = true;
    public float hitDamage;
    GameObject target;
    GameObject Player;
    Vector3 targetPositionForAir;
    Vector3 tempPos;






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
        // AgentStuckAvoid();

        if (isWalking)
            AirMove();

    }

    /*
    public void AgentStuckAvoid()
    {
        if (isWalking && !agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.speed < 0.3)
        {
            Debug.LogWarning("enemy Repathing!!");
            agent.enabled = false;
            agent.enabled = true;
            agent.SetDestination(target.transform.position);
            agent.speed = moveSpeed;
        }
    }
    */
    public void AirMove()
    {
        transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
        transform.position = Vector3.MoveTowards(this.transform.position, targetPositionForAir, moveSpeed * Time.deltaTime);

    }

    public void RemoveObject()
    {
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
        if (other.gameObject.CompareTag("Player"))
        {

            isWalking = false;
            StartCoroutine(HitPlayer());
        }

    }

    IEnumerator HitPlayer()
    {

        anim.SetBool("ContactPlayer", true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Player.GetComponent<Player>().GetHitCoroutine(hitDamage));
        yield return new WaitForSeconds(0.75f);
        RemoveObject();
    }

    public void ReadyToDie()
    {
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        anim.SetBool("isDead", true);
        isWalking = false;
        yield return new WaitForSeconds(1.1f);
        RemoveObject();
    }






}
