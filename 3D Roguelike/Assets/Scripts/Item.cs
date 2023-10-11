using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { EXP , HEART};
    public ItemType I_type;
    public enum ExpType { BLUE , YELLOW , RED };
    public ExpType E_type;
    public float HP_healing;
    public float Exp;
    public AudioClip Sound;

    private void Start()
    {
        
    }
    private void Update()
    {
        transform.Rotate(0, 0.2f, 0);
        switch (I_type)
        {
            case ItemType.EXP:
                switch (E_type)
                {
                    case ExpType.BLUE:
                        Exp = 10f;
                        HP_healing = 0f;
                        break;
                    case ExpType.YELLOW:
                        Exp = 25f;
                        HP_healing = 0f;
                        break;
                    case ExpType.RED:
                        Exp = 50f;
                        HP_healing = 0f;
                        break;
                }
                break;
            case ItemType.HEART:
                Exp = 0f;
                HP_healing = 15;
                break;
        }
    }
}
