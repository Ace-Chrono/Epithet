using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public static MusicControl instance; //Remember that this is static, applies to all instances

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //Singleton Pattern
        if (instance == null) 
        {
            instance = this; 
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
