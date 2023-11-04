# 유니티3D 포트폴리오
# 목차
- [소개영상](#https://www.youtube.com/watch?v=jJ99QTiroZ8)
- [구현내용](#구현내용)
- [마무리](#마무리)
# 구현내용
- [데이터 세이브로드](#데이터-세이브로드)
- [맵, 몬스터재배치](#맵,-몬스터재배치)
- [액티브 스킬구현](#액티브-스킬구현)
- [시간에 따른 난이도구현](#시간에-따른-난이도구현)
- [웨이브 이벤트](#웨이브-이벤트)
# 데이터 세이브로드
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
# 맵, 몬스터재배치
- 맵 재배치
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/af0bfaca-b2ae-44fe-8da9-b74ec8060e0e" width="300px" height = "300px">
<img src = "https://github.com/LeeJunh0/3D_Roguelike./assets/83407767/3ab8ae19-b621-420a-bbcf-e90f25b13088" width="300px" height = "300px">
