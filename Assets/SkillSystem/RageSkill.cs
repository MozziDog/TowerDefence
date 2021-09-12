using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RageSkill : MonoBehaviour
{
    [SerializeField]
    GameObject RageEffect;
    private void Start()
    {
        Instantiate(RageEffect, transform);
        Destroy(gameObject, 3.1f);
        var hits = Physics.SphereCastAll(transform.position, 2f, Vector3.up, 0f);
        for(int i = 0; i < hits.Length; i++)
        {
            if(hits[i].transform.tag == "TowerBase" && !hits[i].transform.gameObject.GetComponent<TowerBase>().isBuffed)
            {
                Debug.Log(hits[i].transform.gameObject.name);
                float originAttackRate = hits[i].transform.gameObject.GetComponent<TowerBase>().attackRate;
                float originBulletDamage = hits[i].transform.gameObject.GetComponent<TowerBase>().bulletDamage;
                hits[i].transform.gameObject.GetComponent<TowerBase>().isBuffed = true;
                hits[i].transform.gameObject.GetComponent<TowerBase>().attackRate *= 0.9f;
                hits[i].transform.gameObject.GetComponent<TowerBase>().bulletDamage *= 1.1f;
                hits[i].transform.gameObject.GetComponent<TowerBase>().SetUp();
                for(int index = 0; index < hits[i].transform.childCount; index++)
                {
                    if (hits[i].transform.GetChild(index).GetComponent<NavMeshModifier>())
                    {
                        hits[i].transform.GetChild(index).GetComponent<MeshRenderer>().material.color = Color.red;
                        StartCoroutine(ColorChange(hits[i].transform.GetChild(index)));
                    }
                }
                StartCoroutine(BuffOff(hits[i].transform.gameObject, originAttackRate, originBulletDamage));
            }
        }
    }

    IEnumerator BuffOff(GameObject _tower, float AttackRate, float BulletDamage)
    {
        yield return new WaitForSeconds(3.1f);
        _tower.transform.gameObject.GetComponent<TowerBase>().isBuffed = false;
        _tower.transform.gameObject.GetComponent<TowerBase>().attackRate = AttackRate;
        _tower.transform.gameObject.GetComponent<TowerBase>().bulletDamage = BulletDamage;
        _tower.transform.gameObject.GetComponent<TowerBase>().SetUp();
        Destroy(this.gameObject);
    }

    IEnumerator ColorChange(Transform _gameObject)
    {
        yield return new WaitForSeconds(3f);
        _gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 3f);
    }
}
