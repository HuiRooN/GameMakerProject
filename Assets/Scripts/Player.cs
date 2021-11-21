using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public GameObject[] weapons;
    public bool[] haveWeapons;

    public GameObject[] grenades;
    public int haveGrenades;
    public GameObject grenadeObj;

    public Camera followCamera;

    public int ammo;
    public int hp;

    public int maxAmmo;
    public int maxHp;
    public int maxHaveGrenades;

    float hAxis;
    float vAxis;
    float dodgeCooltime;
    
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isAttackReady = true;
    bool isBorder;
    bool isDamage;

    bool dDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool aDown;
    bool gDown;
    bool rDown;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshes;

    GameObject nearObject;
    Weapon equipWeapon;
    int equipWeaponIndex;
    float attackDelay;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshes = GetComponentsInChildren<MeshRenderer>();

        dodgeCooltime = 5.0f;
        haveWeapons[0] = true;
        equipWeapon = weapons[0].GetComponent<Weapon>();
        equipWeapon.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Dodge();
        DodgeCooltime();
        Attack();
        Grenade();
        Reload();
        Interaction();
        Swap();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        dDown = Input.GetButtonDown("Jump");
        aDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            moveVec = dodgeVec;

        if (isSwap || isReload || !isAttackReady)
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

    void Dodge()
    {
        if(dDown && moveVec != Vector3.zero && dodgeCooltime >= 5.0f && !isSwap)
        {
            dodgeVec = moveVec;
            speed *= 2.0f;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
        dodgeCooltime = 0;
    }

    void DodgeCooltime()
    {
        if (isDodge == false)
            dodgeCooltime += Time.deltaTime;
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        if (equipWeapon.type == Weapon.Type.Range && equipWeapon.curAmmo <= 0)
            return;

        attackDelay += Time.deltaTime;
        isAttackReady = equipWeapon.attackSpeed < attackDelay;

        if(aDown && isAttackReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            attackDelay = 0;
        }
    }

    void Grenade()
    {
        if (haveGrenades == 0)
            return;

        if(gDown && !isReload && !isSwap)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHIt;
            if (Physics.Raycast(ray, out rayHIt, 100))
            {
                Vector3 nextVec = rayHIt.point - transform.position;
                nextVec.y = 7;

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                haveGrenades--;
                grenades[haveGrenades].SetActive(false);
            }
        }
    }

    void Reload()
    {
        if (equipWeapon == null)
            return;

        if (equipWeapon.type == Weapon.Type.Melee)
            return;

        if (ammo == 0)
            return;

        if (equipWeapon.curAmmo == equipWeapon.maxAmmo)
            return;

        if(rDown && !isDodge && !isSwap && isAttackReady)
        {
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 0.5f);
        }
    }

    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }
   
    void Swap()
    {
        if (sDown1 && (!haveWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!haveWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!haveWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = 0;
        if (sDown1)
            weaponIndex = 0;
        if (sDown2)
            weaponIndex = 1;
        if (sDown3)
            weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isDodge)
        {
            if(equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.5f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }
    
    void Interaction()
    {
        if(iDown && nearObject != null && !isDodge)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                haveWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
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
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch(item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.HP:
                    hp += item.value;
                    if (hp > maxHp)
                        hp = maxHp;
                    break;
                case Item.Type.Grenade:
                    grenades[haveGrenades].SetActive(true);
                    haveGrenades += item.value;
                    if (haveGrenades >= maxHaveGrenades)
                        haveGrenades = maxHaveGrenades;
                     
                    break;
            }
            Destroy(other.gameObject);
        }
        else if(other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                hp -= enemyBullet.damage;
                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);

                StartCoroutine(OnDamage());
            }
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
    
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }
}
