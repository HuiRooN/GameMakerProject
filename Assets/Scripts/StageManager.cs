using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance  
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("StageManager");
                    instance = instanceContainer.AddComponent<StageManager>();
                }
            }
            return instance;
        }
    }
    private static StageManager instance;

    public GameObject Player;

    [System.Serializable]
    public class StartPositionArray
    {
        public List<Transform> StartPosition = new List<Transform>();
    }

    public StartPositionArray[] startPositionArrays;
    //startPositionArrays[0] 1~10 Stage
    //startPositionArrays[1] 11~20 Stage

    public List<Transform> StartPositionStore = new List<Transform>();

    public Transform StartPositionLastBoss;

    public int currentStage;
    int LastStage;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        currentStage = 1;
        LastStage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStage()
    {
        currentStage++;

        if(currentStage == LastStage)
        {
            return;
        }

        if(currentStage % 5 != 0)
        {
            int arrayIndex = currentStage / 10;
            int randomIndex = Random.Range(0, startPositionArrays[arrayIndex].StartPosition.Count);
            Player.transform.position = startPositionArrays[arrayIndex].StartPosition[randomIndex].position;
            startPositionArrays[arrayIndex].StartPosition.RemoveAt(randomIndex);
        }

        else
        {
            if (currentStage % 10 == 5)
            {
                int randomIndex = Random.Range(0, StartPositionStore.Count);
                Player.transform.position = StartPositionStore[randomIndex].position;
            }
            else
            {
                if (currentStage == LastStage)
                {
                    Player.transform.position = StartPositionLastBoss.position;
                }
            }
        }
    }
}
