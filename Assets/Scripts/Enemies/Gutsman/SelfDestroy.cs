using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    private void selfDestroy() // Called by animator
    {
        StartCoroutine(coroutineSelfDestroy());
    }

    private IEnumerator coroutineSelfDestroy()
    {
        StageActor.instance.stageController.fadeObjectWhite.GetComponent<FadeFeature>().isFadingIn = true;
        yield return new WaitForSeconds(3);
        StageActor.instance.stageController.fadeObjectWhite.GetComponent<FadeFeature>().isFadingIn = false;
        transform.parent.GetComponentInChildren<GutsmanHealthTouchController>().healthBar.gameObject.SetActive(false);
        Destroy(transform.parent.gameObject);
        StageActor.instance.stageController.finishStage();
        yield return null;
    }
}
