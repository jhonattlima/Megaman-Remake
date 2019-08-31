using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private int score = 0;

    public void updateScore(int value)
    {
        score += value;
        GetComponent<Text>().text = "Score: " + score;
    }
}
