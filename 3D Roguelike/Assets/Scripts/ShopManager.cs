using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int Sell_Hp;
    public int Sell_ATTspd;
    public int Sell_MoveSpd;
    public int Sell_Dmg;

    public Text Look_HpLevel;
    public Text Look_ATTspdLevel;
    public Text Look_MoveSpdLevel;
    public Text Look_DmgLevel;

    public Text Hp_Text;
    public Text ATT_Text;
    public Text SPD_Text;
    public Text ATS_Text;
    public Text Coin_Text;

    public int isLevel_HP;
    public int isLevel_ATT;
    public int isLevel_ATS;
    public int isLevel_SPD;

    public float Hp_value = 0;
    public float ATT_value = 0;
    public float ATS_value = 0f;
    public float SPD_value = 0;

    void Start()
    {
        
    }

    void Update()
    {
        isLevel_HP = DataController.instance.playerData.Hp_level;
        isLevel_ATT = DataController.instance.playerData.ATT_level;
        isLevel_ATS = DataController.instance.playerData.ATS_level;
        isLevel_SPD = DataController.instance.playerData.SPD_level;

        Sell_Hp = 200 * isLevel_HP;
        Sell_ATTspd = 200 * isLevel_ATS;
        Sell_MoveSpd = 200 * isLevel_SPD;  
        Sell_Dmg = 200 * isLevel_ATT;

        Look_HpLevel.text = string.Format("체력 {0} -> {1}", DataController.instance.playerData.MaxHp, DataController.instance.playerData.MaxHp + Hp_value);
        Look_ATTspdLevel.text = string.Format("공격속도 {0:F1} -> {1:F1}",DataController.instance.playerData.GunSpeed, DataController.instance.playerData.GunSpeed - ATS_value);
        Look_MoveSpdLevel.text = string.Format("이동속도 {0:F1} -> {1:F1}", DataController.instance.playerData.PlayerSpeed, DataController.instance.playerData.PlayerSpeed + SPD_value);
        Look_DmgLevel.text = string.Format("공격력 {0} -> {1}",DataController.instance.playerData.Bullet_Damage,DataController.instance.playerData.Bullet_Damage + ATT_value);

        Hp_Text.text = string.Format("구매\n{0}원", Sell_Hp);
        ATT_Text.text = string.Format("구매\n{0}원", Sell_Dmg);
        SPD_Text.text = string.Format("구매\n{0}원", Sell_MoveSpd);
        ATS_Text.text = string.Format("구매\n{0}원", Sell_ATTspd);
        Coin_Text.text = string.Format("Coin : {0}원", DataController.instance.playerData.Coin);
    }

    public bool Check(int sellvalue)
    {
        return DataController.instance.playerData.Coin >= sellvalue ? true : false;
    }
    public void AfterCoin(int Price)
    {
        DataController.instance.playerData.Coin -= Price;
    }
    public void BuyButton(int Select)
    {
        switch (Select)
        {
            case 0:
                if (Check(Sell_Hp))
                {
                    if(DataController.instance.playerData.Hp_level < 10)
                    {
                        AfterCoin(Sell_Hp);
                        DataController.instance.playerData.MaxHp += Hp_value;
                        DataController.instance.playerData.Hp_level++;
                    }                   
                }
                break;
            case 1:
                if (Check(Sell_Dmg))
                {
                    if(DataController.instance.playerData.ATT_level < 10)
                    {
                        AfterCoin(Sell_Dmg);
                        DataController.instance.playerData.Bullet_Damage += ATT_value;
                        DataController.instance.playerData.ATT_level++;
                    }
                }
                break;
            case 2:
                if (Check(Sell_ATTspd))
                {
                    if(DataController.instance.playerData.ATS_level < 10)
                    {
                        AfterCoin(Sell_ATTspd);
                        DataController.instance.playerData.GunSpeed -= ATS_value;
                        DataController.instance.playerData.ATS_level++;
                    }                   
                }
                break;
            case 3:
                if (Check(Sell_MoveSpd))
                {
                    if (DataController.instance.playerData.SPD_level < 10) 
                    {
                        AfterCoin(Sell_MoveSpd);
                        DataController.instance.playerData.PlayerSpeed += SPD_value;
                        DataController.instance.playerData.SPD_level++;
                    }
                }
                break;
        }
    }

}