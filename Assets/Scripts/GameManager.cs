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

    public Text stageTxt;
    public Text playerHPTxt;
    public Text playerCoinTxt;

    public RectTransform bossHPGroup;
    public RectTransform bossHP;

    void Awake()
    {
        //player.hp = PlayerPrefs.GetInt("SetHP");
        //player.coin = PlayerPrefs.GetInt("SetCoin");
        //player.ammo = PlayerPrefs.GetInt("SetAmmo");
        //player.haveGrenades = PlayerPrefs.GetInt("SetGrenade");
    }

    void LateUpdate()
    {
        playerHPTxt.text = player.hp + "/" + player.maxHp;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        stageTxt.text =SceneManager.GetActiveScene().name + "-" + StageManager.Instance.currentStage;

        //bossHP.localScale = new Vector3(boss.curHP / boss.maxHP, 1, 1);
    }
}
