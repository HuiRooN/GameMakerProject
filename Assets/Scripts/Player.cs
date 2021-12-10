using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;

    public GameObject[] weapons;
    public bool[] haveWeapons;

    public Camera followCamera;
    public GameManager gamemanager;

    public GameObject FailPanel;

    public int hp;
    public int coin;

    public int maxHp;

    public int killEnemy;

    float hAxis;
    float vAxis;
    
    bool isAttackReady = true;
    bool isBorder;
    bool isDamage;

    bool aDown;
    bool iDown;

    Vector3 moveVec;

    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshes;

    GameObject nearObject;
    public Weapon equipWeapon;
    int equipWeaponIndex;
    float attackDelay;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshes = GetComponentsInChildren<MeshRenderer>();

        haveWeapons[0] = true;
        equipWeapon = weapons[0].GetComponent<Weapon>();
        equipWeapon.gameObject.SetActive(true);
    }

    void Start()
    {
        killEnemy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Attack();

        if(iDown)
        {
            StageManager.Instance.currentStage = 9;
        }

        //PlayerPrefs.SetInt("SetHP", hp);
        //PlayerPrefs.SetInt("SetCoin", coin);
        //PlayerPrefs.SetInt("SetAmmo", ammo);
        //PlayerPrefs.SetInt("SetGrenade", haveGrenades);
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        aDown = Input.GetButton("Fire1");
        iDown = Input.GetButton("Interaction");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (!isAttackReady)
            moveVec = Vector3.zero;

        if (!isBorder)
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);

        if (aDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHIt;
            if (Physics.Raycast(ray, out rayHIt, 100))
            {
                Vector3 nextVec = rayHIt.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        attackDelay += Time.deltaTime;
        isAttackReady = equipWeapon.attackSpeed < attackDelay;

        if(aDown && isAttackReady)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            attackDelay = 0;
        }
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 1, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 1, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            switch(item.type)
            {
                case Item.Type.HP:
                    hp += item.value;
                    if (hp > maxHp)
                        hp = maxHp;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("EnemyBullet"))
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                hp -= enemyBullet.damage;
                anim.SetTrigger("doDamage");
                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);

                if (hp <= 0)
                    GameOver();

                StartCoroutine(OnDamage());
            }
        }
        else if(other.CompareTag("goToNext"))
        {
            StageManager.Instance.NextStage();
            killEnemy = 0;
        }
    }

    IEnumerator OnDamage()
    {
        isDamage = true;
        foreach(MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(1f);

        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.white;
        }

        isDamage = false;

    }
    
    void GameOver()
    {
        Time.timeScale = 0;
        FailPanel.SetActive(true);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
            nearObject = other.gameObject;
        if (other.CompareTag("ShopStage"))
        {
            gamemanager.ShopPanel.SetActive(true);
            GameObject.FindWithTag("ShopStage").GetComponent<Stage>().Nextstage.SetActive(true);
        }
        if (other.CompareTag("Stage") || other.CompareTag("BossStage"))
            gamemanager.ShopPanel.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
            nearObject = null;
        if (other.CompareTag("ShopStage"))
            gamemanager.ShopPanel.SetActive(true);
    }
}
