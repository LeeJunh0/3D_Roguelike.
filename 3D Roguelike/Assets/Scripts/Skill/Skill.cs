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

    // 방패스킬
    public float Shield_Radius;   
    public GameObject Prefab_Shield;
    public float Shield_Speed;
    public List<GameObject> Shield_Count;

    // 필드스킬
    public GameObject Field_Prefab;
    public GameObject Field;
    public float DebuffValue = 0.2f;
    public float Debuffsec = 1.5f;
    // 크로우스킬
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
                Skill_Plan[0] = "주변을 회전하는 방패가 몬스터로부터 플레이어를 지켜줍니다.";
                Skill_Plan[1] = "방패 1개 증가 ";
                Skill_Plan[2] = "데미지 소폭증가 ";
                Skill_Plan[3] = "생성주기 감소";
                Skill_Plan[4] = "개수가 1증가 합니다, 이제 방패가 사라지지않습니다.";
                break;
            case Skill_Name.FIELD:
                SkillIcon = Resources.Load<Sprite>("Field");                                  
                Skill_Plan[0] = "마음의 어둠이 표출됩니다.";
                Skill_Plan[1] = "데미지 소폭증가";
                Skill_Plan[2] = "범위 소폭증가";
                Skill_Plan[3] = "공격속도 소폭증가";
                Skill_Plan[4] = "마음의 슬픔으로 주변적이 느려집니다. 범위 소폭증가";
                break;
            case Skill_Name.CROW:
                SkillIcon = Resources.Load<Sprite>("Crow");                                    
                Skill_Plan[0] = "비둘기가 나타나 적에게 울음소리를 냅니다.";
                Skill_Plan[1] = "데미지 소폭증가";
                Skill_Plan[2] = "공격속도 소폭 증가";
                Skill_Plan[3] = "데미지 소폭증가";
                Skill_Plan[4] = "비둘기의 친구가 나타납니다. 공격속도 소폭 증가 ";
                break;
            case Skill_Name.HEALTH:
                SkillIcon = Resources.Load<Sprite>("Heart");                                   
                Skill_Plan[0] = "체력이 10 증가합니다.";
                Skill_Plan[1] = "체력이 20 증가합니다.";
                Skill_Plan[2] = "체력이 30 증가합니다.";
                Skill_Plan[3] = "체력이 40 증가합니다.";
                Skill_Plan[4] = "체력이 50 증가합니다.";
                break;
            case Skill_Name.ATT:
                SkillIcon = Resources.Load<Sprite>("ATT");                                   
                Skill_Plan[0] = "공격력이 5 증가합니다.";
                Skill_Plan[1] = "공격력이 10 증가합니다.";
                Skill_Plan[2] = "공격력이 15 증가합니다.";
                Skill_Plan[3] = "공격력이 20 증가합니다.";
                Skill_Plan[4] = "공격력이 25 증가합니다.";
                break;
            case Skill_Name.SPEED:
                SkillIcon = Resources.Load<Sprite>("SPD");
                Skill_Plan[0] = "공격속도가 10% 증가합니다.";
                Skill_Plan[1] = "공격속도가 20% 증가합니다.";
                Skill_Plan[2] = "공격속도가 30% 증가합니다.";
                Skill_Plan[3] = "공격속도가 40% 증가합니다.";
                Skill_Plan[4] = "공격속도가 50% 증가합니다.";
                break;
            case Skill_Name.PENETRATION:
                SkillIcon = Resources.Load<Sprite>("Penetration");
                Skill_Plan[0] = "적을 1번 관통합니다.";
                Skill_Plan[1] = "적을 2번 관통합니다.";
                Skill_Plan[2] = "적을 3번 관통합니다.";
                Skill_Plan[3] = "적을 4번 관통합니다.";
                Skill_Plan[4] = "적을 5번 관통합니다.";
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
                //1레벨이 되면 생성, 범위 안에 특정시간있으면 있는적들에게 데미지를 주고 시간 초기화.
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
