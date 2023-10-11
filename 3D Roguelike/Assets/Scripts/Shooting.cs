using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject Armoprefab;
    public GameObject FirePos;
    public float time = 0;
    public float Shotfreeze;
    public float Speed;
    public AudioClip Shot_Sound;
    AudioSource Shot_Audio;
    int ShotCount;
    private void Start()
    {
        Shot_Audio = GetComponent<AudioSource>();
        ShotCount = GameManager.gameManager.ArmoCount;
    }
    void Update()
    {
        time += Time.deltaTime;

        if(Time.timeScale != 0)
            if (time > Shotfreeze)
                ArmoCreat();
    }
    
    void ArmoCreat()
    {
        GameObject armo = Pooling.instance.GetElement(this.gameObject);
        Armo armo_set = armo.GetComponent<Armo>();
        armo_set.ArmoPenetration = GameManager.gameManager.Penetration;
        armo.transform.position = FirePos.transform.position;
        armo.SetActive(true);
        Rigidbody armoRigid = armo.GetComponent<Rigidbody>();
        armoRigid.velocity = FirePos.transform.forward * Speed;

        Shot_Audio.Stop();
        Shot_Audio.PlayOneShot(Shot_Sound);

        time = 0f;
    }

}
