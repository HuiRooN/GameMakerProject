using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject BigFireBall;
    public Transform FireBallPos;

    Vector3 lookVec;
    Vector3 jumpVec;
    public bool isLook;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshes = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Pattern());
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Player>().transform;

        if(isDead)
        {
            StopAllCoroutines();
            return;
        }

        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 3.5f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(jumpVec);
    }

    IEnumerator Pattern()
    {
        yield return new WaitForSeconds(0.5f);

        int ranPattern = Random.Range(0, 5);
        switch(ranPattern)
        {
            case 0:
                StartCoroutine(JumpAttack());
                break;
            case 1:
                StartCoroutine(BigFireBallShot());
                break;
            case 2:
                StartCoroutine(FireBallShot());
                break;
            case 3:
                StartCoroutine(BigFireBallShot());
                break;
            case 4:
                StartCoroutine(FireBallShot());
                break;
        }
    }

    IEnumerator FireBallShot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantFireball = Instantiate(bullet, FireBallPos.position, FireBallPos.rotation);

        yield return new WaitForSeconds(0.3f);
        GameObject instantFireball2 = Instantiate(bullet, FireBallPos.position, FireBallPos.rotation);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Pattern());
    }

    IEnumerator BigFireBallShot()
    {
        isLook = false;
        anim.SetTrigger("doBigball");
        Instantiate(BigFireBall, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true;
        StartCoroutine(Pattern());
    }

    IEnumerator JumpAttack()
    {
        jumpVec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doJump");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);

        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;

        StartCoroutine(Pattern());
    }
}
