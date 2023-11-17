using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Platform : MonoBehaviour
{
    public Motor[] Motors;
    float[] sliderValue = new float[6];
    public bool Success = true;
    private sbyte Version=1;
    public TMP_Text Title;
    public Slider[] sliders = new Slider[6];
    private void Start()
    {


        VersionChange();
    }
    
    public void newPosRot(int Axes, float value)
    {
        sliderValue[Axes] = value;
        transform.position = new Vector3(sliderValue[0], sliderValue[1], sliderValue[2]);
        transform.rotation = Quaternion.Euler(new Vector3(sliderValue[3], sliderValue[4], sliderValue[5]));
        foreach (Motor m in Motors)
        {
            m.StickStrect();
            if (m.Success)
            {
                Success = true;
            }
            else
            {
                Success = false;
                break;
            }
        }
    }
    
    public void Close()
    {
        Application.Quit();
    }
    private void VersionChange()
    {
        if (Version < 0)
        {
            Version = 0;
            return;
        }
        if (Version > 1)
        {
            Version = 1;
            return;
        }
        foreach (Slider s in sliders)
        {
            s.value = 0;
        }
        switch (Version)
        {
            case 0:
                MaxMinChanges("4 Inch Stewart Pro", 389.4f, 287.8f, new float[] { -60, 60f, -60, 60, 397, 497, -20, 20, -20, 20, -20, 20 }/*min>max*/);
                break;
            case 1:
                MaxMinChanges("8 Inch Stewart Pro", 598, 389.91f,new float[] { -80,80f,-80,80, 498, 704+5, -30,30,-30,30,-30,30}/*min>max*/);
                break;
        }

        
    }
    private void MaxMinChanges(string title, float MaxLenght, float MinLenght, float[] SliderMaxMin)
    {
        Title.text = title;
        foreach (Motor m in Motors)
        {
            m.MaxLenght =  MaxLenght/ 50f;
            m.MinLenght = MinLenght / 50f;
        }


        float Multp = 50;
        for (int i = 0; i < sliders.Length; i++)
        {
            if (i > 2)
            {
                Multp = 1;
            }
            sliders[i].minValue = SliderMaxMin[i*2]/Multp;
            sliders[i].maxValue = SliderMaxMin[i*2+1]/Multp;

        }

        sliders[2].value = sliders[2].minValue + (sliders[2].maxValue - sliders[2].minValue) / 2f;
    }
    public void NextVersion()
    {
        Version++;
        VersionChange();
    }
    public void PreviousVersion()
    {
        Version--;
        VersionChange();
    }
}
