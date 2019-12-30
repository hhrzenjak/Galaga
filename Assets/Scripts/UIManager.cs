using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Highscore Screen Data")]

    public GameObject HighscoreScreen;
    public Button HighscoreScreenFirstButton;

    public Text[] ScoreEntryNames = new Text[Constants.TopEntriesCount];
    public Text[] ScoreEntryScores = new Text[Constants.TopEntriesCount];

    [Header("Menu Screen Data")]

    public GameObject MenuScreen;
    public Button MenuScreenFirstButton;

    [Header("Title Screen Data")]

    public GameObject TitleScreen;

    private void Awake()
    {

        GoToTitleScreen();

        if (GameManager.GM)
            GameManager.GM.Continue();

        if (InputManager.IM)
            InputManager.IM.ButtonInputEvent.AddListener(ButtonInputHandler);

    }

    private void ButtonInputHandler(InputType input)
    {

        if (input == InputType.ANY)
            if (TitleScreen.activeSelf)
                GoToMenuScreen();

        if (input == InputType.SELECT)
            if (EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();

    }

    private IEnumerator TurnOnScreen(GameObject screen, Button button)
    {

        yield return new WaitForSeconds(0.05f);

        screen.SetActive(true);
        button.Select();

        yield return null;

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void GoToHighscoreScreen()
    {

        MenuScreen.SetActive(false);
        StartCoroutine(TurnOnScreen(HighscoreScreen, HighscoreScreenFirstButton));

        ScoreEntry[] entries = DataManager.DM.GetTopEntries(Constants.TopEntriesCount);

        for (int counter = 0; counter < Constants.TopEntriesCount; counter++)
        {

            if (entries[counter].Name.Equals(""))
                ScoreEntryNames[counter].text = "EMPTY";
            else
                ScoreEntryNames[counter].text = entries[counter].Name;

            ScoreEntryScores[counter].text = entries[counter].Score.ToString("00000000");

        }

    }

    public void GoToMenuScreen()
    {

        if (HighscoreScreen.activeSelf)
            HighscoreScreen.SetActive(false);

        if (TitleScreen.activeSelf)
            TitleScreen.SetActive(false);

        StartCoroutine(TurnOnScreen(MenuScreen, MenuScreenFirstButton));

    }

    public void GoToTitleScreen()
    {

        if (HighscoreScreen.activeSelf)
            HighscoreScreen.SetActive(false);

        if (MenuScreen.activeSelf)
            MenuScreen.SetActive(false);

        TitleScreen.SetActive(true);

    }

}
