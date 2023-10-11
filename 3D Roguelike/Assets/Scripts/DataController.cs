using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Text;

public class DataController : MonoBehaviour
{    
    [Serializable]
    public class PlayerData
    {
        public float PlayerSpeed = 9f;
        public string MaxScore;
        public int Coin;
        public float MaxHp;
        public int isLevel;
        public float Bullet_Damage;
        public float GunSpeed;
        public int Hp_level;
        public int ATT_level;
        public int ATS_level;
        public int SPD_level;
        public PlayerData()
        {
            PlayerSpeed = 9f;
            MaxScore = "";
            Coin = 0;
            MaxHp = 10;
            isLevel = 1;
            Bullet_Damage = 10;
            GunSpeed = 2f;
            Hp_level = 1;
            ATT_level = 1;
            ATS_level = 1;
            SPD_level = 1;
        }
    }


    static public DataController instance = null;
    public string playerName;
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

    public PlayerData gamedata
    {
        get
        {
            if(playerData == null)
            {
                LoadData();
                SaveData();
            }
            return playerData;
        }
    }
    
    void Start()
    {
        playerName = "StartText.json";
        LoadData();
        SaveData();
    }
    public void LoadData()
    {
        string FilePath = "C:/Users/Administrator/Downloads/3D Roguelike/Assets/Resources/" + playerName;
        if (File.Exists(FilePath) == true)
        {          
            string FromJsonFile = File.ReadAllText(FilePath);
            playerData = JsonUtility.FromJson<PlayerData>(FromJsonFile);
            Debug.Log("불러오기 성공");
        }
        else
        {
            playerData = new PlayerData();
            Debug.Log("새로운 저장파일 생성");
        }
    }
    public void SaveData()
    {
        Debug.Log("저장시작");
        string FilePath = "C:/Users/Administrator/Downloads/3D Roguelike/Assets/Resources/" + playerName;
        string FileJson = JsonUtility.ToJson(playerData, true);

        FileStream filestream = new FileStream(FilePath, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(FileJson);
        filestream.Write(data, 0, data.Length);
        filestream.Close();
    }
}



