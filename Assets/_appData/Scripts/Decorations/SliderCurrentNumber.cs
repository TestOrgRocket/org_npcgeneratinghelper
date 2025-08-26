using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCurrentNumber : MonoBehaviour
{
    public Text currentNumber;
    public Slider slider;
    void OnEnable()
    {
        currentNumber.text = slider.value.ToString("0");
    }
    public void OnSliderValueChanged() => currentNumber.text = slider.value.ToString("0");
}
