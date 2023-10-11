using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public VariableJoystick MoveJoy;
    public VariableJoystick RotationJoy;
    public GameObject Area;
    Rigidbody PlayerRigid;
    public float HorizontalAxis;
    public float VerticalAxis;
    public Vector3 MoveVec;
    
    public float PlayerSpeed = 0f;
    public float InvTime;
    public float MaxHp;
    public float curHp;

    public int GetCoin;
    public int isLevel;
    public float curExp = 0.1f;
    public float MaxExp;

    public int Health_Level;
    public int Power_Level;
    public int Speed_Level;
    public int Penetration_Level;
    public int Boom_Level;

    public AudioClip LevelUp_Sound;
    AudioSource Player_Audio;

    public GameObject Text_Prefab;
    GameObject text_Object;
    GetTextScript tempText;
    TextMeshPro Tmptext;

    Animator anim;
    void Start()
    {
        MaxHp = DataController.instance.playerData.MaxHp;
        curHp = MaxHp;
        isLevel = 1;
        curExp = 99f;
        MaxExp = 100f;
        PlayerRigid = GetComponent<Rigidbody>();
        Player_Audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Freeze()
    {
        PlayerRigid.velocity = Vector3.zero;
        PlayerRigid.angularVelocity = Vector3.zero;
    }
    void Update()
    {
        //HorizontalAxis = MoveJoy.Horizontal;
        //VerticalAxis = MoveJoy.Vertical;
        HorizontalAxis = Input.GetAxisRaw("Horizontal");
        VerticalAxis = Input.GetAxisRaw("Vertical");
        // transform.forward = new Vector3(RotationJoy.Horizontal, 0, RotationJoy.Vertical);

        MoveVec = new Vector3(HorizontalAxis, 0, VerticalAxis).normalized;
        Area.transform.position = transform.position;
        transform.position += MoveVec * PlayerSpeed * Time.deltaTime;
        if (curExp >= MaxExp)
            LevelUp();

        MaxHp = DataController.instance.playerData.MaxHp + (Health_Level * 5);
    }
    
    private void FixedUpdate()
    {
        Freeze();
    }
    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.layer == 8 || other.gameObject.layer == 21) && gameObject.layer == 6) // 몹 피격
        {
            Enermy enermy = other.GetComponent<Enermy>();            
            curHp -= enermy.Damage;

            StartCoroutine(PlayerOnDamage());
        }
        if(other.gameObject.layer == 13) // 아이템
        {
            Item item = other.GetComponent<Item>();
            Player_Audio.PlayOneShot(item.Sound);
            float tempExp = item.Exp + (isLevel * 1.5f);
            curExp += tempExp;
            Healing(item.HP_healing);
            text_Object = Pooling.instance.GetTextMesh();
            text_Object.transform.position = new Vector3(transform.position.x, 2, transform.position.z);
            tempText = text_Object.GetComponent<GetTextScript>();
            Tmptext = text_Object.GetComponent<TextMeshPro>();
            switch (item.I_type)
            {
                case Item.ItemType.EXP:
                    tempText.Alpha = new Color(150f / 255f, 255f / 255f, 255f / 255f);
                    Tmptext.text = string.Format("{0}EXP", (int)tempExp);                    
                    break;
                case Item.ItemType.HEART:
                    tempText.Alpha = new Color(255f / 255f, 125f / 255f, 125f / 255f);
                    Tmptext.text = string.Format("{0}", (int)(item.HP_healing));                    
                    break;
            }
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
    void LevelUp()
    {
        isLevel++;
        curExp = 0f;
        MaxExp = 100 * isLevel;
        MaxHp += 10f;
        Healing(20f);
        Player_Audio.PlayOneShot(LevelUp_Sound);
        GameManager.gameManager.UiManager.SelectUI_Init();
    }
    void Healing(float heal)
    {
        if (heal == 0f)
            return;
        else if (curHp + heal < MaxHp)
            curHp += heal;
        else if (curHp + heal >= MaxHp)
            curHp = MaxHp;       
    }
    IEnumerator PlayerOnDamage()
    {        
        gameObject.layer = 10;
        anim.SetTrigger("isDamage");
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            yield return null;

        if (curHp > 0)
        {
            gameObject.layer = 6;
            anim.SetTrigger("isDamage");
        } 
    }
}
