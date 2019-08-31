using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroController : MonoBehaviour
{
    private Camera cam;
    private VideoPlayer videoPlayer;

    public VideoClip parrots;
    public VideoClip chaosSign;
    public GameObject disclaimer;
    public Animator login;
    public Animator chaosSignText;
    public Animator code;
    public Animator potato;
    public Animator text2;
    public GameObject noInputEnabled;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        videoPlayer = cam.GetComponent<VideoPlayer>();
        StartCoroutine(startIntro());
    }

    void LateUpdate()
    {
        if (Input.anyKey)
        {
            StartCoroutine(activateNoInputEnabledText());
        }
    }

    IEnumerator startIntro()
    {
        yield return new WaitForSeconds(3);
        disclaimer.SetActive(true);
        yield return new WaitForSeconds(1);
        login.SetTrigger("02GO");
        yield return new WaitForSeconds(1);
        chaosSignText.SetTrigger("03GO");
        yield return new WaitForSeconds(1);
        code.SetTrigger("04GO");
        yield return new WaitForSeconds(1);
        potato.SetTrigger("05GO");
        yield return new WaitForSeconds(1);
        text2.SetTrigger("06 GO");
        yield return new WaitForSeconds(3);
        disclaimer.SetActive(false);
        //videoPlayer.url = parrots.originalPath;
        videoPlayer.clip = parrots;
        videoPlayer.Prepare();
        yield return new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(1);
            break;
        }
        videoPlayer.Play();
        while (videoPlayer.isPlaying) yield return null;
        videoPlayer.clip = chaosSign;
        //videoPlayer.url = chaosSign.originalPath;
        yield return new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(1);
            break;
        }
        videoPlayer.Play();
        while (videoPlayer.isPlaying) yield return null;
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator activateNoInputEnabledText()
    {
        if (!noInputEnabled.activeSelf)
        {
            noInputEnabled.SetActive(true);
            yield return new WaitForSeconds(3);
            noInputEnabled.SetActive(false);
        }
    }
}
