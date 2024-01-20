using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionTriggers : MonoBehaviour
{
    public static TransitionTriggers instance; 
    
    public Animator transition;

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

    public void LoadTrigger(int num, int Scene_Num)
    {
        if (num == 1)
            StartCoroutine(Trigger1(Scene_Num));
        else if (num == 2)
            StartCoroutine(Trigger2());
    }

    IEnumerator Trigger1(int Scene_Num)
    {
        transition.SetTrigger("Start_End");
        Debug.Log("T1");
        yield return new WaitForSeconds(1f);
        transition.SetTrigger("End_Reset");
        SceneManager.LoadScene(Scene_Num);
    }

    IEnumerator Trigger2()
    {
        transition.SetTrigger("Start_Start");
        Debug.Log("T2");
        yield return new WaitForSeconds(1f);
        transition.SetTrigger("Start_Reset");
    }
}
