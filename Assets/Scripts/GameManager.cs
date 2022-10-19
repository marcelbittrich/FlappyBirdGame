using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public TMP_Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public GameObject prideToggle;
    private int score;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        gameOver.SetActive(false);
        Pause();
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        prideToggle.SetActive(false);

        Time.timeScale = 1;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

    }

    public void Pause()
    {
        Time.timeScale = 0;
        player.enabled = false;
    }


    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);
        prideToggle.SetActive(true);
        Pause();
    }

    public void IncreaseScore() 
    {
        score++;
        scoreText.text = score.ToString();
    }
}