using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float RegenerateTime;
    public float curTime;
    public int ShieldLife;

    private void Start()
    {
        ShieldLife = SkillManager.skillManager.ShieldHitCount;
    }
    void Update()
    {
        RegenerateTime = SkillManager.skillManager.regenerateTime;

        if (ShieldLife > 0) return;
        else
            transform.GetChild(0).gameObject.SetActive(false);

        if(transform.GetChild(0).gameObject.activeSelf == false)
        {
            curTime += Time.deltaTime;
            if (curTime >= RegenerateTime)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                ShieldLife = SkillManager.skillManager.ShieldHitCount;
                curTime = 0f;
            }
                
        }
    }
}
