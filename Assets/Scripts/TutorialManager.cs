using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject Tutorial1Panel;
    public GameObject HpPanel;
    public GameObject CoinPanel;
    public GameObject Tutorial2Panel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        Tutorial1Panel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch(item.type)
            {
                case Item.Type.HP:
                    HpPanel.SetActive(true);
                    Time.timeScale = 0;
                    break;
                case Item.Type.Coin:
                    CoinPanel.SetActive(true);
                    Time.timeScale = 0;
                    break;
            }
        }
        else if (other.tag == "Tutorial2Start")
        {
            Tutorial2Panel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Tutorial1Close()
    {
        Time.timeScale = 1;
        Tutorial1Panel.SetActive(false);
    }

    public void HpClose()
    {
        Time.timeScale = 1;
        HpPanel.SetActive(false);
    }

    public void CoinClose()
    {
        Time.timeScale = 1;
        CoinPanel.SetActive(false);
    }

    public void Tutorial2Close()
    {
        Time.timeScale = 1;
        Tutorial2Panel.SetActive(false);
    }

    public void TutorialSkip()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage1");
    }
}
