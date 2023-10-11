using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armo : MonoBehaviour
{
    public float Rotation = 0f;
    public float Damage;
    public float LifeTime;
    public float Radius = 0f;
    public int ArmoPenetration;

    public int layerMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
            transform.Rotate(0, Rotation, Rotation);

        LifeTime += Time.deltaTime;
        if (LifeTime >= 2f)
        {
            Pooling.instance.ReturnElement(this.gameObject);
            LifeTime = 0f;
        }            
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enermy") return;
        if(ArmoPenetration > 0)
            ArmoPenetration--;       
        else
            Pooling.instance.ReturnElement(this.gameObject);
    }
}
