using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameCam;
    public Player player;
    public int stage;
    public bool isFight;
    public int enemyACnt;
    public int enemyBCnt;
    public int enemyCCnt;

    public GameObject GamePanel;
    public GameObject ShopPanel;
    public GameObject FailPanel;

    public Text stageTxt;
    public Text playerHPTxt;
    public Text playerCoinTxt;

    public RectTransform bossHPGroup;
    public RectTransform bossHP;

    void Awake()
    {

    }

    void Start()
    {
        Time.timeScale = 1;
        FailPanel.SetActive(false);
    }

    void LateUpdate()
    {
        playerHPTxt.text = player.hp + "/" + player.maxHp;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        stageTxt.text =SceneManager.GetActiveScene().name + "-" + StageManager.Instance.currentStage;

        if(GameObject.FindWithTag("BossStage").GetComponent<BossStage>().bossInStage)
        {
            bossHPGroup.anchoredPosition = Vector3.left * 114;
            bossHPGroup.anchoredPosition = Vector3.down * 20;
            bossHP.localScale = new Vector3((float)GameObject.FindWithTag("Boss").GetComponent<Boss>().curHp / GameObject.FindWithTag("Boss").GetComponent<Boss>().maxHp, 1, 1);
        }
        else
        {
            bossHPGroup.anchoredPosition = Vector3.left * 114;
            bossHPGroup.anchoredPosition = Vector3.up * 200;
        }
        
    }

    public void TitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Stage1");
    }
}
