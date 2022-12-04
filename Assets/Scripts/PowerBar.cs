using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    private static PowerBar instance;

    public Slider slider;

    private PowerBar() { }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static PowerBar GetInstance()
    {
        return instance;
    }

    public void SetMinPower(float power)
    {
        slider.minValue = power;
        slider.value = power;
    }

    public void SetPower(float power)
    {
        slider.value = power;
    }

    public void SetMaxPower(float power)
    {
        slider.maxValue = power;
    }
}
