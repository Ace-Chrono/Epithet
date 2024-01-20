using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFXControl : MonoBehaviour
{
    public static ButtonFXControl instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
