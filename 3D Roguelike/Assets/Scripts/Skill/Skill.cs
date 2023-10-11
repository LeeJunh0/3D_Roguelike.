using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill : MonoBehaviour
{ 
    public enum Skill_Name { SHIELD, FIELD, CROW , HEALTH,ATT,SPEED, PENETRATION, NONE};
    public Skill_Name _Name = Skill_Name.NONE;
    public string[] Skill_Plan;
    public Sprite SkillIcon;
    public float ATT;
    public int Level;
    public float Angle;
    Vector3 playerpos;

    // ���н�ų
    public float Shield_Radius;   
    public GameObject Prefab_Shield;
    public float Shield_Speed;
    public List<GameObject> Shield_Count;

    // �ʵ彺ų
    public GameObject Field_Prefab;
    public GameObject Field;
    public float DebuffValue = 0.2f;
    public float Debuffsec = 1.5f;
    // ũ�ο콺ų
    public List<GameObject> Crow_Count;
    public float Crow_Speed;
    public float Crow_Radius;
    public GameObject Crow_Prefab;
    public GameObject Bird_Prefab;
    GameObject Crow;
    private void Start()
    {
        Skill_Plan = new string[5];

        switch (_Name)
        {
            case Skill_Name.SHIELD:
                Shield_Count = new List<GameObject>();
                SkillIcon = Resources.Load<Sprite>("Shield");
                Skill_Plan[0] = "�ֺ��� ȸ���ϴ� ���а� ���ͷκ��� �÷��̾ �����ݴϴ�.";
                Skill_Plan[1] = "���� 1�� ���� ";
                Skill_Plan[2] = "������ �������� ";
                Skill_Plan[3] = "�����ֱ� ����";
                Skill_Plan[4] = "������ 1���� �մϴ�, ���� ���а� ��������ʽ��ϴ�.";
                break;
            case Skill_Name.FIELD:
                SkillIcon = Resources.Load<Sprite>("Field");                                  
                Skill_Plan[0] = "������ ����� ǥ��˴ϴ�.";
                Skill_Plan[1] = "������ ��������";
                Skill_Plan[2] = "���� ��������";
                Skill_Plan[3] = "���ݼӵ� ��������";
                Skill_Plan[4] = "������ �������� �ֺ����� �������ϴ�. ���� ��������";
                break;
            case Skill_Name.CROW:
                SkillIcon = Resources.Load<Sprite>("Crow");                                    
                Skill_Plan[0] = "��ѱⰡ ��Ÿ�� ������ �����Ҹ��� ���ϴ�.";
                Skill_Plan[1] = "������ ��������";
                Skill_Plan[2] = "���ݼӵ� ���� ����";
                Skill_Plan[3] = "������ ��������";
                Skill_Plan[4] = "��ѱ��� ģ���� ��Ÿ���ϴ�. ���ݼӵ� ���� ���� ";
                break;
            case Skill_Name.HEALTH:
                SkillIcon = Resources.Load<Sprite>("Heart");                                   
                Skill_Plan[0] = "ü���� 10 �����մϴ�.";
                Skill_Plan[1] = "ü���� 20 �����մϴ�.";
                Skill_Plan[2] = "ü���� 30 �����մϴ�.";
                Skill_Plan[3] = "ü���� 40 �����մϴ�.";
                Skill_Plan[4] = "ü���� 50 �����մϴ�.";
                break;
            case Skill_Name.ATT:
                SkillIcon = Resources.Load<Sprite>("ATT");                                   
                Skill_Plan[0] = "���ݷ��� 5 �����մϴ�.";
                Skill_Plan[1] = "���ݷ��� 10 �����մϴ�.";
                Skill_Plan[2] = "���ݷ��� 15 �����մϴ�.";
                Skill_Plan[3] = "���ݷ��� 20 �����մϴ�.";
                Skill_Plan[4] = "���ݷ��� 25 �����մϴ�.";
                break;
            case Skill_Name.SPEED:
                SkillIcon = Resources.Load<Sprite>("SPD");
                Skill_Plan[0] = "���ݼӵ��� 10% �����մϴ�.";
                Skill_Plan[1] = "���ݼӵ��� 20% �����մϴ�.";
                Skill_Plan[2] = "���ݼӵ��� 30% �����մϴ�.";
                Skill_Plan[3] = "���ݼӵ��� 40% �����մϴ�.";
                Skill_Plan[4] = "���ݼӵ��� 50% �����մϴ�.";
                break;
            case Skill_Name.PENETRATION:
                SkillIcon = Resources.Load<Sprite>("Penetration");
                Skill_Plan[0] = "���� 1�� �����մϴ�.";
                Skill_Plan[1] = "���� 2�� �����մϴ�.";
                Skill_Plan[2] = "���� 3�� �����մϴ�.";
                Skill_Plan[3] = "���� 4�� �����մϴ�.";
                Skill_Plan[4] = "���� 5�� �����մϴ�.";
                break;
        }
    }
    private void Update()
    {
        playerpos = GameManager.gameManager.PlayerScript.transform.position;
        switch (_Name)
        {
            case Skill_Name.SHIELD:
                switch (Level)
                {
                    case 1:
                        if (3 > Shield_Count.Count)
                            Shield_Count.Add(Instantiate(Prefab_Shield));
                        break;
                    case 2:
                        if (4 > Shield_Count.Count)
                            Shield_Count.Add(Instantiate(Prefab_Shield));
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        if (5 > Shield_Count.Count)
                            Shield_Count.Add(Instantiate(Prefab_Shield));
                        break;
                }
                if (Shield_Count.Count > 0)
                {
                    Angle += Time.deltaTime * Shield_Speed;
                    if (Angle < 360)
                    {
                        for (int i = 0; i < Shield_Count.Count; i++)
                        {
                            float rad = Mathf.Deg2Rad * (Angle + i * (360 / Shield_Count.Count));
                            float x = Shield_Radius * Mathf.Sin(rad);
                            float z = Shield_Radius * Mathf.Cos(rad);
                            Shield_Count[i].transform.position = playerpos + new Vector3(x, 0, z);
                            Shield_Count[i].transform.LookAt(playerpos);
                        }
                    }
                    else
                        Angle = 0;
                }
                break;
            case Skill_Name.FIELD:
                //1������ �Ǹ� ����, ���� �ȿ� Ư���ð������� �ִ����鿡�� �������� �ְ� �ð� �ʱ�ȭ.
                switch (Level)
                {
                    default:
                        break;
                    case 1:
                        if (Field == null)
                            Field = Instantiate(Field_Prefab);
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                }
                if (Level > 0)
                    Field.transform.position = new Vector3(playerpos.x, Field.transform.position.y, playerpos.z);
                break;
            case Skill_Name.CROW:
                switch (Level)
                {
                    case 1:
                        if (Crow_Count.Count < 1)
                            Crow_Count.Add(Instantiate(Bird_Prefab));
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        if (Crow_Count.Count < 2)
                            Crow_Count.Add(Instantiate(Crow_Prefab));
                        break;
                }
                if (Crow_Count.Count > 0)
                {
                    Angle += Time.deltaTime * Crow_Speed;
                    for (int i = 0; i < Crow_Count.Count; i++)
                    {
                        float rad = Mathf.Deg2Rad * (Angle + i * (360 / Crow_Count.Count));
                        float x = Crow_Radius * Mathf.Sin(rad);
                        float z = Crow_Radius * Mathf.Cos(rad);
                        Crow_Count[i].transform.position = playerpos + new Vector3(x, 0, z);
                        Crow_Count[i].transform.forward = new Vector3(x, 0.1f, z).normalized;
                        //Crow_Count[i].transform.LookAt(playerpos * -1);
                    }
                }
                break;
            default:
                break;
        }
    }
}
