using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;

    public Transform lightningContainer;

    Level currentLevelData;
    #region Game status
    private bool isGameWin = false;
    private bool isGameLose = false;
    private bool isGameStart = false;
    private float startCountDown = 5.5f;
    #endregion

    private void Start()
    { 
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);
        lightningContainer = Instantiate(currentLevelData.levelObjects).transform.Find("Lightnings");
    }

    private void Update()
    {
        if (!isGameStart && startCountDown > 0)
        {
            startCountDown -= Time.deltaTime;
            gameScene.SetCountDown(startCountDown);
        }
        else if (!isGameStart && startCountDown <= 0)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        isGameStart = true;
        foreach (Transform lightning in lightningContainer)
        {
            lightning.GetComponent<SpriteRenderer>().DOFade(0, .5f);
            gameScene.CloseCountdown();
        }
    }

    public void Win()
    {
        isGameWin = true;
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }
        LevelManager.instance.levelData.SaveDataJSON();
        StartCoroutine(WaitToWin());
    }

    private IEnumerator WaitToWin()
    {
        yield return new WaitForSeconds(1f);
        gameScene.ShowWinPanel();
    }

    public void Lose()
    {
        isGameLose = true;
        StartCoroutine(WaitToLose());
    }

    private IEnumerator WaitToLose()
    {
        yield return new WaitForSeconds(1f);
        sceneChanger.ChangeToGameScene();
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool IsGameLose()
    {
        return isGameLose;
    }

    public bool IsGameStart()
    {
        return isGameStart;
    }
}

