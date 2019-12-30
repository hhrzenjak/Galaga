using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region GAME MANAGER PROPERTY

    private static GameManager _GM;

    public static GameManager GM
    {
        get
        {
            if (_GM == null)
                _GM = FindObjectOfType<GameManager>();
            return _GM;
        }
    }

    #endregion

    #region PAUSED PROPERTY

    private bool _paused;

    public bool Paused
    {
        get
        {
            return _paused;
        }
    }

    #endregion

    #region PLAYER DATA PROPERTY

    private PlayerData _playerData;

    public PlayerData PlayerData
    {
        get
        {
            return _playerData;
        }
    }

    #endregion

    #region SCENE INDEX PROPERTY

    public int SceneIndex
    {
        get
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }

    #endregion

    #region SCORE PROPERTY

    private int _score;

    public int Score
    {
        get
        {
            return _score;
        }
    }

    #endregion

    public UnityEvent ScoreChangedEvent = new UnityEvent();

    private void Awake()
    {

        #region GAME MANAGER PROPERTY SET-UP

        if (_GM == null)
            _GM = this;

        if (_GM.Equals(this) == false)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        #endregion

        ResetPlayerData();
        ResetScore();

    }

    private void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void GoToNextScene()
    {
        GoToScene(SceneIndex + 1);
    }

    #region METHODS FOR CHANGING STAGES AND RETURNING TO THE MAIN MENU

    public void StartGame()
    {
        GoToScene(1);
    }

    public void StartNewStage()
    {
        GoToNextScene();
    }

    public void ReturnToMenu()
    {
        ResetPlayerData();
        ResetScore();
        GoToScene(0);
    }

    #endregion

    #region METHODS FOR CHANGING THE SCORE

    private void ResetScore()
    {
        _score = 0;
    }

    public void AddToScore(int points)
    {
        _score += points;
        ScoreChangedEvent.Invoke();
    }

    #endregion

    #region METHODS FOR PAUSING THE GAME

    public void Continue()
    {
        _paused = false;
        Time.timeScale = 1;
    }

    public void Pause()
    {
        _paused = true;
        Time.timeScale = 0;
    }

    #endregion

    #region METHODS FOR SAVING THE PLAYER DATA

    public void ResetPlayerData()
    {
        _playerData = null;
    }

    public void SavePlayerData(int playerLives, int playerShootingLevel)
    {
        _playerData = new PlayerData(playerLives, playerShootingLevel);
    }

    #endregion

}
