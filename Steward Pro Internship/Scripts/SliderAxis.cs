using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SliderAxis : MonoBehaviour
{
    public Platform Pl;
    public int Axes;
    float OldValue;
    public Slider slider;
    public TMP_Text text;
    public string type;
    public void ValueChange(float value)
    {
        float UnityValue = 1;
        if (type == "mm")
        {
            UnityValue = 50;
        }
        text.text = (value * UnityValue) + type;
        Pl.newPosRot(Axes, value);
        if (Pl.Success)
        {
            OldValue = value;
        }
        else
        {
            slider.value = OldValue;
            Pl.newPosRot(Axes, OldValue);
        }

    }
   
}
