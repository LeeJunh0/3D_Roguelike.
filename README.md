# 유니티3D 포트폴리오
# 목차
- [소개영상](https://www.youtube.com/watch?v=jJ99QTiroZ8)
- [구현내용](#구현내용)
- [마무리](#마무리)
# 구현내용
- [데이터 세이브로드](#데이터-세이브로드)
- [몬스터와 맵 재배치](#몬스터와-맵-재배치)
  >[몬스터 재배치](#몬스터-재배치)   
  >[맵 재배치](#맵-재배치)
- [액티브 스킬구현](#액티브-스킬구현)
- [시간에 따른 난이도구현](#시간에-따른-난이도구현)
- [웨이브 이벤트](#웨이브-이벤트)

## 데이터 세이브로드
싱글톤으로 구현하여 언제든 저장을 할수있도록 구현,   
**PlayerData를 수정하여 다른 데이터방식도 저장할수있도록 구현**
```C#
static public DataController instance = null;
public PlayerData playerData;

private void Awake()
{       
  if (instance == null)
  {
      instance = this;            
  }
  else
  {
      Destroy(this.gameObject);
  }
  DontDestroyOnLoad(this);
}
```
- Load함수
```C#
public void LoadData()
{
    string FilePath = Application.persistentDataPath + "/Save";

    // 데이터파일이 있는지 확인
    if (File.Exists(FilePath) == true)
    {          
        string FromJsonFile = File.ReadAllText(FilePath);
        playerData = JsonUtility.FromJson<PlayerData>(FromJsonFile);            
    }
    // 없다면 초기값 데이터생성
    else
    {
        playerData = new PlayerData();            
    }
}
```
- Save함수

```C#
public void SaveData()
{
    // 가지고있는 데이터 저장
    string FilePath = Application.persistentDataPath + "/Save";
    string FileJson = JsonUtility.ToJson(playerData, true);

    FileStream filestream = new FileStream(FilePath, FileMode.Create);
    byte[] data = Encoding.UTF8.GetBytes(FileJson);
    filestream.Write(data, 0, data.Length);
    filestream.Close();
}

```
## 몬스터와 맵 재배치
### 몬스터 재배치
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/b468e8a4-7bea-445a-a688-d1c484a6ee8f" width="400px" height = "300px">
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/6a80bf43-076f-4e5d-b633-d96ee015100e" width="400px" height = "300px"> <br>
- 몬스터가 경계에서 떨어질때 플레이어주변 랜덤한 위치로 재배치 되도록 구현


### 맵 재배치    
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/af0bfaca-b2ae-44fe-8da9-b74ec8060e0e" width="400px" height = "300px">
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/3ab8ae19-b621-420a-bbcf-e90f25b13088" width="400px" height = "300px"> <br>
- 맵이 경계에서 떨어질때 플레이어의 진행방향으로 맵을 재배치하도록 구현

### 재배치 상세코드  
```C#
Vector3 InputVec = GameManager.gameManager.PlayerScript.MoveVec;
float inputX = InputVec.x < 0 ? -1 : 1;
float inputZ = InputVec.z < 0 ? -1 : 1;

switch (gameObject.tag)
{
    case "Ground":
    if(Mathf.Abs(diffX-diffZ) <= 0.1f)
    {
          // 미세한차이로 맵타일이 튕겨져 나가는것을 방지하기위해 구현하였다.
          gameObject.transform.Translate(Vector3.up * inputZ * 100f);
          gameObject.transform.Translate(Vector3.right * inputX * 100f);
    }
    else if(diffX >= diffZ)
    {
        // 좌우 재배치
        gameObject.transform.Translate(Vector3.right * inputX * 100f);
        return;
    }
    else if(diffX <= diffZ)
    {
        // 위아래 재배치
        gameObject.transform.Translate(new Vector3(0, 0, 1) * inputZ * 100f);
        return;
    }
    break;
    case "Enermy":
    if(gameObject.layer == 8)
    {
        // 몬스터는 플레이어의 주변 랜덤한 위치에서 재배치되도록 구현하였다.
        gameObject.transform.position = (playerPos + (GameManager.gameManager.PlayerScript.MoveVec * 25f) + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)));                  
        return;
    }          
    break;
}

```
## 액티브 스킬구현
### 회전방패
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/78d9e72c-20aa-4833-8b94-6e5cf3e6b643" width="600px" height = "300px"><br>
- 주변을 회전하는 방패로 최대 5개까지 증가
```C#
if (Shield_Count.Count > 0)
{
    Angle += Time.deltaTime * Shield_Speed;
    if (Angle < 360)
    {
        // 360도에서 방패의 개수 만큼 나눠 방패들의 위치를 결정
        for (int i = 0; i < Shield_Count.Count; i++)
        {
            // 각 방패별 각도를 radian값으로 바꿔
            // radian값을 삼각함수를 이용해 x,z좌표 값을 얻어와 방패의 위치를 결정
            float rad = Mathf.Deg2Rad * (Angle + i * (360 / Shield_Count.Count));
            float x = Shield_Radius * Mathf.Sin(rad);
            float z = Shield_Radius * Mathf.Cos(rad);
            Shield_Count[i].transform.position = playerpos + new Vector3(x, 0, z);
            Shield_Count[i].transform.LookAt(playerpos);
        }
    }
    // 360도를 넘으면 Angle을 0으로 초기화해준다.
    else
        Angle = 0;
}
```
### 바닥지속오라
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/9b951625-66b8-4a6f-8c77-655a5a500ae0" width="600px" height = "300px"><br>
- 최대레벨시 닿은적에게 슬로우부여
```C#
private void OnTriggerStay(Collider other)
{       
    if(other.gameObject.layer == 17 && (gameObject.layer == 8 || gameObject.layer == 21))
    {
        // 스킬레벨 확인후 슬로우코루틴실행
        if (SkillManager.skillManager.Skill_Inventory[1].Level >= 5)
        {
            float sec = SkillManager.skillManager.Skill_Inventory[1].Debuffsec;
            float value = SkillManager.skillManager.Skill_Inventory[1].DebuffValue;
            StartCoroutine(Debuff(value, sec));
        }
        // time은 Update문에서 deltaTime을 더해주는 방식
        // time을 검사하고 피격코루틴 실행 후 time초기화
        if (time >= SkillManager.skillManager.DamageTime)
        {
            TextInit(SkillManager.skillManager.Skill_Inventory[1].ATT, transform);
            StartCoroutine(EnermyDamage(SkillManager.skillManager.Skill_Inventory[1].ATT));                
            time = 0;
        }            
    }
  }
```
### 비둘기
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/b172fe37-8eec-4f46-819f-89193f52e3cb" width="600px" height = "300px"><br>
- 최대레벨시 2마리까지 증가
#### 회전 및 위치결정 코드
```C#
if (Crow_Count.Count > 0)
{
    // 방패의 회전과 같은 과정으로 위치를 결정
    Angle += Time.deltaTime * Crow_Speed;
    for (int i = 0; i < Crow_Count.Count; i++)
    {
        float rad = Mathf.Deg2Rad * (Angle + i * (360 / Crow_Count.Count));
        float x = Crow_Radius * Mathf.Sin(rad);
        float z = Crow_Radius * Mathf.Cos(rad);
        Crow_Count[i].transform.position = playerpos + new Vector3(x, 0, z);
        Crow_Count[i].transform.forward = new Vector3(x, 0.1f, z).normalized;
    }
}
```
#### 범위지정 및 공격코드
```C#
void Update()
{
    // 공격 가능한 시간일때 Overlap을 만들어 공격가능한 몬스터를 배열에 담고
    // 배열을 순회하며 해당몬스터의 피격코루틴을 실행
    AttackRange = SkillManager.skillManager.HowlingRange;
    LoopTime = SkillManager.skillManager.AttackDelay;
    CurTime += Time.deltaTime;

    if (CurTime > LoopTime)
    {
        Myaudio.PlayOneShot(Myaudio.clip);
        Collider[] enemy = Physics.OverlapSphere(transform.position, AttackRange, 1 << 8);
        if (enemy.Length > 0)
        {
            for(int i = 0; i < enemy.Length; i++)
            {
                Enermy cur = enemy[i].GetComponent<Enermy>();
                cur.TextInit(SkillManager.skillManager.Skill_Inventory[2].ATT, cur.transform);
                StartCoroutine(cur.EnermyDamage(SkillManager.skillManager.Skill_Inventory[2].ATT));
            }
        }
        CurTime = 0f;
    }
}
```
## 시간에 따른 난이도구현
**시간에 따른 난이도 상승을 위해 구현**
- 유니티에서의 스폰설정<br>
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/5017d0f5-9477-4b2f-abe7-82f8a9bcec27" width="400px" height = "600px">

### SpawnData
```C#
//Inspector창에서 조정 가능하게 구현
public SpawnData[] NowData;

[System.Serializable]
public class SpawnData
{
    public float Speed;
    public float MaxHP;
    public float Damage;
    public float SpawnTime;
}
```
### Spawn레벨설정 및 몬스터Status값설정
#### Spawn레벨설정
```C#
// PlayTime에 따라 스폰레벨값을 변경
// 현재 스폰레벨에 맞는 스폰시간으로 지정
time += Time.deltaTime;
SpawnLevel = Mathf.FloorToInt(GameManager.gameManager.UiManager.PlayTime / 100);
SpawnTime = NowData[SpawnLevel].SpawnTime; 
if (time > SpawnTime)
{
    Spawn(NowData[SpawnLevel]);
    time = 0f;
}
```

#### 몬스터Status값설정

```C#
void Spawn(SpawnData data)
{
    // 랜덤포인트에 몬스터 생성
    // 기본 Layer, tag, 현재레벨의 Status를 설정
    int Pointidx = Random.Range(1, 12);
    for (int i = 0; i <= SpawnLevel; i++)
    {
        GameObject CopyMob = Pooling.instance.GetElement(this.gameObject);
        CopyMob.layer = 8;
        CopyMob.tag = "Enermy";
        CopyMob.transform.position = SpawnPoints[Pointidx].transform.position;
        CopyMob.SetActive(true);
        Enermy Mob_Init = CopyMob.GetComponent<Enermy>();
        Mob_Init.curHp = data.MaxHP;
        Mob_Init.maxHp = data.MaxHP;
        Mob_Init.Damage = data.Damage;
        Mob_Init.MoveSpeed = data.Speed;
        Mob_Init.time = 1.5f;
    }
}
```
## 웨이브 이벤트
# 마무리
