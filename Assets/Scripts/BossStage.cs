using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStage : MonoBehaviour
{
    public Player player;
    public GameObject Boss;
    public Transform bossPos;


    public bool bossInStage;

    public bool playerInStage;
    
    // Start is called before the first frame update
    void Start()
    {
        bossInStage = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInStage = true;
            if (!bossInStage)
                BossSpawn();
        }
    }

    void BossSpawn()
    {
        if (playerInStage)
        {
            GameObject instantBoss = Instantiate(Boss, bossPos.position, bossPos.rotation);
            Boss boss = instantBoss.GetComponent<Boss>();
            boss.target = player.transform;

            bossInStage = true;
        }
    }

}
