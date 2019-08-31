using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    private CameraController _cam;

    public GameObject readySign;
    public GameObject gutsman;
    public GameObject fadeObjectBlack;
    public GameObject fadeObjectWhite;
    public GameObject thanksForPlayingScreen;
    public ScoreController scoreController;

    void Start()
    {
        _cam = Camera.main.GetComponent<CameraController>();
        StartCoroutine(coroutineStartStage());
    }

    private void Update()
    {
        if (!gutsman)
        {
            GameManager.instance.isInputEnabled = false; // Prevent megaman from being able to walk if gutsman is dead
        }
    }

    public void restartStage()
    {
        StartCoroutine(coroutineRestartStage());
    }

    public void finishStage()
    {
        StartCoroutine(coroutineFinishStage());
    }

    public void startBossBattle()
    {
        StartCoroutine(coroutineStartBossFight());
    }

    private IEnumerator coroutineStartStage()
    {
        fadeObjectWhite.SetActive(true);
        fadeObjectBlack.SetActive(true);
        GameManager.instance.isInputEnabled = false;
        MegamanActor.instance.megamanAnimation.arrive();
        MegamanActor.instance.body.MovePosition(new Vector2(9.61f, 3f));
        _cam.transform.position = new Vector3(9.6f, 7.7f, -10f);    // Camera start position: 9.6, 7.7
        MegamanActor.instance.body.isKinematic = true;
        readySign.SetActive(true);
        yield return new WaitForSeconds(3);
        readySign.SetActive(false);
        SFXPlayer.instance.Play("megamanTeleport");
        MegamanActor.instance.body.isKinematic = false;
        MegamanActor.instance.megamanAnimation.arrive();
        while (!MegamanActor.instance.body.IsTouchingLayers(GameManager.instance.layerGround)) yield return null;
        MegamanActor.instance.megamanAnimation.hasArrived();
        GameManager.instance.isInputEnabled = true;
    }

    private IEnumerator coroutineRestartStage()
    {
        yield return new WaitForSeconds(0.4f);
        fadeObjectBlack.GetComponent<FadeFeature>().isFadingIn = true;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(GameManager.instance.chosenScene, LoadSceneMode.Single);
    }

    private IEnumerator coroutineStartBossFight()
    {
        yield return new WaitForSeconds(1);
        MusicPlayer.instance.switchToBossMusic();
        yield return new WaitForSeconds(3);
        gutsman.SetActive(true);
        yield return new WaitForSeconds(2);
        GameManager.instance.isInputEnabled = true;
    }

    private IEnumerator coroutineFinishStage()
    {
        MegamanActor.instance.body.velocity = Vector2.zero;
        GameManager.instance.isInputEnabled = false;
        MusicPlayer.instance.switchToStageClearMusic();
        yield return new WaitForSeconds(6.8f);
        MegamanActor.instance.megamanAnimation.victory();
        SFXPlayer.instance.Play("megamanVictory");
        yield return new WaitForSeconds(1.5f);
        SFXPlayer.instance.Play("megamanTeleport");
        MegamanActor.instance.body.isKinematic = true;
        MegamanActor.instance.body.velocity = new Vector2(0,30);
        yield return new WaitForSeconds(2);
        fadeObjectBlack.GetComponent<FadeFeature>().isFadingIn = true;
        yield return new WaitForSeconds(2);
        thanksForPlayingScreen.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
