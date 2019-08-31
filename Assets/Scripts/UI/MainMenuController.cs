using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject stageSelect;
    public GameObject loadingScreen;
    public GameObject creditScreen;
    public Button cutmanButton;
    public Button chaosSignButton;

    void Start()
    {
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null && stageSelect.activeSelf && !loadingScreen.activeSelf && !creditScreen.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(cutmanButton.gameObject);
        }
        else if (EventSystem.current.currentSelectedGameObject == null && mainMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }
    }

    public void buttonEvent(Button button) // Method called by buttons
    {
        StartCoroutine(selection(button));
    }

    IEnumerator selection (Button button)
    {
        yield return new WaitForSeconds(1);
        switch (button.name)
        {
            case "Normal_Megaselect":
                mainMenu.SetActive(false);
                stageSelect.SetActive(true);
                EventSystem.current.SetSelectedGameObject(cutmanButton.gameObject);
                GameManager.instance.chosenScene = GameManager.instance.sceneClassicName;
                break;
            case "Director'sCut_Megaselect":
                mainMenu.SetActive(false);
                stageSelect.SetActive(true);
                EventSystem.current.SetSelectedGameObject(cutmanButton.gameObject);
                GameManager.instance.chosenScene = GameManager.instance.sceneDirectorsCutName;
                break;
            case "Button_GutsMan":
                loadingScreen.SetActive(true);
                yield return new WaitForSeconds(5);
                SceneManager.LoadScene(GameManager.instance.chosenScene, LoadSceneMode.Single);
                break;
            case "Button_Megaman":
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                break;
            case "Button_ChaosSign":
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().GetComponent<Animator>().Rebind();
                stageSelect.SetActive(false);
                creditScreen.SetActive(true);
                yield return new WaitForSeconds(5);
                creditScreen.SetActive(false);
                stageSelect.SetActive(true);
                EventSystem.current.SetSelectedGameObject(cutmanButton.gameObject);
                break;
        }
    }
}
