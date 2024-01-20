using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Diagnostics;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        instance = this; 
    }

    [SerializeField]
    private TextAsset textAssetWords;
    [SerializeField]
    private string[] wordsList;
    public TextMeshProUGUI requiredLetterOutput = null;
    public TextMeshProUGUI wordOutput = null;
    public TextMeshProUGUI message = null;
    private List<int> messageHistory = new List<int>(); 
    public int numLetters;

    private string outputWord = string.Empty;

    private List<KeyCode> validKeyCodes = new List<KeyCode>
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
    };

    //private string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    private string[] alphabet10 = { "a", "e", "t"}; //24
    private string[] alphabet15 = { "d", "h", "i", "n", "o", "r", "s" }; //49
    private string[] alphabet25 = { "c", "f", "g", "l", "m", "u", "w", "y" }; //48
    private string[] alphabet30 = { "b", "j", "k", "p", "q", "v", "x", "z" }; //40
    private List<string> requiredLetters = new List<string>();
    private string requiredLettersString;

    private List<string> submittedWords = new List<string>();

    public System.DateTime startTime;
    private bool swStarted = false;
    private bool comboBool = false;
    private List<bool> comboHistory = new List<bool>();
    int comboNum = 0;

    public AudioSource EnterFX;
    public AudioSource ComboFX; 

    private bool gameLost = true;

    public int numBoss;

    private void Start()
    {
        ReadTextAsset();
        CreateRequiredLetters();
    }

    private void ReadTextAsset()
    {
        wordsList = textAssetWords.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < wordsList.Length; i++)
        {
            wordsList[i] = wordsList[i].Trim();
        }
    }

    private void CreateRequiredLetters()
    {
        for (int x = 0; x < numLetters; x++)
        {
            int chance = UnityEngine.Random.Range(0, 160);
            if (chance < 24)
            {
                string adder = alphabet10[UnityEngine.Random.Range(0, alphabet10.Length)];
                requiredLetters.Add(adder);
                if (x != (numLetters - 1))
                    requiredLettersString += adder.ToUpper() + ", ";
                else
                    requiredLettersString += adder.ToUpper() + ".";
            }
            else if (chance < 73)
            {
                string adder = alphabet15[UnityEngine.Random.Range(0, alphabet15.Length)];
                requiredLetters.Add(adder);
                if (x != (numLetters - 1))
                    requiredLettersString += adder.ToUpper() + ", ";
                else
                    requiredLettersString += adder.ToUpper() + ".";
            }
            else if (chance < 121)
            {
                string adder = alphabet25[UnityEngine.Random.Range(0, alphabet25.Length)];
                requiredLetters.Add(adder);
                if (x != (numLetters - 1))
                    requiredLettersString += adder.ToUpper() + ", ";
                else
                    requiredLettersString += adder.ToUpper() + ".";
            }
            else if (chance < 161)
            {
                string adder = alphabet30[UnityEngine.Random.Range(0, alphabet30.Length)];
                requiredLetters.Add(adder);
                if (x != (numLetters - 1))
                    requiredLettersString += adder.ToUpper() + ", ";
                else
                    requiredLettersString += adder.ToUpper() + ".";
            }
        }

        if (requiredLetterOutput != null)
        {
            requiredLetterOutput.text = "REQUIRED LETTER(S): " + requiredLettersString;
        }
    }

    private void ClearRequiredLetters()
    {
        requiredLettersString = "";
        requiredLetters.Clear();
    }

    private int BinarySearch(string[] array, string word)
    {
        int low = 0;
        int high = array.Length - 1;
        int mid;

        while (low <= high)
        {
            mid = (low + high) / 2;

            if (array[mid].CompareTo(word) < 0)
                low = mid + 1;
            else if (array[mid].CompareTo(word) > 0)
                high = mid - 1;
            else
                return mid;
        }

        return -1;
    }

    private void Update()
    {
        if (Dialogue.instance.GetTyperStarter() == true)
            CheckInput();
    }

    private void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
            {
                if (string.IsNullOrEmpty(outputWord) == false)
                {
                    outputWord = outputWord.Remove(outputWord.Length - 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                SubmitWord();
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                SkipWord();
            }
            else
            {
                foreach (KeyCode keyCode in validKeyCodes)
                    if (Input.GetKeyDown(keyCode))
                    {
                        outputWord = outputWord + keyCode.ToString().ToLower();
                    }
            }
            wordOutput.text = outputWord.ToUpper();
        }
    }

    private void SkipWord()
    {
        PlayerHealth.instance.removeHealth(5);
        ClearRequiredLetters();
        CreateRequiredLetters();
        message.text = "WORD SKIPPED, -5 HEALTH";
        messageHistory.Add(1);
        StartCoroutine(messageChecker()); 
    }

    private bool ContainsLetters(string word, List<string> letters)
    {
        int startCheckAt = 0;
        bool decider = true;
        for (int x = 0; x < letters.Count; x++)
        {
            if (word.IndexOf(letters[x], startCheckAt) < 0)
            {
                decider = false;
            }
            if (!(x == letters.Count - 1))
            {
                if (letters[x] == letters[x + 1])
                {
                    startCheckAt = word.IndexOf(letters[x], startCheckAt) + 1;
                }
            }
        }
        return decider;
    }

    private bool NotRepeating()
    {
        bool decider = true;
        for (int x = 0; x < submittedWords.Count; x++)
        {
            if (outputWord.Equals(submittedWords[x]))
            {
                decider = false;
            }
        }
        return decider;
    }

    private void SubmitWord()
    {
        UnityEngine.Debug.Log(BinarySearch(wordsList, outputWord));
        if (BinarySearch(wordsList, outputWord) > -1 && ContainsLetters(outputWord, requiredLetters) && NotRepeating())
        {
            if (swStarted == true)
            {
                System.TimeSpan ts = System.DateTime.UtcNow - startTime;
                swStarted = false;
                double timeEl = ts.TotalMilliseconds;
                UnityEngine.Debug.Log("Time " + timeEl);
                if (timeEl <= 2500)
                {
                    UnityEngine.Debug.Log("Combo");
                    comboBool = true;
                }
                else
                {
                    comboBool = false;
                }
                comboHistory.Add(comboBool);
                comboNum++;
            }
            if (swStarted == false)
            {
                startTime = System.DateTime.UtcNow;
                swStarted = true;
            }


            submittedWords.Add(outputWord);
            BossHealth.instance.takeDamage(outputWord); 
            PlayerHealth.instance.addHealth(comboDecider()); 


            if (comboBool == true)
            {
                message.text = "HEALTH + " + comboDecider();
                messageHistory.Add(2);
                StartCoroutine(messageChecker());
                ComboFX.Play();
            }
            else
            {
                EnterFX.Play();
            }


            outputWord = string.Empty;
            ClearRequiredLetters();
            CreateRequiredLetters();


            if (BossHealth.instance.bossHealth <= 0)
            {
                gameLost = false;
                if (PlayerPrefs.GetInt("BossesBeat", 0) < numBoss)
                    PlayerPrefs.SetInt("BossesBeat", numBoss); 
                Stopwatch.instance.setGameEnded(true);
                if (Stopwatch.instance.getTimerTime() < PlayerPrefs.GetInt(ScoreController.instance.getHighscoreString(), 1000))
                {
                    ScoreController.instance.setNewHighscore(true);
                    ScoreController.instance.SetHsToAdd(Stopwatch.instance.getTimerTime());
                    ScoreController.instance.AddHighscoreEntry();
                }
                else
                {
                    ScoreController.instance.AddOtherScore();
                }
                Dialogue.instance.startEnd();  
            }
        }

        else
        {
            if (outputWord == "" || outputWord == string.Empty || outputWord == null)
            {
                message.text = "YOU DONT HAVE A WORD TYPED";
            }
            else if (NotRepeating() == false)
            {
                message.text = "REPEATING WORD";
            }
            else if (BinarySearch(wordsList, outputWord) < 0)
            {
                message.text = "WORD DOES NOT EXIST";
            }
            else if (!ContainsLetters(outputWord, requiredLetters))
            {
                message.text = "WORD DOESN'T FIT REQUIREMENTS"; 
            }
            messageHistory.Add(3);
            StartCoroutine(messageChecker());
        }
    }

    private int comboDecider()
    {
        bool threeInRow = false, sixInRow = false;
        if (comboBool == true)
        {
            if (comboNum >= 3)
            {
                threeInRow = true;
                for (int x = comboNum - 1; x > comboNum - 4; x--)
                {
                    if (comboHistory[x] == false)
                    {
                        threeInRow = false;
                    }
                }
            }

            if (comboNum >= 6)
            {
                sixInRow = true;
                for (int x = comboNum - 1; x > comboNum - 7; x--)
                {
                    if (comboHistory[x] == false)
                    {
                        sixInRow = false;
                    }
                }
            }

            if (sixInRow == true)
            {
                return outputWord.Length * 4;
            }
            else if (threeInRow == true)
            {
                return outputWord.Length * 3;
            }
            else
            {
                return outputWord.Length * 2;
            }
        }

        else
        {
            return 0;
        }
    }

    IEnumerator messageChecker()
    {
        int inSize = messageHistory.Count;
        yield return new WaitForSeconds(1f);
        if (inSize == messageHistory.Count)
        {
            message.text = ""; 
        }
    }

    public bool getGameLost()
    {
        return gameLost; 
    }
}