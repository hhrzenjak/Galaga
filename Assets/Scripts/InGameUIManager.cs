using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{

    [Header("Enter Name Screen Data")]

    public GameObject EnterNameScreen;
    public Button EnterNameScreenFirstButton;
    public Text NameDisplay;

    [Header("Display Rank Screen Data")]

    public GameObject DisplayRankScreen;
    public List<ScoreEntryDisplay> ScoreEntries = new List<ScoreEntryDisplay>();

    [Header("Message Screen Data")]

    public float MessageDuration;
    public GameObject MessageScreen;
    public Text MessageText;

    [Header("Overheat Screen Data")]

    public GameObject OverheatScreen;
    public Image OverheatOverlay;
    public Text OverheatText;

    [Header("Pause Menu Screen Data")]

    public GameObject PauseMenuScreen;
    public Button PauseMenuScreenFirstButton;

    [Header("Player Info Screen Data")]

    public GameObject PlayerInfoScreen;
    public Text ScoreText;

    private string _stage;

    private bool _cleared;
    private float _clearedTimer;

    private float _displayTimer;

    private float _messageTimer;

    private bool _over;
    private float _overTimer;

    private string _messageText;
    private string _overheatText;

    private string _name = "";

    private void Awake()
    {

        if (GameManager.GM)
        {

            GameManager.GM.ScoreChangedEvent.AddListener(ChangeScore);

            _stage = GameManager.GM.SceneIndex.ToString();
            _stage = _stage.Length == 2 ? _stage : "0" + _stage;

        }

        if (InputManager.IM)
            InputManager.IM.ButtonInputEvent.AddListener(ButtonInputHandler);

        if (StageManager.SM)
            StageManager.SM.StageClearedEvent.AddListener(StageCleared);

        PlayerLifeManager.GameOverEvent.AddListener(GameOver);

        ActionChooseSymbol.SymbolChosenEvent.AddListener(AddSymbol);
        ActionDeleteSymbol.SymbolDeletedEvent.AddListener(DeleteSymbol);
        ActionSubmit.SubmitEvent.AddListener(SubmitName);

        EnterNameScreen.SetActive(false);
        DisplayRankScreen.SetActive(false);
        MessageScreen.SetActive(false);
        OverheatScreen.SetActive(false);
        PauseMenuScreen.SetActive(false);
        PlayerInfoScreen.SetActive(true);

        _overheatText = OverheatText.text;

        ChangeScore();
        DisplayMessage("STAGE " + _stage);

    }

    private void Update()
    {

        #region STAGE CLEARED SECTION

        if (_cleared && _clearedTimer > 0)
        {

            _clearedTimer -= Time.deltaTime;

            if (_clearedTimer <= 0)
            {

                if (GameManager.GM.SceneIndex == Constants.LastStageIndex && GameManager.GM.Score > 0)
                {
                    
                    EnterNameScreen.SetActive(true);
                    EnterNameScreenFirstButton.Select();
                }

                else
                    GameManager.GM.StartNewStage();

            }

        }

        #endregion

        #region MESSAGE DISPLAY SECTION

        if (_messageTimer > 0)
        {

            _messageTimer -= Time.deltaTime;

            if (_messageTimer < 0)
            {

                _messageTimer = 0;

                MessageText.text = "";
                OverheatText.text = _overheatText;

                MessageScreen.SetActive(false);

            }

        }

        #endregion

        #region GAME OVER SECTION

        if (_over && _overTimer > 0)
        {

            _overTimer -= Time.deltaTime;

            if (_overTimer <= 0)
            {

                if (GameManager.GM.Score > 0)
                {
                    EnterNameScreen.SetActive(true);
                    EnterNameScreenFirstButton.Select();
                }

                else
                    GameManager.GM.ReturnToMenu();

            }

        }

        #endregion

        #region DISPLAY RANK SECTION

        if (_displayTimer > 0)
        {

            _displayTimer -= Time.deltaTime;

            if (_displayTimer <= 0)
                GameManager.GM.ReturnToMenu();

        }

        #endregion

    }

    private void ButtonInputHandler(InputType input)
    {

        if (input == InputType.PAUSE)
            TurnOnPauseMenuScreen();

        if (input == InputType.SELECT)
            if (EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();

    }

    #region METHODS FOR ENTERING THE PLAYER'S NAME

    private void AddSymbol(char symbol)
    {

        if (_name.Length > Constants.NameLength.y)
            return;

        _name += symbol;

        if (NameDisplay)
            NameDisplay.text = _name;

    }

    private void DeleteSymbol()
    {

        if (_name.Length > 0)
            _name = _name.Remove(_name.Length - 1);

        if (NameDisplay)
            NameDisplay.text = _name;

    }

    private void SubmitName()
    {

        if (GameManager.GM == null)
            return;

        if (_name.Length < Constants.NameLength.x)
            return;

        if (EnterNameScreen.activeSelf == false)
            return;


        int rank = DataManager.DM.AddScoreEntry(_name, GameManager.GM.Score);
        DisplayRank(rank);

    }

    #endregion

    #region METHODS FOR DISPLAYING THE PLAYER'S RANK

    private void DisplayRank(int rank)
    {

        int startingIndex;
        int endingIndex;

        if (rank == -1)
        {

            startingIndex = Constants.EntriesCount - 4;
            endingIndex = Constants.EntriesCount - 1;

            for (int counter = 0, index = startingIndex; index < endingIndex; counter++, index++)
                DisplayScoreEntry(counter, index + 1, DataManager.DM.GetEntry(index));

            DisplayScoreEntry(4, 0, new ScoreEntry(_name, GameManager.GM.Score));

        }

        else
        {

            if (rank == 0 || rank == 1)
            {
                startingIndex = 0;
                endingIndex = 4;
            }

            else if (rank == Constants.EntriesCount - 2 || rank == Constants.EntriesCount - 1)
            {
                startingIndex = Constants.EntriesCount - 5;
                endingIndex = Constants.EntriesCount - 1;
            }

            else
            {
                startingIndex = rank - 2;
                endingIndex = rank + 2;
            }

            for (int counter = 0, index = startingIndex; index <= endingIndex; counter++, index++)
                DisplayScoreEntry(counter, index + 1, DataManager.DM.GetEntry(index));

        }

        int indexScoreEntry;

        if (rank == -1)
            indexScoreEntry = 4;
        else if (rank == 0)
            indexScoreEntry = 0;
        else if (rank == 1)
            indexScoreEntry = 1;
        else if (rank == Constants.EntriesCount - 2)
            indexScoreEntry = 3;
        else if (rank == Constants.EntriesCount - 1)
            indexScoreEntry = 4;
        else
            indexScoreEntry = 2;

        Image background = ScoreEntries[indexScoreEntry].GetComponent<Image>();

        if (background)
            background.color = new Color(245f/255, 245f/255, 245f/255, 128f/255);

        EnterNameScreen.SetActive(false);
        DisplayRankScreen.SetActive(true);

        _displayTimer = 5.5f;

    }

    private void DisplayScoreEntry(int index, int rank, ScoreEntry entry)
    {

        if (rank > 0)
            ScoreEntries[index].PlayerRankText.text = rank.ToString("00");
        else
            ScoreEntries[index].PlayerRankText.text = "...";

        if (entry.Name.Length == 0)
            ScoreEntries[index].PlayerNameText.text = "EMPTY";
        else 
            ScoreEntries[index].PlayerNameText.text = entry.Name;

        ScoreEntries[index].PlayerScoreText.text = entry.Score.ToString("00000000");

    }

    #endregion

    private void ChangeScore()
    {
        if (GameManager.GM)
            ScoreText.text = GameManager.GM.Score.ToString("00000000");
    }

    private void GameOver()
    {

        DisplayMessage("GAME OVER");
        TurnOffOverheatScreen();

        _over = true;
        _overTimer = MessageDuration + 0.25f;

    }

    private void StageCleared()
    {

        if (_over)
            return;

        if (GameManager.GM.SceneIndex == Constants.LastStageIndex)
        {
            DisplayMessage("GAME FINISHED");
            GameManager.GM.AddToScore(Constants.PointsForGameFinished);
        }

        else
        {
            DisplayMessage("STAGE CLEARED");
            GameManager.GM.AddToScore(Constants.PointsForStageCleared);
        }

        TurnOffOverheatScreen();

        _cleared = true;
        _clearedTimer = MessageDuration + 0.25f;

    }

    #region MESSAGE SCREEN METHODS

    public void DisplayMessage(string message)
    {

        if (OverheatScreen.activeSelf)
            OverheatText.text = "";

        MessageScreen.SetActive(true);
        MessageText.text = message;

        if (MessageText.GetComponent<TextFader>())
            MessageText.GetComponent<TextFader>().StartFading();

        _messageTimer = MessageDuration;

    }

    #endregion

    #region OVERHEAT SCREEN METHODS

    public void TurnOnOverheatScreen()
    {

        if (_cleared || _over)
            return;

        if (MessageScreen.activeSelf)
        {

            MessageText.text = "";
            MessageScreen.SetActive(false);

            _messageTimer = 0;

        }

        OverheatText.text = _overheatText;
        OverheatScreen.SetActive(true);

        if (OverheatOverlay.GetComponent<ImageFader>())
            OverheatOverlay.GetComponent<ImageFader>().StartFading();

        if (OverheatText.GetComponent<TextFader>())
            OverheatText.GetComponent<TextFader>().StartFading();

    }

    public void TurnOffOverheatScreen()
    {
        OverheatScreen.SetActive(false);
    }

    #endregion

    #region PAUSE MENU SCREEN METHODS

    public void TurnOnPauseMenuScreen()
    {

        if (PauseMenuScreen.activeSelf)
            return;

        _messageText = MessageText.text;
        _overheatText = OverheatText.text;

        MessageText.text = "";
        OverheatText.text = "";

        EventSystem.current.SetSelectedGameObject(null);

        PauseMenuScreen.SetActive(true);
        PauseMenuScreenFirstButton.Select();

        if (GameManager.GM)
            GameManager.GM.Pause();

    }

    public void TurnOffPauseMenuScreen()
    {

        if (PauseMenuScreen.activeSelf == false)
            return;

        MessageText.text = _messageText;
        OverheatText.text = _overheatText;

        PauseMenuScreen.SetActive(false);

        if (GameManager.GM)
            GameManager.GM.Continue();

    }

    #endregion

}
