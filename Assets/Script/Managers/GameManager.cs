using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EnemySpawner spawner;

    float score;
    int gold;

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        Application.targetFrameRate = 60;
    }

    Vector2 playerDefaultPos = new Vector2(0, -7.5f);
    Vector2 enemySpawnPos = new Vector2(0, 50f);

    public Vector2 PlayerDefaultPos { get => playerDefaultPos; }
    public Vector2 EnemySpawnPos { get => enemySpawnPos; }
    public float Score
    {
        get => score;
        set
        {
            score = value;
            UIManager.instance.OnUpdateScore(score);
        }
    }

    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            UIManager.instance.OnUpdateGold(gold);
        }
    }

    private void Start()
    {
        UIManager.instance.OpenCoverPanel("Start", GameStart);
    }

    public void GameStart()
    {
        spawner.SpawnEnemyWave();
        UIManager.instance.CloseCoverPanel();
    }

    public void GameWin()
    {
        UIManager.instance.OpenCoverPanel("Game Win");
    }

    public void GameOver()
    {
        UIManager.instance.OpenCoverPanel("Game Over");
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(0);
    }
}
