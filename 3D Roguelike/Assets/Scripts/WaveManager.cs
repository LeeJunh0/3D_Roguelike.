using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Transform playerpos;
    public Transform Temppos;
    public List<GameObject> WaveMobList;
    public List<GameObject> SubWaveList;
    public int NowWave;
    public float WaveTime;
    public float Duration;
    public float curWaveTime;
    public float SubTimer = 61f;
    public float LastTimer = 5f;
    public float BossTimer = 0f;
    public bool Wave1;
    public bool Wave2;
    public bool Wave3;
    public bool Wave4;
    public bool Wave5;
    public bool BossWave;

    public float Speed;
    public WaveData[] WaveDatas;
    public GameObject Boss;

    public float Wave1Time;
    public float Wave2Time;
    public float Wave3Time;
    public float Wave4Time;
    public float Wave5Time;
    public float BossTime;
    void Start()
    {
        WaveMobList = new List<GameObject>();
        NowWave = 0;
        Boss.SetActive(false);
        Enermy bossStatus = Boss.GetComponent<Enermy>();

        bossStatus.MoveSpeed = WaveDatas[5].Speed;
        bossStatus.maxHp = WaveDatas[5].MaxHP;
        bossStatus.curHp = bossStatus.maxHp;
        bossStatus.Damage = WaveDatas[5].Damage;
    }

    void Update()
    {           
        playerpos = GameManager.gameManager.PlayerScript.transform;
        WaveTime = GameManager.gameManager.UiManager.PlayTime;

        if(BossTime > WaveTime)
        {
            if (NowWave < 1 && WaveTime > Wave1Time && Wave1 == false)
            {
                Wave1Init();
                Wave1 = true;
            }

            if (WaveTime > Wave2Time && Boss.activeSelf == false)
            {
                Wave2 = true;
                if (NowWave < 2)
                    NowWave = 2;

                if (SubTimer > 5f)
                    Wave2Init();
            }
            if (NowWave < 3 && WaveTime > Wave3Time && Wave3 == false)
            {
                Wave3Init();
                Wave3 = true;
            }
            if (NowWave < 4 && WaveTime > Wave4Time && Wave4 == false)
            {
                Wave4Init();
                Wave4 = true;
            }
            if (NowWave < 5 && WaveTime > Wave5Time && Wave5 == false)
            {
                Wave5Init();
                Wave5 = true;
            }
            if (Wave1 == true)
                curWaveTime += Time.deltaTime;
            if (Wave2 == true)
                SubTimer += Time.deltaTime;
            if (Wave5 == true && WaveTime < 240f)
            {
                LastTimer += Time.deltaTime;
                if (LastTimer >= 5f)
                    Wave5Init();
            }
        }
        
        if (WaveTime >= BossTime)
        {
            if(WaveMobList.Count > 0)
            {
                for (int i = 0; i < WaveMobList.Count; i++)
                    Pooling.instance.ReturnElement(WaveMobList[i]);
                WaveMobList.Clear();
            }
            if(Boss.activeSelf == false)
                Boss.SetActive(true);
        }
        if (WaveMobList.Count > 0)
        {
            for (int i = 0; i < WaveMobList.Count; i++)
            {
                if (Duration < curWaveTime)
                {
                    Pooling.instance.ReturnElement(WaveMobList[i]);
                }
            }
            WaveMobList.Clear();
        }
        else
            curWaveTime = 0;

        if (Boss.activeSelf == true)
        {
            BossTimer += Time.deltaTime;
            if (BossTimer > 6f)
                BossPattern();
        }
    }
    public void Wave1Init()
    {
        Transform center = playerpos;
        float radius = 25f;
        for (int i = 1; i <= 72; i++)
        {
            GameObject enemy = Pooling.instance.GetElement(this.gameObject);
            enemy.layer = 21;
            enemy.tag = "Enermy";
            enemy.SetActive(true);
            WaveMobList.Add(enemy);

            float rad = Mathf.Deg2Rad * (i * 5);
            float x = radius * Mathf.Sin(rad);
            float z = radius * Mathf.Cos(rad);

            enemy.transform.position = playerpos.position + new Vector3(x, 0, z);

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

    public void Wave2Init()
    {
        //체력이 약하고 빠른 몬스터들생성후 몇 초후 소멸
        Temppos = GameManager.gameManager.PlayerScript.transform;
        
        Transform temppos = GameManager.gameManager.spawner.SpawnPoints[Random.Range(1,5)].transform;       
        Speed = 15f;
        for(int i = 0; i < 15; i++)
        {
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
        }

        for(int i = 0; i < SubWaveList.Count; i++)
        {
            Rigidbody rigid = SubWaveList[i].GetComponent<Rigidbody>();
            rigid.velocity = new Vector3(Temppos.position.x - SubWaveList[i].transform.position.x,
                           0, Temppos.position.z - SubWaveList[i].transform.position.z).normalized
                           * Speed;
            SubWaveList[i].transform.LookAt(new Vector3(Temppos.position.x - SubWaveList[i].transform.position.z,
                           0, Temppos.position.z - SubWaveList[i].transform.position.z));
        }
        SubTimer = 0f;
        SubWaveList.Clear();
    }
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
    public void Wave4Init()
    {
        List<GameObject> Enemylist = new List<GameObject>();
        for(int i = 0; i < 120; i++)
        {
            Enemylist.Add(Pooling.instance.GetElement(this.gameObject));
            Enemylist[i].layer = 21;
            Enemylist[i].tag = "WaveEnemy";

            Enermy status = Enemylist[i].GetComponent<Enermy>();
            status.maxHp = WaveDatas[3].MaxHP;
            status.curHp = status.maxHp;
            status.MoveSpeed = WaveDatas[3].Speed;
            status.Damage = WaveDatas[3].Damage;
            status.MyWave = 4;
            Duration = WaveDatas[3].LifeTime;
        }
        StartCoroutine(Wave4Cor(Enemylist));
        NowWave = 4;
    }
    public void Wave5Init()
    {
        float radius = 25f;
        for(int i = 1; i <= 36; i++)
        {
            GameObject enemy = Pooling.instance.GetElement(this.gameObject);
            enemy.layer = 21;
            enemy.tag = "Enermy";
            enemy.SetActive(true);
            WaveMobList.Add(enemy);

            float rad = Mathf.Deg2Rad * (i * 10f);
            float x = radius * Mathf.Sin(rad);
            float z = radius * Mathf.Cos(rad);
            enemy.transform.position = playerpos.position + new Vector3(x, 0, z);

            Enermy status = enemy.GetComponent<Enermy>();
            status.maxHp = WaveDatas[4].MaxHP;
            status.curHp = status.maxHp;
            status.MoveSpeed = WaveDatas[4].Speed;
            status.Damage = WaveDatas[4].Damage;
            status.MyWave = 5;
            Duration = WaveDatas[4].LifeTime;
        }
        LastTimer = 0f;
        NowWave = 5;
    }
    public IEnumerator Wave4Cor(List<GameObject> list)
    {
        for (int i = 0; i < 30; i++)
        {
            list[i].SetActive(true);
            list[i].transform.position = new Vector3(playerpos.position.x - 30f, 0, playerpos.position.z - 50f + (i * 5f));
            list[i].transform.LookAt(new Vector3(9999f, 0, list[i].transform.position.z));           
        }
        list.RemoveRange(0, 30);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 30; i++)
        {
            list[i].SetActive(true);
            list[i].transform.position = new Vector3(playerpos.position.x + 28f, 0, playerpos.position.z - 50f + (i * 5f));
            list[i].transform.LookAt(new Vector3(-9999f, 0, list[i].transform.position.z));
        }
        list.RemoveRange(0, 30);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 30; i++)
        {
            list[i].SetActive(true);
            list[i].transform.position = new Vector3(playerpos.position.x - 50f + (i * 5f), 0, playerpos.position.z - 30f);
            list[i].transform.LookAt(new Vector3(list[i].transform.position.x, 0, 9999f));
        }
        list.RemoveRange(0, 30);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 30; i++)
        {
            list[i].SetActive(true);
            list[i].transform.position = new Vector3(playerpos.position.x - 48f + (i * 5f), 0, playerpos.position.z + 30f);
            list[i].transform.LookAt(new Vector3(list[i].transform.position.x, 0, -9999f));
        }
        list.RemoveRange(0, 30);
    }
    public void BossPattern()
    {
        
        for (int i = 0; i < 3; i++)
        {
            GameObject bossknight = Pooling.instance.GetElement(this.gameObject);
            bossknight.layer = 21;
            bossknight.tag = "Boss";
            bossknight.SetActive(true);
            bossknight.transform.position = Boss.transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

            Enermy status = bossknight.GetComponent<Enermy>();
            status.MoveSpeed = WaveDatas[4].Speed;
            status.maxHp = WaveDatas[4].MaxHP;
            status.curHp = status.maxHp;
            status.Damage = WaveDatas[4].Damage;
        }
        BossTimer = 0;
    }
}
[System.Serializable]
public class WaveData
{
    public float Speed;
    public float MaxHP;
    public float Damage;
    public float LifeTime;
}
