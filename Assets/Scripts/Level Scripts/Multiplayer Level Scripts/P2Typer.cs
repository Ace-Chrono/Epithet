using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Diagnostics;
using Photon.Pun;

public class P2Typer : MonoBehaviour
{
    public static P2Typer instance;

    [SerializeField]
    private TextAsset textAssetWords;
    [SerializeField]
    private string[] wordsList;
    public GameObject player;
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
    private string[] alphabet10 = { "a", "e", "t" };
    private string[] alphabet15 = { "d", "h", "i", "n", "o", "r", "s" };
    private string[] alphabet25 = { "c", "f", "g", "l", "m", "u", "w", "y" };
    private string[] alphabet30 = { "b", "j", "k", "p", "q", "v", "x", "z" };
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

    public int pointGoal;
    PhotonView view;
    private bool startedGame = false;
    private string winStatus = "LOST";
    string randomUsername;

    private void Start()
    {
        randomUsername = UnityEngine.Random.Range(0, 10000000).ToString();
        player.gameObject.SetActive(false); 
        ReadTextAsset();
    }

    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();
    }

    public void syncOutputText(string t)
    {
        view.RPC("updateOutputText", RpcTarget.All, t);
    }

    [PunRPC]
    void updateOutputText(string t)
    {
        wordOutput.text = t.ToUpper();
    }

    public void syncMessageText(string t)
    {
        view.RPC("updateMessageText", RpcTarget.All, t);
    }

    [PunRPC]
    void updateMessageText(string t)
    {
        message.text = t.ToUpper();
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
    }

    public void syncLetterText(string t)
    {
        view.RPC("updateLetterText", RpcTarget.All, t);
    }

    [PunRPC]
    void updateLetterText(string t)
    {
        requiredLetterOutput.text = "REQUIRED LETTER(S): " + t;
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
        if (SceneManagerMultiplayer.instance.GetTyperStarter() == true && startedGame == false)
        {
            player.gameObject.SetActive(true);
            CreateRequiredLetters();
            startedGame = true;
        }
        if (SceneManagerMultiplayer.instance.GetTyperStarter() == true && view.IsMine)
        {
            CheckInput();
            syncLetterText(requiredLettersString);
        }
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
            syncOutputText(outputWord); 
        }
    }

    private void SkipWord()
    {
        P2ScoreManager.instance.RemovePoints(25);
        ClearRequiredLetters();
        CreateRequiredLetters();
        syncMessageText("WORD SKIPPED, -25 POINTS");
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
            P2ScoreManager.instance.AddPoint(outputWord, comboDecider());


            if (comboBool == true)
            {
                syncMessageText("COMBO + " + comboDecider());
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
        }

        else
        {
            if (outputWord == "" || outputWord == string.Empty || outputWord == null)
            {
                syncMessageText("YOU DONT HAVE A WORD TYPED");
            }
            else if (NotRepeating() == false)
            {
                syncMessageText("REPEATING WORD");
            }
            else if (BinarySearch(wordsList, outputWord) < 0)
            {
                syncMessageText("WORD DOES NOT EXIST");
            }
            else if (!ContainsLetters(outputWord, requiredLetters))
            {
                syncMessageText("WORD DOESN'T FIT REQUIREMENTS");
            }
            messageHistory.Add(3);
            StartCoroutine(messageChecker());
        }
    }

    public void syncWinStatus()
    {
        view.RPC("winStatusSet", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void winStatusSet()
    {
        winStatus = "WON";
    }

    public void syncGameEnded()
    {
        view.RPC("gameEndedSync", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void gameEndedSync()
    {
        SceneManagerMultiplayer.instance.setGameEnded(true);
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
                if (outputWord.Length <= 4 && outputWord.Length > 0)
                {
                    return outputWord.Length * 10;
                }
                else if (outputWord.Length <= 6)
                {
                    return outputWord.Length * 20;
                }
                else if (outputWord.Length >= 7)
                {
                    return outputWord.Length * 30;
                }
                else
                {
                    return 0;
                }
            }
            else if (threeInRow == true)
            {
                if (outputWord.Length <= 4 && outputWord.Length > 0)
                {
                    return outputWord.Length * 5;
                }
                else if (outputWord.Length <= 6)
                {
                    return outputWord.Length * 10;
                }
                else if (outputWord.Length >= 7)
                {
                    return outputWord.Length * 15;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (outputWord.Length <= 4 && outputWord.Length > 0)
                {
                    return outputWord.Length * 3;
                }
                else if (outputWord.Length <= 6)
                {
                    return outputWord.Length * 5;
                }
                else if (outputWord.Length >= 7)
                {
                    return outputWord.Length * 10;
                }
                else
                {
                    return 0;
                }
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
            syncMessageText("");
        }
    }

    public string returnWinStatus()
    {
        return winStatus;
    }
}
