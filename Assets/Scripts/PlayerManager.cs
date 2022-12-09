using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private PlayerManager() { }
    public bool isGameOver;
    public GameObject gameOverPanel;

    // Collectible
    public int collectibleCount;
    public TextMeshProUGUI collectibleText;

    // Power
    public PowerBar powerBar;
    public float powerCount;
    public float maxPowerCount = 5f;
    public bool stopIncrease = false;

    public static PlayerManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        powerBar.SetMinPower(0);
        isGameOver = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (isGameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        collectibleText.text = "Coins: " + collectibleCount;

        if (powerCount <= maxPowerCount)
        {
            powerBar.SetPower(powerCount);
        }
        if(powerCount >= maxPowerCount)
        {
            powerCount = maxPowerCount;
            powerBar.SetPower(powerCount);
            stopIncrease = true;
        }
    }
}
