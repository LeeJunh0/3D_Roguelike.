using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] NowData;
    public Transform[] SpawnPoints;
    public List<GameObject> Mob;
    public Enermy BossObject;
    public GameObject Boss;
    public Transform PlayerPos;
    public float time;
    public float SpawnTime;    
    public int SpawnLevel = 0;
    void Start()
    {        
        SpawnPoints = GetComponentsInChildren<Transform>();        
    }

    void Update()
    {        
        if(GameManager.gameManager.UiManager.PlayTime < 1080f)
        {
            time += Time.deltaTime;
            SpawnLevel = Mathf.FloorToInt(GameManager.gameManager.UiManager.PlayTime / 100);
            SpawnTime = NowData[SpawnLevel].SpawnTime;
            if (time > SpawnTime)
            {
                Spawn(NowData[SpawnLevel]);
                time = 0f;
            }
            transform.position = GameManager.gameManager.PlayerScript.transform.position;
        }
    }
    void Spawn(SpawnData data)
    {
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
}

[System.Serializable]
public class SpawnData
{
    public float Speed;
    public float MaxHP;
    public float Damage;
    public float SpawnTime;
    //public GameObject Boss_Prefab;
}
