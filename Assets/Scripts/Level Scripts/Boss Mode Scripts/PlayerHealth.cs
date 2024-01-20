using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public int health;
    private int inHealth; 
    public Animator transition;
    public AudioSource attackFX;
    public TextMeshProUGUI userHealth;
    public GameObject enemy;
    public int enemyDamage; 
    private bool startAttacks;
    public GameObject deathCanvas;
    public Slider bar;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inHealth = health;
        bar.maxValue = inHealth;
        bar.value = health; 
    }

    private void Update()
    {
        if (startAttacks == true)
        {
            StartCoroutine(EnemyAttacks());
            setAttacks(false);
        }
    }

    public void setAttacks(bool x)
    {
        startAttacks = x; 
    }

    public void removeHealth(int x)
    {
        if (health - x < 0)
            health = 0; 
        else
            health -= x;
        bar.value = health;
        checkHealth();
    }

    public void addHealth(int x)
    {
        if (health + x > 100)
        {
            health = 100; 
        }
        else
        {
            health += x;
        }
        bar.value = health;
    }

    private void checkHealth()
    {
        if (health <= 0)
        {
            Stopwatch.instance.setGameEnded(true);
            Dialogue.instance.SetTyperStarter(false);
            Dialogue.instance.GameMusic.Stop();
            deathCanvas.gameObject.SetActive(true); 
        }
    }

    public IEnumerator EnemyAttacks()
    {
        while (health >= 0)
        {
            yield return new WaitForSeconds(Random.Range(15f, 20f));

            transition.SetTrigger("Start");

            Debug.Log("Started"); 

            yield return new WaitForSeconds(1f);

            transition.SetTrigger("End");

            Debug.Log("Ended");

            attackFX.Play();

            removeHealth(Random.Range(enemyDamage, enemyDamage + 5));

            bar.value = health;

            checkHealth();
        }
    }

    public void setBossPause()
    {
        Time.timeScale = 0;
    }

    public void breakBossPause()
    {
        Time.timeScale = 1;
    }

    public int returnPlayerHealth()
    {
        return health;
    }
}
