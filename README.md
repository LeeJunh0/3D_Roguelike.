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

    //데이터파일이 있는지 확인
    if (File.Exists(FilePath) == true)
    {          
        string FromJsonFile = File.ReadAllText(FilePath);
        playerData = JsonUtility.FromJson<PlayerData>(FromJsonFile);            
    }
    //없다면 초기값 데이터생성
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
    //가지고있는 데이터 저장
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
//플레이어의 진행방향을 가져온다.
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
        //위아래 재배치
        gameObject.transform.Translate(new Vector3(0, 0, 1) * inputZ * 100f);
        return;
    }
    break;
    case "Enermy":
    if(gameObject.layer == 8)
    {
        //몬스터는 플레이어의 주변 랜덤한 위치에서 재배치되도록 구현하였다.
        gameObject.transform.position = (playerPos + (GameManager.gameManager.PlayerScript.MoveVec * 25f) + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)));                  
        return;
    }          
    break;
}

```
## 액티브 스킬구현
### 회전방패
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/78d9e72c-20aa-4833-8b94-6e5cf3e6b643" width="600px" height = "300px"><br>
### 바닥지속오라
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/9b951625-66b8-4a6f-8c77-655a5a500ae0" width="600px" height = "300px"><br>
### 비둘기
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/b172fe37-8eec-4f46-819f-89193f52e3cb" width="600px" height = "300px"><br>

## 시간에 따른 난이도구현
## 웨이브 이벤트
# 마무리
