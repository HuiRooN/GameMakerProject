using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Player player;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public GameObject Nextstage;

    public List<int> enemyList;

    public bool playerInStage;

    public int enemyCount;

    // Start is called before the first frame update

    void Awake()
    {
        enemyList = new List<int>();
    }
    void Start()
    {
        playerInStage = false;
        enemyCount = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.killEnemy == enemyCount)
            Nextstage.SetActive(true);
        else
            Nextstage.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInStage = true;
            StartCoroutine(EnemySpawn());
        }
    }

    IEnumerator EnemySpawn()
    {
        if (playerInStage)
        {
            if (enemyCount <= StageManager.Instance.currentStage)
                enemyCount = StageManager.Instance.currentStage;
            else
                enemyCount = 4;

            for (int index = 0; index < enemyCount; index++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);
            }

            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemyList.RemoveAt(0);

                yield return new WaitForSeconds(5f);
            }
        }            
    }
}
