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

    public GameObject lightningContainer;

    Level currentLevelData;
    #region Game status
    [SerializeField]
    private bool isGameWin = false;
    [SerializeField]
    private bool isGameLose = false;
    #endregion

    private void Start()
    { 
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);
        lightningContainer = Instantiate(currentLevelData.levelObjects);
        
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
        yield return new WaitForSeconds(.5f);
        gameScene.ShowWinPanel();
    }

    public void Lose()
    {
        isGameLose = true;
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
}

