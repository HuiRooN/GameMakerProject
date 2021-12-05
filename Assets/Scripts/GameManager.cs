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

    public Text stageTxt;
    public Text playerHPTxt;
    public Text playerCoinTxt;
    public Text playerAmmoTxt;

    public Image Dodge;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;

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
        stageTxt.text = "" + SceneManager.GetActiveScene().name;

        if (player.equipWeapon == null)
            playerAmmoTxt.text = "-/" + player.ammo;
        else if(player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text = "-/" + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + "/" + player.ammo;

        Dodge.color = new Color(1, 1, 1, player.dodgeCooltime >= 5.0f ? 1 : 0);
        weapon1Img.color = new Color(1, 1, 1, player.haveWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.haveWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.haveWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.haveGrenades > 0 ? 1 : 0);

        //bossHP.localScale = new Vector3(boss.curHP / boss.maxHP, 1, 1);
    }
}
