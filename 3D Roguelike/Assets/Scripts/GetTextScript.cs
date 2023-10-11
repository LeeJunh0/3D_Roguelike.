using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetTextScript : MonoBehaviour
{
    public TextMeshPro text;
    float MoveSpeed;
    float AlphaSpeed;
    public Color Alpha;
    public float time;
    
    void Start()
    {
        MoveSpeed = 1f;
        AlphaSpeed = 3f;
        text = GetComponent<TextMeshPro>();
        Alpha = text.color;
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.Translate(new Vector3(0, MoveSpeed * Time.deltaTime, 0));
        Alpha.a = Mathf.Lerp(Alpha.a, 0, Time.deltaTime * AlphaSpeed);
        text.color = Alpha;
        time += Time.deltaTime;
        if (time >= 1f)
        {
            time = 0f;
            Pooling.instance.ReturnTextMesh(this.gameObject);            
        }
    }
}
