using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{
    public Slider slider;
    public float ActiveTime;
    public float ActiveTimeEnergy;
    //public float requiredEnergy;
    public Slider Energy;
    float loopUnit;
    float moment;
    
    public void UpdateActiveTEnergy()
    {

        ActiveTimeEnergy = Energy.maxValue * ActiveTime / Energy.value;



        moment = 0.1f;
        loopUnit = slider.maxValue / (ActiveTimeEnergy / moment);
    }
    public IEnumerator Active()
    {
        slider.value = 0;
        UpdateActiveTEnergy();


        //Energy.value -= requiredEnergy;
        //ActiveTime / moment=loopcount
        for (float time = 0; time < ActiveTimeEnergy; time += moment)
        {
            yield return new WaitForSeconds(moment);
            slider.value += loopUnit;
        }
    }
    public bool Ready()
    {
        return slider.value >= slider.maxValue-0.01f;
    }
    //public bool EnergyExist()
    //{
    //    return Energy.value >= requiredEnergy;

    //}
}
