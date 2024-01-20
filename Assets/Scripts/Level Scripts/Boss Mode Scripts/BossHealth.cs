using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour
{
    public static BossHealth instance;
    public Slider bar;
    public int bossHealth; 

    void Start()
    {
        instance = this;
        bar.maxValue = bossHealth;
        bar.value = bossHealth;
    }

    void Update()
    {
        
    }

    public void takeDamage(string word)
    {
        int damage = 0; 
        int[] letterPoints = { 10, 30, 25, 15, 10, 25, 25, 15, 15, 30, 30, 25, 25, 15, 15, 30, 30, 15, 15, 10, 25, 30, 25, 30, 25, 30 };
        int points = 0;
        foreach (char c in word)
        {
            int letterNum = (int)c - 97;
            Debug.Log(c.ToString().ToUpper() + " " + letterNum + " " + letterPoints[letterNum]);
            points += letterPoints[letterNum];
        }
        damage += points;
        if (bossHealth - points < 0)
            bossHealth = 0;
        else
            bossHealth -= points;
        bar.value = bossHealth;
    }
}
