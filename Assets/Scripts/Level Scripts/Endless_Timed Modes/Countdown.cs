using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Countdown : MonoBehaviour
{
    public static Countdown instance;  

    public int countdownTime;
    [SerializeField]
    private TextMeshProUGUI countdownText;
    public GameObject levelText;
    public AudioSource GameMusic;

    private bool TyperStarter = false;

    public GameObject firstTutorial;
    public int didTutorialNum;
    private string didTutorial = "didTutorial";

    void Start()
    {
        didTutorial += didTutorialNum; 
        if (!PlayerPrefs.HasKey(didTutorial))
        {
            countdownText.gameObject.SetActive(false);
            firstTutorial.SetActive(true);
            PlayerPrefs.SetInt(didTutorial, 1); 
        }
        else
            StartCoroutine(CountdownToStart()); 
    }

    public void startCountdown()
    {
        StartCoroutine(CountdownToStart());
    }

    private void Awake()
    {
        instance = this;
        MusicControl.instance.GetComponent<AudioSource>().Stop();
        GameMusic.Play();
        TransitionTriggers.instance.LoadTrigger(2,0);
    }

    IEnumerator CountdownToStart()
    {
        while(countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--; 
        }

        countdownText.text = "TYPE!";

        yield return new WaitForSeconds(1f); 

        countdownText.gameObject.SetActive(false);

        StartGame(); 
    }

    void StartGame()
    {
        levelText.gameObject.SetActive(true); 
        Timer.instance.StartCountdownToEnd();
        TyperStarter = true;
    }

    public void SetTyperStarter (bool a)
    {
        TyperStarter = a; 
    }

    public bool GetTyperStarter()
    {
        return TyperStarter; 
    }

    public void stopCountdown()
    {
        Time.timeScale = 0;
    }

    public void restartCountdown()
    {
        Time.timeScale = 1;
    }
}
