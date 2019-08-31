using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private float _initLife;
    public Image HealthChips;

    public void setLife(float initLife)
    {
        this._initLife = initLife;
        map(initLife);
    }

    public void map(float currentLife) //Update life bar sprite
    {
        HealthChips.fillAmount = currentLife / _initLife;
    }
}
