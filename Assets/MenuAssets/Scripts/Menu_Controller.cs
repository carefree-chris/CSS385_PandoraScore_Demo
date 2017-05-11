using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Controller : MonoBehaviour {

    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private GameObject button3;
    [SerializeField] private GameObject button4;
    //[SerializeField] private GameObject button5;
    [SerializeField] private GameObject button6;
    [SerializeField] private GameObject button7;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject exitButton;

    [SerializeField] private Text creditsTitle;
    [SerializeField] private Text creditsText;

    private MenuState currentState;


    //We're using an extremely simple finite state machine.
    protected enum MenuState
    {
        mainMenu,
        creditsScreen
    }
    
    //Set uneeded menu items blank or disabled.
	void Start () {

        backButton.SetActive(false);
        creditsTitle.text = "";
        creditsText.text = "";

        currentState = MenuState.mainMenu;

	}

    //Simply switches us from Main Menu to Credits, and vice versa
    public void UpdateState()
    {
        switch (currentState)
        {
            case MenuState.mainMenu:
                currentState = MenuState.creditsScreen;
                CreditsUpdate();
                break;
            case MenuState.creditsScreen:
                currentState = MenuState.mainMenu;
                MenuUpdate();
                break;
        }
    }

    //Load scenes from the menu.
    public void SwitchToScene(string sceneTitle)
    {
        SceneManager.LoadSceneAsync(sceneTitle);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //Switch over to main menu
    private void MenuUpdate()
    {
        backButton.SetActive(false);
        creditsTitle.text = "";
        creditsText.text = "";

        SetButtonVisibility(true);
    }

    //Switch over to the credits
    private void CreditsUpdate()
    {
        backButton.SetActive(true);
        creditsTitle.text = "Presented By...";
        //TODO change date
        creditsText.text = " Andrue Cashman \n Trayce Luxtrum \n Weizong Zhou \n Christopher Lynn \n\n 4/28/2017";

        SetButtonVisibility(false);
    }

    private void SetButtonVisibility(bool isVisible)
    {
        button1.SetActive(isVisible);
        button2.SetActive(isVisible);
        button3.SetActive(isVisible);
        button4.SetActive(isVisible);
        //button5.SetActive(isVisible);
        button6.SetActive(isVisible);
        button7.SetActive(isVisible);
        creditsButton.SetActive(isVisible);
        exitButton.SetActive(isVisible);
    }

    
}
