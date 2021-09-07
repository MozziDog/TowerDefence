using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : MonoBehaviour
{
    public float LV;
    public float poisonDamage;
    public float poisonDuration;
    public float poisonRate;
   

    public float currentPoisonDuration;
    private PoisonDebuff thisDebuff;

    public GameObject PoisonFx;
    public GameObject effect;
    public GameObject thisEnemy;
  
    public void SetUp(float LV,float poisonDamage, float poisonDuration,float poisonRate ,GameObject PoisonFx)
    {

        this.LV = LV;
        this.poisonDamage = poisonDamage;
        this.poisonDuration = poisonDuration;
        this.poisonRate = poisonRate;
        this.PoisonFx = PoisonFx;

        currentPoisonDuration = poisonDuration;
    }

    public void ExecuteDebuff()
    {
        
            StartCoroutine(PoisonCoroutine());
       
        
    }

    IEnumerator PoisonCoroutine()
    {
        currentPoisonDuration = poisonDuration;
        while (currentPoisonDuration > 0)
        {
            if (currentPoisonDuration < poisonRate) break;
            if (thisEnemy.GetComponent<EnemyInterFace>().CheckDead()) break;
                yield return new WaitForSeconds(poisonRate);
            currentPoisonDuration -= poisonRate;
            thisEnemy.GetComponent<EnemyInterFace>().GetDamage(poisonDamage);
            effect = Instantiate(PoisonFx, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            effect.transform.SetParent(this.gameObject.transform);
            Destroy(effect, 1f);

        }
        EraseDebuff();


    }

    public void EraseDebuff()
    {
        //if (effect)
        //Destroy(effect);
        Destroy(thisDebuff);
    }

    public void RefreshDuration(float newPoisonDuration,float newPoisonDamage,float newPoisonRate)
    {
        this.currentPoisonDuration = newPoisonDuration;
        this.poisonDamage = newPoisonDamage;
        this.poisonRate = newPoisonRate;
        /*
        StopCoroutine(PoisonCoroutine());
        StartCoroutine(PoisonCoroutine());
        */
    }

    void OnEnable()
    {
        thisEnemy = this.gameObject;
        thisDebuff = this.gameObject.GetComponent<PoisonDebuff>();
        //SetUp(poisonDamage, poisonDuration, poisonRate, PoisonFx);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (thisEnemy.GetComponent<EnemyInterFace>().CheckDead())
         EraseDebuff();
    }
}

