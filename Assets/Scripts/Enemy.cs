using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Melee, Speed, Range, Stage1Boss};
    public Type enemyType;

    public int maxHp;
    public int curHp;
    public Transform target;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public GameObject Coin;
    public GameObject Health;


    public bool isChase;
    public bool isAttack;
    public bool isDead;
    
    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshes;
    public NavMeshAgent nav;
    public Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshes = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if(enemyType != Type.Stage1Boss)
            Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if(nav.enabled && enemyType != Type.Stage1Boss)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        if(!isDead && enemyType != Type.Stage1Boss)
        {
            float targetRadius = 0;
            float targetRange = 0f;

            switch (enemyType)
            {
                case Type.Melee:
                    targetRadius = 0.9f;
                    targetRange = 1.0f;
                    break;
                case Type.Speed:
                    targetRadius = 0.7f;
                    targetRange = 8.0f;
                    break;
                case Type.Range:
                    targetRadius = 0.3f;
                    targetRange = 15.0f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }  
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch(enemyType)
        {
            case Type.Melee:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);

                break;
            case Type.Speed:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(5f);

                break;
            case Type.Range:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;
        }

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Melee"))
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHp -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec, false));

            Debug.Log("Melee : " + curHp);
        }
        if(other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHp -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reactVec, false));

            Debug.Log("Range : " + curHp);
        }
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        if(curHp > 0)
        {
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.material.color = Color.white;
            }
        }
        else if(curHp <= 0)
        {
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.material.color = Color.gray;
            }

            gameObject.layer = 13;
            isDead = true;
            isChase = false;
            nav.enabled = false;
            if(enemyType == Type.Melee || enemyType == Type.Speed)
            {
                meleeArea.enabled = false;
            }
            anim.SetTrigger("doDie");

            GameObject.FindWithTag("Player").GetComponent<Player>().killEnemy++;

            int ranCoin = Random.Range(0, 1);
            if (ranCoin == 0)
                Instantiate(Coin, transform.position, Quaternion.identity);

            int ranHp = Random.Range(0, 4);
            if (ranHp == 0)
                Instantiate(Health, transform.position, Quaternion.identity);

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);

            if(enemyType != Type.Stage1Boss)
                Destroy(gameObject, 1);
            
        }
    }
}
