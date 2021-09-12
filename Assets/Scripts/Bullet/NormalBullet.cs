using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class NormalBullet : MonoBehaviour, BulletInterFace
{
    public float LV;
    public float bulletSpeed;
    public float bulletDamage;
    private Transform target;
    public GameObject impactParticle;
    public Vector3 aimPosition;
    
    RaycastHit hit;
    public void SetUp(BulletInfo bulletinfo)
    {
        this.LV = bulletinfo.LV;
        this.bulletSpeed = bulletinfo.bulletSpeed;
        this.target = bulletinfo.attackTarget;
        this.bulletDamage = bulletinfo.bulletDamage;
    }

    private void OnTriggerEnter(Collider other) //적과 충돌시 상호작용
    {
        if(other.transform != target) return;
        if ( other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (other.gameObject.GetComponent<EnemyInterFace>() == null) return;
        other.GetComponent<EnemyInterFace>().GetDamage(bulletDamage);
        

        //hit particle spawn
        GameObject clone = Instantiate(impactParticle, target.transform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
        clone.transform.parent = target.transform;
        Destroy(clone, 2f);

        //destroy bullet prefab
        Destroy(gameObject,0.25f);
         
        
    }

    void Shoot()
    {
        //조준방향으로 발사..추적기능 on 
        aimPosition = new Vector3(target.position.x, target.position.y + 0.5f, target.position.z); 
        Physics.Raycast(transform.position, aimPosition, out hit);
        transform.LookAt(aimPosition);
        this.transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, bulletSpeed * Time.deltaTime);
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            
            Shoot();
            
        } 
        else
        {
            Destroy(gameObject); //적 소멸시 자체 파괴
        }
    }
}
