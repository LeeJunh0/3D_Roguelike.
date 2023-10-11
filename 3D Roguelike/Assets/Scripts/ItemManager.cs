using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> Prefab_ItemList;

    static public ItemManager itemManager = null;

    private void Start()
    {
        if (itemManager == null)
            itemManager = this;
        else
            Destroy(this);
    }
}
