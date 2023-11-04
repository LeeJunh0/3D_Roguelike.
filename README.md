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
- [Wave Event](#wave-event)

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
## Wave Event
**유저에게 최소한의 스펙을 강요하는 이벤트로 5개의 웨이브로 구성**   
각 웨이브마다 Inspector창에서 해당 WaveStatus로 초기화 

### First Wave
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/9cc4ee49-fc47-4597-b8ac-d4d3fe6fe742" width="600px" height = "500px">
<br>
- 원형으로 플레이어를 포위하며 다가오는 웨이브
<br>
<br>

```C#
public void Wave1Init()
{
    Transform center = playerpos;
    float radius = 25f;
    for (int i = 1; i <= 72; i++)
    {
        // 스폰과정 생략...
        // 72마리의 몬스터를 원형에 맞게 위치지정
        float rad = Mathf.Deg2Rad * (i * 5);
        float x = radius * Mathf.Sin(rad);
        float z = radius * Mathf.Cos(rad);

        enemy.transform.position = playerpos.position + new Vector3(x, 0, z);
        // 웨이브몬스터 Status 초기화
        Enermy status = enemy.GetComponent<Enermy>();
        status.maxHp = WaveDatas[0].MaxHP;
        status.curHp = status.maxHp;
        status.MoveSpeed = WaveDatas[0].Speed;
        status.Damage = WaveDatas[0].Damage;
        status.MyWave = 1;
        Duration = WaveDatas[0].LifeTime;
    }
NowWave = 1;
}
```
### Second Wave
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/676070ab-1336-41ab-ade2-c40343fefc39" width="600px" height = "500px">
<br>
- 소규모무리가 빠른속도로 특정방향을 향해 돌진하는 웨이브(지속적으로 발생)
<br>
<br>

- 웨이브 생성코드

```C#
public void Wave2Init()
{    
    Temppos = GameManager.gameManager.PlayerScript.transform;
        
    Transform temppos = GameManager.gameManager.spawner.SpawnPoints[Random.Range(1,5)].transform;       
    Speed = 15f;
    for(int i = 0; i < 15; i++)
        {
            // 보스등장시 웨이브몬스터는 전부 사라지기때문에 관리에 용이하도록 리스트에 보관
            SubWaveList.Add(Pooling.instance.GetElement(this.gameObject));
            SubWaveList[i].layer = 21;
            SubWaveList[i].tag = "WaveEnemy";
            SubWaveList[i].SetActive(true);
            SubWaveList[i].transform.position = temppos.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            
            Enermy status = SubWaveList[i].GetComponent<Enermy>();
            status.maxHp = WaveDatas[1].MaxHP;
            status.curHp = status.maxHp;
            status.MoveSpeed = WaveDatas[1].Speed;
            status.Damage = WaveDatas[1].Damage;
            status.MyWave = 2;
            Duration = WaveDatas[1].LifeTime;
            // 동작코드...
        }
}
```
- 웨이브 동작코드
```C#
for(int i = 0; i < SubWaveList.Count; i++)
{
    // 플레이어의 주변포인트로 Speed만큼 빠르게 가도록 지정
    Rigidbody rigid = SubWaveList[i].GetComponent<Rigidbody>();
    rigid.velocity = new Vector3(Temppos.position.x -SubWaveList[i].transform.position.x, 0, Temppos.position.zSubWaveList[i].transform.position.z).normalized * Speed;
    SubWaveList[i].transform.LookAt(new Vector3(Temppos.position.x - SubWaveList[i].transform.position.z,
    0, Temppos.position.z - SubWaveList[i].transform.position.z));
}
SubTimer = 0f;
SubWaveList.Clear();
```
### Third Wave
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/2f17bec5-826c-4227-a864-8eccfeea9adb" width="600px" height = "500px">
<br>
- 왼쪽에서 오른쪽으로 간격을 유지하며 다가오는 웨이브
<br>

```C#
public void Wave3Init()
{
    for(int i = 0; i < 30; i++)
    {
    GameObject enemy = Pooling.instance.GetElement(this.gameObject);
    enemy.layer = 21;
    enemy.tag = "WaveEnemy";
    enemy.SetActive(true);

    Enermy status = enemy.GetComponent<Enermy>();
    status.maxHp = WaveDatas[2].MaxHP;
    status.curHp = status.maxHp;
    status.MoveSpeed = WaveDatas[2].Speed;
    status.Damage = WaveDatas[2].Damage;
    status.MyWave = 3;
    Duration = WaveDatas[2].LifeTime;

    enemy.transform.position = new Vector3(playerpos.position.x - 30f, 0, playerpos.position.z - 50f + (i * 4f));
    }
    NowWave = 3;
}
```
### Fourth Wave
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/c6b6a3e3-64b3-424c-b576-c02236428550" width="600px" height = "500px">
<br>
- 상하좌우 에서 세번째 웨이브가 발생하는 웨이브
<br>
- 웨이브 생성코드

```C#
public void Wave4Init()
    {
        List<GameObject> Enemylist = new List<GameObject>();
        for(int i = 0; i < 120; i++)
        {
            //생성 및 Status초기화 생략...
        }
        // 웨이브 동작코루틴
        StartCoroutine(Wave4Cor(Enemylist));
        NowWave = 4;
    }
```
- 웨이브 동작코드
```C#
public IEnumerator Wave4Cor(List<GameObject> list)
{
    // 상하좌우로 동작하는 세번째웨이브를 실행.
    for (int i = 0; i < 30; i++)
    {
        list[i].SetActive(true);
        list[i].transform.position = new Vector3(playerpos.position.x - 30f, 0, playerpos.position.z - 50f + (i * 5f));
        list[i].transform.LookAt(new Vector3(9999f, 0, list[i].transform.position.z));           
    }
    list.RemoveRange(0, 30);
    yield return new WaitForSeconds(1f);
    // 방향만 다른 반복코드 생략...
}
```
### Fifth Wave
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/96efc4af-3d5d-42da-9b7d-619f6b42bc7d" width="600px" height = "500px">
<br>
- 첫번째 웨이브가 지속적으로 발생하는 웨이브
<br>

# 마무리
