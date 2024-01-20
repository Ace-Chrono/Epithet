using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Diagnostics; 

public class Typer : MonoBehaviour
{
    public static Typer instance; 

    [SerializeField]
    private TextAsset textAssetWords;
    [SerializeField]
    private string[] wordsList;
    public TextMeshProUGUI requiredLetterOutput = null; 
    public TextMeshProUGUI wordOutput = null;
    public TextMeshProUGUI message = null;
    private List<int> messageHistory = new List<int>();
    public int numLetters;
    public int timeAdded; 

    private string outputWord = string.Empty;

    private List<KeyCode> validKeyCodes = new List<KeyCode>
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
    };

    //private string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
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

    private void Awake()
    {
        instance = this; 
    }

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

    private void CreateRequiredLetters() //Creates the letter requirements that words must fit.
    {
        //The number of letters created are determined by the value "numLetters"
        for (int x = 0; x < numLetters; x++)
        {
            //A random number from 0-160 is chosen to decide which list of letters to choose from
            int chance = UnityEngine.Random.Range(0, 160); 
            //Letters are sorted into lists by rarity, which affects the amount of points they give. 
            if (chance < 24)
            {
                //Letters extremely common are given 10 points, and a chance value of 8/160
                string adder = alphabet10[UnityEngine.Random.Range(0, alphabet10.Length)];
                requiredLetters.Add(adder);
                //This if statement adds the letter and a period or comma depending on if it is the last letter we need to use. This text is put on a UI element. 
                if (x != (numLetters - 1))
                    requiredLettersString += adder.ToUpper() + ", ";
                else
                    requiredLettersString += adder.ToUpper() + ".";
            }
            else if (chance < 73)
            {
                //Letters that give 15 points are given a chance value of 7/160
                string adder = alphabet15[UnityEngine.Random.Range(0, alphabet15.Length)];
                requiredLetters.Add(adder);
                if (x != (numLetters - 1))
                    requiredLettersString += adder.ToUpper() + ", ";
                else
                    requiredLettersString += adder.ToUpper() + ".";
            }
            else if (chance < 121)
            {
                //Letters that give 25 points are given a chance value of 6/160
                string adder = alphabet25[UnityEngine.Random.Range(0, alphabet25.Length)];
                requiredLetters.Add(adder);
                if (x != (numLetters - 1))
                    requiredLettersString += adder.ToUpper() + ", ";
                else
                    requiredLettersString += adder.ToUpper() + ".";
            }
            else if (chance < 161)
            {
                //Letters that give 30 points are given a chance value of 5/160
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
            //This is where we put the requiredLettersString into a UI element. 
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
        int high = array.Length-1;
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
        if (Countdown.instance.GetTyperStarter() == true)
            CheckInput();
    }

    private void CheckInput()
    {
        //If we detect that a key has been pressed...
        if (Input.anyKeyDown)
        {
            //Deletes a letter out of the users output if the key pressed is Delete or Backspace
            if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
            {
                if (string.IsNullOrEmpty(outputWord) == false)
                {
                    outputWord = outputWord.Remove(outputWord.Length - 1);
                }
            }
            //Submits the typed word to determine if it is a valid word and return points when Enter, Return or Space is pressed
            else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                SubmitWord();
            }
            //Skips the letter requirements and creates new ones when Tab is pressed
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                SkipWord();
            }
            else
            {
                //All the letters on the keyboard are valid key codes, and this is how the user types
                foreach (KeyCode keyCode in validKeyCodes)
                    if (Input.GetKeyDown(keyCode))
                    {
                        outputWord = outputWord + keyCode.ToString().ToLower();
                    }
            }
            //Here the output word is put into a UI element to simulate typing
            wordOutput.text = outputWord.ToUpper();
        }
    }

    private void SkipWord()
    {
        ScoreManager.instance.RemovePoints(25);
        ClearRequiredLetters();
        CreateRequiredLetters();
        message.text = "WORD SKIPPED, -25 POINTS";
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
        //Checks if the submitted word is a real wordby a binary search through a dictionary, checks if it fits the letter requirements
        //, and checks if the user has already submitted this word. 
        if (BinarySearch(wordsList, outputWord) > -1 && ContainsLetters(outputWord, requiredLetters) && NotRepeating())
        {
            //The stopwatch "sw" is used to determine combos. 
            //If a previous stopwatch has started from a previously submitted word...
            if (swStarted == true)
            {
                //compares the time distance from a previously correctly submitted word
                System.TimeSpan ts = System.DateTime.UtcNow - startTime;
                swStarted = false;
                double timeEl = ts.TotalMilliseconds;
                UnityEngine.Debug.Log("Time " + timeEl);
                //if the time distance is less than 2.5 seconds, it makes a combo and adds the combo to the history.
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
            //starts the stopwatch for the next submitted word
            if (swStarted == false)
            {
                startTime = System.DateTime.UtcNow;
                swStarted = true; 
            }

            //adds time to the timer depending on gamemode
            Timer.instance.addTime(timeAdded);
            //adds this submitted word to the history
            submittedWords.Add(outputWord);
            //prompts the score manager to add points
            ScoreManager.instance.AddPoint(outputWord, comboDecider());


            if (comboBool == true)
            {
                //adds a pop up message if there is a combo
                message.text = "COMBO + " + comboDecider();
                messageHistory.Add(2);
                StartCoroutine(messageChecker());
                //Plays a sound effect if there is a combo
                ComboFX.Play();
            }
            else
            {
                //Plays a sound effect if a word is a normal word
                EnterFX.Play();
            }

            //clears the user output and creates new letter requirements
            outputWord = string.Empty;
            ClearRequiredLetters();
            CreateRequiredLetters(); 
        }

        else
        {
            //if the submitted word is incorrectly submitted there will be a message displayed depending on the error
            //If the user doesn't type a word...
            if (outputWord == "" || outputWord == string.Empty || outputWord == null)
            {
                message.text = "YOU DONT HAVE A WORD TYPED";
            }
            //If the user already used this word...
            else if (NotRepeating() == false)
            {
                message.text = "REPEATING WORD";
            }
            //If the word is not in the dictionary...
            else if (BinarySearch(wordsList, outputWord) < 0)
            {
                message.text = "WORD DOES NOT EXIST";
            }
            //If the word doesn't contain the required letters...
            else if (!ContainsLetters(outputWord, requiredLetters))
            {
                message.text = "WORD DOESN'T FIT REQUIREMENTS";
            }
            //Adds an error message into the message history
            messageHistory.Add(3);
            StartCoroutine(messageChecker());
        }
    }

    private int comboDecider()
    {
        bool threeInRow = false, sixInRow = false;
        if (comboBool == true)
        {
            //Checks if there have been three combos in a row
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

            //Checks if there have been six combos in a row
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

            //Determines points based on if there have been six, three, or less combos in a row
            if (sixInRow == true)
            {
                //Amount of points also based upon the length of the word
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
            message.text = "";
        }
    }
}

