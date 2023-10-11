using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brid : MonoBehaviour
{
    public float CurTime;
    public float LoopTime;
    public float Delay;
    public float AttackRange;
    AudioSource Myaudio;
    void Start()
    {
        Myaudio = GetComponent<AudioSource>();
    }

    void Update()
    {
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
}
