using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager skillManager = null;
    public List<Skill> Skill_Inventory;

    public GameObject Shield_Prefab;
    public GameObject Field_Prefab;
    public GameObject Crow_Prefab;

    // 스킬 - 실드관련
    public float radius = 2;
    public float ShieldSpeed = 200;
    public int ShieldLevel = 0;
    public int ShieldHitCount = 3;
    public float regenerateTime = 3f;
    //스킬 - 필드관련
    public int FieldLevel = 5;
    public float DamageTime = 0f;
    //스킬 - 까마귀관련
    public int CrowLevel = 0;
    public float AttackDelay = 1.5f;
    public float CrowRange = 8f;
    public float HowlingRange = 2.65f;
    //패시브 스킬관련
    public int HealthLevel = 0;
    public int ATTLevel = 0;
    public int SPDLevel = 0;
    public int PenetrationLevel = 0;
    private void Awake()
    {
        if (skillManager == null)
            skillManager = this;
        else
            Destroy(this);
    }
    void Start()
    {
        Shield_Prefab = Resources.Load<GameObject>("prefab/Skill/Shield_Prefab");
        Field_Prefab = Resources.Load<GameObject>("prefab/Skill/Shield_Prefab");

        DamageTime = 1.5f;
    }

    void Update()
    {
        Skill_Init();
    }
    public void Skill_LevelUp(int type)
    {
        Debug.Log("넘어온 idx : " + type);
        switch (type)
        {
            case 0:
                if (ShieldLevel >= 5) break;
                ShieldLevel++;
                break;
            case 1:
                if (FieldLevel >= 5) break;
                FieldLevel++;
                break;
            case 2:
                if (CrowLevel >= 5) break;
                CrowLevel++;
                break;
            case 3:
                if (HealthLevel >= 5) break;
                HealthLevel++;
                break;
            case 4:
                if (ATTLevel >= 5) break;
                ATTLevel++;
                break;
            case 5:
                if (SPDLevel >= 5) break;
                SPDLevel++;
                break;
            case 6:
                if (PenetrationLevel >= 5) break;
                PenetrationLevel++;
                break;
        }
        GameManager.gameManager.UiManager.SelectUI_Off();
    }
    public void Skill_Init()
    {
        for (int i = 0; i < Skill_Inventory.Count; i++)
        {
            switch (Skill_Inventory[i]._Name)
            {
                case Skill.Skill_Name.SHIELD:
                    Skill_Inventory[i].Level = ShieldLevel;
                    switch (Skill_Inventory[i].Level)
                    {
                        case 1:
                            Skill_Inventory[i].Shield_Speed = 150f;
                            Skill_Inventory[i].ATT = GameManager.gameManager.BulletDamage * 0.3f;
                            Skill_Inventory[i].Shield_Radius = radius;
                            break;
                        case 2:
                            Skill_Inventory[i].ATT = GameManager.gameManager.BulletDamage * 0.5f;
                            break;
                    }
                    switch (ShieldLevel)
                    {
                        default:
                            regenerateTime = 3f;
                            break;
                        case 4:
                            regenerateTime = 1.5f;
                            break;
                        case 5:
                            regenerateTime = 0f;
                            break;
                    }
                    break;
                case Skill.Skill_Name.FIELD:
                    Skill_Inventory[i].Level = FieldLevel;
                    switch (Skill_Inventory[i].Level)
                    {
                        case 1:
                            Skill_Inventory[i].ATT = GameManager.gameManager.BulletDamage * 0.3f;
                            break;
                        case 2:
                            Skill_Inventory[i].ATT = (GameManager.gameManager.BulletDamage * 0.3f) + 10;
                            break;
                        case 3:
                            Skill_Inventory[i].Field.transform.localScale = new Vector3(2 + 2 / 10, 2 + 2 / 10, 1);
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                    }
                    break;
                case Skill.Skill_Name.CROW:
                    Skill_Inventory[i].Level = CrowLevel;
                    Skill_Inventory[i].Crow_Speed = 100f;
                    switch (Skill_Inventory[i].Level)
                    {
                        case 1:
                            Skill_Inventory[i].Crow_Radius = 6f;
                            Skill_Inventory[i].ATT = GameManager.gameManager.BulletDamage * 0.4f;
                            AttackDelay = 1.5f;
                            break;
                        case 2:
                            Skill_Inventory[i].ATT = GameManager.gameManager.BulletDamage * 0.5f;
                            break;
                        case 3:
                            AttackDelay = 1.2f;
                            break;
                        case 4:
                            Skill_Inventory[i].ATT = GameManager.gameManager.BulletDamage * 0.7f;
                            break;
                        case 5:
                            AttackDelay = 0.9f;
                            break;
                    }
                    break;
                case Skill.Skill_Name.HEALTH:
                    Skill_Inventory[i].Level = HealthLevel;
                    switch (Skill_Inventory[i].Level)
                    {
                        case 1:
                            GameManager.gameManager.Health = 1;
                            break;
                        case 2:
                            GameManager.gameManager.Health = 2;
                            break;
                        case 3:
                            GameManager.gameManager.Health = 3;
                            break;
                        case 4:
                            GameManager.gameManager.Health = 4;
                            break;
                        case 5:
                            GameManager.gameManager.Health = 5;
                            break;
                    }

                    break;
                case Skill.Skill_Name.ATT:
                    Skill_Inventory[i].Level = ATTLevel;
                    switch (Skill_Inventory[i].Level)
                    {
                        case 1:
                            GameManager.gameManager.AttackDamage = 1;
                            break;
                        case 2:
                            GameManager.gameManager.AttackDamage = 2;
                            break;
                        case 3:
                            GameManager.gameManager.AttackDamage = 3;
                            break;
                        case 4:
                            GameManager.gameManager.AttackDamage = 4;
                            break;
                        case 5:
                            GameManager.gameManager.AttackDamage = 5;
                            break;
                    }
                    break;
                case Skill.Skill_Name.SPEED:
                    Skill_Inventory[i].Level = SPDLevel;
                    switch (Skill_Inventory[i].Level)
                    {
                        case 1:
                            GameManager.gameManager.AttackSpeed = 1;
                            break;
                        case 2:
                            GameManager.gameManager.AttackSpeed = 2;
                            break;
                        case 3:
                            GameManager.gameManager.AttackSpeed = 3;
                            break;
                        case 4:
                            GameManager.gameManager.AttackSpeed = 4;
                            break;
                        case 5:
                            GameManager.gameManager.AttackSpeed = 5;
                            break;
                    }
                    break;
                case Skill.Skill_Name.PENETRATION:
                    Skill_Inventory[i].Level = PenetrationLevel;
                    switch (Skill_Inventory[i].Level)
                    {
                        case 1:
                            GameManager.gameManager.Penetration = 1;
                            break;
                        case 2:
                            GameManager.gameManager.Penetration = 2;
                            break;
                        case 3:
                            GameManager.gameManager.Penetration = 3;
                            break;
                        case 4:
                            GameManager.gameManager.Penetration = 4;
                            break;
                        case 5:
                            GameManager.gameManager.Penetration = 5;
                            break;
                    }
                    break;
            }
        }
    }
}


