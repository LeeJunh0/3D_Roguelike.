using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Spawner SpawnerScript;
    public PlayerController PlayerScript;
    public Text AttackDamage;
    public Text AttackSpeed;
    public Text Penetration;
    public Text Health;

    public GameObject InGame_UI;
    public Text InGameTime_Text;
    public Text ScoreTime_Text;
    public float PlayTime;
    public float Sec;
    public int Min;

    public Slider HPbar;
    public Slider ExpBar;

    public GameObject Die_UI;
    public Text DieCoin_text;

    public GameObject TabMenu_UI;
    public Text Damage_TabText;
    public Text ATTSpd_TabText;
    public Text Penetration_TabText;
    public Text Boom_TabText;

    public GameObject Danger_UI;

    public GameObject Clear_UI;
    public Text ClearScore_Text;
    public Text ClearCoin_text;

    public GameObject Select_UI;
    public Text[] Skill_LevelText;
    public Text[] Skill_Text;
    public Image[] Skill_Icon;
    public Button[] Select_Buttons;
    public Text BossText;

    private void Start()
    {
        InGame_UI.SetActive(true);
        Clear_UI.SetActive(false);
        TabMenu_UI.SetActive(false);
        Die_UI.SetActive(false);
        BossText.color = new Color(255, 255, 255, 0);
    }
    void Update()
    {
        if (PlayerScript.curHp > 0)
        {
            if(TabMenu_UI.activeSelf == false)
            {
                Time.timeScale = 1;
            }
            PlayTime += Time.deltaTime;
            Die_UI.SetActive(false);
            Min = (int)(PlayTime / 60);
            Sec = (int)(PlayTime % 60);

            InGameTime_Text.text = string.Format(string.Format("{0:00}", Min) + ":" + string.Format("{0:00}", Sec));
            ScoreTime_Text.text = string.Format("�ְ���� : {0:00}", Min) + " : " + string.Format("{0:00}", Sec);
            ClearScore_Text.text = ScoreTime_Text.text;           
            AttackDamage.text = "���ݷ�+: " + PlayerScript.Power_Level + " ����";
            AttackSpeed.text = "���ݼӵ�+: " + PlayerScript.Speed_Level + " ����";
            Penetration.text = "�����+: " + PlayerScript.Penetration_Level + " ����";
            Health.text = "ü��+: " + PlayerScript.Health_Level + " ����";

            //�����̴�
            ExpBar.value = PlayerScript.curExp / PlayerScript.MaxExp;
            HPbar.value = PlayerScript.curHp / PlayerScript.MaxHp;

            //�Ǹ޴�
            Damage_TabText.text = string.Format("������ : {0}", GameManager.gameManager.BulletDamage);
            ATTSpd_TabText.text = string.Format("���ݼӵ� : {0:F2}", GameManager.gameManager.Gunreload);
            Penetration_TabText.text = string.Format("����� : {0}��", GameManager.gameManager.Penetration);
            Boom_TabText.text = string.Format("ü�� : {0:F1}", GameManager.gameManager.playerMaxHP);
        }
        else
        {
            Time.timeScale = 0;
            Die_UI.SetActive(true);
            BossText.color = new Color(1, 1, 1, 0);
            DieCoin_text.text = string.Format("ȹ�� Coin : {0}", (int)(GameManager.gameManager.KillCount + (PlayTime / 2)));
            DataController.instance.playerData.MaxScore = ScoreTime_Text.text;
            DataController.instance.playerData.Coin += (int)(GameManager.gameManager.KillCount + (PlayTime / 2));
            DataController.instance.SaveData();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && TabMenu_UI.activeSelf != true)
        {
            Time.timeScale = 0;
            TabMenu_UI.SetActive(true);            
        } 
        
        if(Select_UI.activeSelf == true)
        {
            Time.timeScale = 0;
        }

        if(PlayTime >= GameManager.gameManager.wavemanager.BossTime)
        {
            if (BossText.color.a < 1f)
                BossText.color = new Color(255, 255, 255, BossText.color.a + Time.deltaTime);
        }

        if (GameManager.gameManager.wavemanager.Boss.activeSelf == true)
        {
            Enermy boss = GameManager.gameManager.wavemanager.Boss.GetComponent<Enermy>();
            if (boss.curHp <= 0)
            {
                Time.timeScale = 0;
                ClearCoin_text.text = string.Format("ȹ�� Coin : {0}", (int)(GameManager.gameManager.KillCount + (PlayTime / 2)));
                BossText.color = new Color(1, 1, 1, 0);
                Clear_UI.SetActive(true);
            }
        }
    }
    public Queue<int> Set_Select()
    {
        List<int> Case = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
        Queue<int> que = new Queue<int>();

        for(int i = 0; i < 3; i++)
        {
            int cur = Random.Range(0, Case.Count);
            que.Enqueue(Case[cur]);
            Case.RemoveAt(cur);
        }
        return que;
    }
    public void SelectUI_Init()
    {
        Select_UI.SetActive(true);

        Queue<int> Skillque = Set_Select();
        
        for (int i = 0; i < 3; i++)
        {
            int CurSkillIndex = Skillque.Dequeue();
            Skill_Icon[i].sprite = SkillManager.skillManager.Skill_Inventory[CurSkillIndex].SkillIcon;
            if (SkillManager.skillManager.Skill_Inventory[CurSkillIndex].Level < 5)
            {
                Skill_LevelText[i].text = string.Format(
                                                      "���緹�� {0} -> {1}", SkillManager.skillManager.Skill_Inventory[CurSkillIndex].Level,
                                                      SkillManager.skillManager.Skill_Inventory[CurSkillIndex].Level + 1);

                Skill_Text[i].text = SkillManager.skillManager.Skill_Inventory[CurSkillIndex].
                                                 Skill_Plan[SkillManager.skillManager.Skill_Inventory[CurSkillIndex].Level];
            }
            else
            {
                Skill_LevelText[i].text = string.Format("���緹�� Max");
                Skill_Text[i].text = string.Format("���緹�� Max, �÷��� ȿ�� ������ �����ϴ�.");
            }
            Select_Buttons[i].onClick.AddListener(() => SkillManager.skillManager.Skill_LevelUp(CurSkillIndex));
            Select_Buttons[i].onClick.AddListener(() => SelectUI_Off());
        }
        
    }
    public void SelectUI_Off() 
    {
        for (int i = 0; i < 3; i++)
        {
            Select_Buttons[i].onClick.RemoveAllListeners();
        }
        Select_UI.SetActive(false);
    }
    public void GameOverButton()
    {
        SceneManager.LoadScene("StartMain");
    }
    public void MainButton()
    {
        DataController.instance.playerData.Coin += (int)(GameManager.gameManager.KillCount + (PlayTime / 2));
        DataController.instance.SaveData();
        SceneManager.LoadScene("StartMain");
    }
    
    public void Continue_Button()
    {
        Time.timeScale = 1;
        TabMenu_UI.SetActive(false);
    }
    public void ReStart_Button()
    {
        MainManager main = new MainManager();
        main.StartButton();
    }
    public void DangerON()
    {
        Danger_UI.SetActive(true);
    }
    public void DangerOFF()
    {
        Danger_UI.SetActive(false);
    }
}
