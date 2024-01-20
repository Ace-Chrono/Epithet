using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public static Dialogue instance;

    public GameObject levelText;
    public AudioSource GameMusic;
    public AudioSource introMusic;
    private bool TyperStarter = false;
    public TextMeshProUGUI monologueText;
    public GameObject enemy;
    public int monologueNum;
    bool endSkipBugPreventor = false;
    public TextMeshProUGUI skipText;
    IEnumerator co;
    
    public int didTutorialNum;
    private string didTutorial = "didTutorial";
    public GameObject tutorial;
    public GameObject monologueImage;

    public bool gameStarted = false;

    void Start()
    {
        introMusic.Play();
        if (monologueNum == 1)
            co = monologueOne(); 
        if (monologueNum == 2)
            co = monologueTwo();
        if (monologueNum == 3)
            co = monologueThree();

        didTutorial += didTutorialNum;
        if (!PlayerPrefs.HasKey(didTutorial))
        {
            monologueText.gameObject.SetActive(false);
            skipText.gameObject.SetActive(false); 
            monologueImage.SetActive(false);
            tutorial.SetActive(true);
            enemy.SetActive(false);
            PlayerPrefs.SetInt(didTutorial, 1);
        }
        else
            startMonologue();
    }

    public void startMonologue()
    {
        StartCoroutine(co);
    }

    private void Awake()
    {
        instance = this;
        //(Set music control false for testing)
        MusicControl.instance.GetComponent<AudioSource>().Stop();
        TransitionTriggers.instance.LoadTrigger(2, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && TyperStarter == false && endSkipBugPreventor == false)
        {
            StopCoroutine(co); 
            monologueText.gameObject.SetActive(false);
            introMusic.Stop();
            StartGame();
        }
    }

    IEnumerator monologueOne()
    {
        monologueText.text = "MACHINE, TURN BACK NOW";
        yield return new WaitForSeconds(3f);
        monologueText.text = "THIS PLANET IS NOT FOR YOUR TAKING";
        yield return new WaitForSeconds(3f);
        monologueText.text = "...";
        yield return new WaitForSeconds(3f);
        monologueText.text = "FINE, YOUR CHOICE IS MADE";
        yield return new WaitForSeconds(3f);
        monologueText.text = "I'LL MAKE YOU LEAVE AS A PIECE OF SCRAP";
        yield return new WaitForSeconds(3f);
        monologueText.gameObject.SetActive(false);
        introMusic.Stop();
        StartGame();
    }

    IEnumerator monologueTwo()
    {
        monologueText.text = "TO THINK YOU BEAT GABRIEL";
        yield return new WaitForSeconds(3f);
        monologueText.text = "I ALWAYS TOLD HIM HE WAS TOO RELIANT ON TECH";
        yield return new WaitForSeconds(3f);
        monologueText.text = "WELL, LETS NOT WASTE ANY MORE WORDS";
        yield return new WaitForSeconds(3f);
        monologueText.text = "COME, SHOW ME WHAT YOU HAVE";
        yield return new WaitForSeconds(3f);
        monologueText.gameObject.SetActive(false);
        introMusic.Stop();
        StartGame();
    }

    IEnumerator monologueThree()
    {
        monologueText.text = "IT SEEMS I AM THE ONLY ONE LEFT";
        yield return new WaitForSeconds(3f);
        monologueText.text = "MACHINE, YOU WILL LAY NO MORE WASTE TO MY WORLD";
        yield return new WaitForSeconds(3f);
        monologueText.text = "MY CREATION SHALL NOT FALL TO THE LIKES OF YOU";
        yield return new WaitForSeconds(3f);
        monologueText.gameObject.SetActive(false);
        introMusic.Stop();
        StartGame();
    }

    public void startEnd()
    {
        StartCoroutine(gameEnd()); 
    }

    IEnumerator gameEnd()
    {
        endSkipBugPreventor = true;
        SetTyperStarter(false);
        skipText.gameObject.SetActive(false); 
        GameMusic.Stop();
        levelText.gameObject.SetActive(false);
        monologueText.gameObject.SetActive(true);
        if (monologueNum == 1)
        {
            monologueText.text = "TO THINK A MERE MACHINE BEAT ME...";
            yield return new WaitForSeconds(3f);
        }
        if (monologueNum == 2)
        {
            monologueText.text = "IT LOOKS LIKE MY SKILLS WERE NOT ENOUGH...";
            yield return new WaitForSeconds(3f);
        }
        if (monologueNum == 3)
        {
            monologueText.text = "IM SORRY MY CHILDREN, I COULD NOT SAVE YOU...";
            yield return new WaitForSeconds(3f);
        }
        MusicControl.instance.GetComponent<AudioSource>().Play();
        TransitionTriggers.instance.LoadTrigger(1, 17);
    }

    void StartGame()
    {
        gameStarted = true;
        GameMusic.Play();
        skipText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(true);
        Stopwatch.instance.StartTimerToEnd();
        TyperStarter = true;
        PlayerHealth.instance.setAttacks(true); 
    }

    public void SetTyperStarter(bool a)
    {
        TyperStarter = a;
    }

    public bool GetTyperStarter()
    {
        return TyperStarter;
    }

    public void setMonologuePause()
    {
        Time.timeScale = 0;
    }

    public void breakMonologuePause()
    {
        Time.timeScale = 1;
    }

    public bool getGameStarted()
    {
        return gameStarted;
    }
}
