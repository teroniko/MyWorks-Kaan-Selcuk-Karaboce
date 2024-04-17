using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultySlider : MonoBehaviour
{
    public Main m;
    public TMP_Text Text;
    private short SliderValue = 0;
    public void OnValueChange(float value)
    {
        Text.text = "Game Difficulty Level : " + value;
        m.SliderDifficulty();
        SliderValue = (short)value;
    }
    public void SaveDiff()
    {
        PlayerPrefs.SetInt("Difficulty Value", SliderValue);
    }
}
