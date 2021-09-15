using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get => instance; }

    [SerializeField] GameObject loseCanvas;
    [SerializeField] GameObject inGameCanvas;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TowerGenerator towerGenerator;

    private int score = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetScore(score);
    }

    private void SetScore(int value)
    {
        scoreText.text = "Score: " + value.ToString();
    }

    public void IncreaseScore()
    {
        score++;
        SetScore(score);
    }

    public void GameOver()
    {
        inGameCanvas.SetActive(false);
        loseCanvas.SetActive(true);
    }

    public void InstatiateTower()
    {
        towerGenerator.Create();
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
