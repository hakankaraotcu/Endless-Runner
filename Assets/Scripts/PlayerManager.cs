using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    private PlayerManager()
    {
        if (instance == null)
        {
            Debug.Log("Instance is null");
            instance = this;
        }
    }

    public static PlayerManager getInstance()
    {
        return instance;
    }

    public bool isGameOver;
    public GameObject gameOverPanel;

    void Start()
    {
        isGameOver = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (isGameOver)
        {
            Debug.Log("Game Over");
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
    }

}
