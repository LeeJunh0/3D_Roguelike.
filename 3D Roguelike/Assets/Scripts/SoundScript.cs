using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    static SoundScript instance = null;
    AudioSource SoundSource;
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        SoundSource = GetComponent<AudioSource>();
        StartCoroutine(BackSoundStart());
    }

    IEnumerator BackSoundStart()
    {
        yield return new WaitForSeconds(1.5f);
        SoundSource.Play();
    }

}
