using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChanger : MonoBehaviour
{
    public static PlayerChanger instance;

    [HideInInspector]
    public GameObject young;
    [HideInInspector]
    public GameObject mature;

    private PlayerChanger()
    {

    }

    public static PlayerChanger GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        young = transform.GetChild(0).gameObject;
        mature = transform.GetChild(1).gameObject;

        if(instance == null)
        {
            instance = this;
        }
    }
}
