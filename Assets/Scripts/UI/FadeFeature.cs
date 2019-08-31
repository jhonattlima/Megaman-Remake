using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFeature : MonoBehaviour
{
    public Image fadeImage;
    public bool isFadingIn = false;
    public float fadeInTime = 0.8f;
    public float fadeOutTime = 1.0f;

    void Update()
    {
        //If the toggle returns true, fade in the Image
        if (isFadingIn == true)
        {
            //Fully fade in Image with the duration of fadeInTime
            fadeImage.CrossFadeAlpha(1, fadeInTime, false);
        }
        //If the toggle is false, fade out to nothing the Image with a duration of fadeOutTime
        if (isFadingIn == false)
        {
            fadeImage.CrossFadeAlpha(0, fadeOutTime, false);
        }
    }
}