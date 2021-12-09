using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public bool playerInStage;

    public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        playerInStage = false;
        enemyCount = 0;
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
        }
    }
}
