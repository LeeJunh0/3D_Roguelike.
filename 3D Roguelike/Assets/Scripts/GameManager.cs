using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager = null;
    public UIManager UiManager;
    public WaveManager wavemanager;
    public Spawner spawner;
    //플레이어 관련
    public PlayerController PlayerScript;
    public float InvincibilityTime;
    public float PlayerMoveSpeed;
    public float playercurHP;
    public float playerMaxHP;

    public int Health = 0;
    public int BoomArmo = 0;
    public int Penetration = 0;
    public int AttackSpeed = 0;
    public int AttackDamage = 0;

    //탄환 관련
    public Armo ArmoScript;
    public float ArmoRotation;
    public float ArmoSpeed;
    public float BulletDamage;
    public float BoomRadius = 2f;
    public int ArmoCount = 1;

    //총 관련
    public Shooting ShootingScript;    
    public float Gunreload = 1f;

    //리스포너 관련
    public Spawner SpawnerScript;

    public int KillCount;

    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(this);
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayerMoveSpeed = DataController.instance.playerData.PlayerSpeed;
        KillCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerScript != null)
        {
            PlayerScript.InvTime = InvincibilityTime;
            PlayerScript.PlayerSpeed = PlayerMoveSpeed;
            playerMaxHP = PlayerScript.MaxHp;
            playercurHP = PlayerScript.curHp;
            PlayerScript.Health_Level = Health;
            PlayerScript.Boom_Level = BoomArmo;
            PlayerScript.Penetration_Level = Penetration;
            PlayerScript.Power_Level = AttackDamage;
            PlayerScript.Speed_Level = AttackSpeed;
            if(DataController.instance.playerData.GunSpeed - (AttackSpeed * 0.10f) > 0)
                Gunreload = DataController.instance.playerData.GunSpeed - (AttackSpeed * 0.10f);
            if (PlayerScript.Power_Level >= 0)
                BulletDamage = (int)DataController.instance.playerData.Bullet_Damage + (AttackDamage * 5) + PlayerScript.isLevel;
        }        
        if (ArmoScript != null)
        {
            ArmoScript.Rotation = ArmoRotation;
            ArmoScript.Radius = BoomRadius;
            ArmoScript.Damage = BulletDamage;
            if (BoomArmo > 0)
                BoomRadius = 2 + BoomArmo * 1.5f;
        }       
        if (ShootingScript != null)
        {
            ShootingScript.Speed = ArmoSpeed;
            ShootingScript.Shotfreeze = Gunreload;
        }

    }
}
