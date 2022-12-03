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

    public int collectibleCount;
    public TextMeshProUGUI collectibleText;

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
    }

}
