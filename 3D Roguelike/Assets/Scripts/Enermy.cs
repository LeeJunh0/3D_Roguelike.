using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
public class Enermy : MonoBehaviour
{
    public enum MobType { NORMAL, BULLET, BOSS };
    public MobType EnermyType;

    Rigidbody EnermyRigid;
    public Transform PlayerPos;
    public GameObject PrefabBullet;
    public GameObject PrefabBossBullet;
    public Transform Firepos;
    AudioSource Enermy_Audio;
    public AudioClip HitSound;
    public AudioClip BossDead_Sound;
    Animator anim;

    public float distance;
    public float maxHp;
    public float curHp;
    public float Damage;
    public float MoveSpeed;
    public float BulletSpeed;
    public float BulletDeley;
    public float time;

    public float WaveLifeTime;
    public int MyWave;
    Vector3 Look;

    GameObject Text_Object;
    TextMeshPro Text_mesh;
    private void Start()
    {
        Enermy_Audio = GetComponent<AudioSource>();
        EnermyRigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        PlayerPos = GameManager.gameManager.PlayerScript.transform;
        curHp = maxHp;
        time = SkillManager.skillManager.DamageTime;
    }
    private void Update()
    {
        time += Time.deltaTime * (SkillManager.skillManager.Skill_Inventory[1].Level * 0.5f);
        if(gameObject.tag == "WaveEnemy")
        {
            WaveLifeTime += Time.deltaTime;
            switch(MyWave)
            {
                case 3:
                    transform.position = new Vector3(transform.position.x + Time.deltaTime * MoveSpeed, transform.position.y, transform.position.z);
                    transform.LookAt(new Vector3(9999f, 0, transform.position.z));
                    break;
                case 4:
                    if (transform.forward.x > 0)
                        transform.position = new Vector3(transform.position.x + Time.deltaTime * MoveSpeed, transform.position.y, transform.position.z);
                    if (transform.forward.x < 0)
                        transform.position = new Vector3(transform.position.x - Time.deltaTime * MoveSpeed, transform.position.y, transform.position.z);
                    if (transform.forward.z > 0)
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime * MoveSpeed);
                    if (transform.forward.z < 0)
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * MoveSpeed);
                    break;
            }    
            if (WaveLifeTime >= GameManager.gameManager.wavemanager.WaveDatas[GameManager.gameManager.wavemanager.NowWave - 1].LifeTime)
            {
                Pooling.instance.ReturnElement(this.gameObject);
            }

        }

        if(GameManager.gameManager.wavemanager.BossTime <= GameManager.gameManager.UiManager.PlayTime && gameObject.tag != "Boss")        
            Pooling.instance.ReturnElement(this.gameObject);        
    }
    private void FixedUpdate()
    {
        //플레이어 추적

        if (gameObject.tag == "Enermy" || gameObject.tag == "Boss")
        {
            Look = new Vector3(PlayerPos.position.x, transform.position.y, PlayerPos.position.z);
            transform.LookAt(Look);
        }
        if (gameObject.tag == "Enermy" || gameObject.tag == "Boss")
        {
            transform.position += new Vector3(PlayerPos.position.x - transform.position.x, 0,
                                              PlayerPos.position.z - transform.position.z).normalized
                                              * MoveSpeed * Time.deltaTime;
            Freeze();
        }
    }

    void Freeze()
    {
        EnermyRigid.velocity = Vector3.zero;
        EnermyRigid.angularVelocity = Vector3.zero;
    }
    public void TextInit(float damage, Transform pos)
    {
        Text_Object = Pooling.instance.GetTextMesh();
        Text_Object.transform.position = new Vector3(pos.position.x, 2, pos.position.z);
        Text_mesh = Text_Object.GetComponent<TextMeshPro>();
        GetTextScript temp_Color = Text_Object.GetComponent<GetTextScript>();
        Text_mesh.text = string.Format("{0}", (int)damage);
        temp_Color.Alpha = new Color(255f / 255f, 255f / 255f, 255f / 255f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 16 || other.tag == "Armo") && (gameObject.layer == 8 || gameObject.layer == 21))
        {
            if (other.gameObject.layer == 16)
            {
                TextInit(SkillManager.skillManager.Skill_Inventory[0].ATT,transform);
                StartCoroutine(EnermyDamage(SkillManager.skillManager.Skill_Inventory[0].ATT));
                Shield shield = other.transform.parent.GetComponent<Shield>();
                shield.ShieldLife--;
            }
            if(other.tag == "Armo")
            {
                TextInit(GameManager.gameManager.BulletDamage,transform);               
                StartCoroutine(EnermyDamage(GameManager.gameManager.BulletDamage));
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {       
        if(other.gameObject.layer == 17 && (gameObject.layer == 8 || gameObject.layer == 21))
        {
            if (SkillManager.skillManager.Skill_Inventory[1].Level >= 5)
            {
                float sec = SkillManager.skillManager.Skill_Inventory[1].Debuffsec;
                float value = SkillManager.skillManager.Skill_Inventory[1].DebuffValue;
                StartCoroutine(Debuff(value, sec));
            }
            if (time >= SkillManager.skillManager.DamageTime)
            {
                TextInit(SkillManager.skillManager.Skill_Inventory[1].ATT, transform);
                StartCoroutine(EnermyDamage(SkillManager.skillManager.Skill_Inventory[1].ATT));                
                time = 0;
            }            
        }
    }
    int RandomWeight()
    {        
        int total = 0;
        int totalWeight = 0;
        int ChoiceItem = 0;
        int[] ItemWeight = { 40, 30, 20, 10 };

        for (int i = 0; i < ItemWeight.Length; i++)
        {
            total += ItemWeight[i];
        }
               
        int RandomValue = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));
        for(int i = 0; i < ItemWeight.Length; i++)
        {
            totalWeight += ItemWeight[i];
            if(totalWeight >= RandomValue)
            {
                ChoiceItem = i;
                break;
            }
        }
        return ChoiceItem;
    }

    void Dropitem()
    {
        Vector3 Pos = new Vector3(transform.position.x, 0f, transform.position.z);
        GameObject Item = Instantiate(ItemManager.itemManager.Prefab_ItemList[RandomWeight()], Pos, this.transform.rotation);
    }

    public void KnockBack()
    {
        Vector3 HitBackVec = new Vector3(transform.position.x - PlayerPos.position.x, 0, transform.position.z - PlayerPos.position.z);
        EnermyRigid.AddForce(HitBackVec.normalized + new Vector3(0, 0.2f, 0) * 10f,ForceMode.Impulse);
    }

    public IEnumerator Debuff(float value, float sec)
    {
        float tempspeed = MoveSpeed;
        MoveSpeed = tempspeed * 0.2f;

        yield return new WaitForSeconds(sec);

        MoveSpeed = tempspeed;
    }

    public IEnumerator EnermyDamage(float Damage)
    { 
        curHp -= Damage;
        Enermy_Audio.Stop();
        Enermy_Audio.PlayOneShot(HitSound);
        anim.SetTrigger("isDamage");
        KnockBack();
        if (curHp <= 0)
            gameObject.layer = 9;
               
            
        yield return new WaitForSeconds(0.2f);
      
        if (gameObject.layer != 9)
        {
            anim.SetTrigger("isDamage");
            yield break;
        }
        GameManager.gameManager.KillCount++;
        Dropitem();
        Pooling.instance.ReturnElement(this.gameObject);
    }
}
