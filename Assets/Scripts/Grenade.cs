using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;


    void Start()
    {
        StartCoroutine(Explosion());
        StartCoroutine(TutorialExplosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3.0f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 10, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        foreach(RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy>().HitGrenade(transform.position);
        }
        Destroy(gameObject, 5);
    }

    IEnumerator TutorialExplosion()
    {
        yield return new WaitForSeconds(3.0f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 10, Vector3.up, 0f, LayerMask.GetMask("TutorialEnemy"));
        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<TutorialEnemy>().HitGrenade(transform.position);
        }
        Destroy(gameObject, 5);
    }
}
