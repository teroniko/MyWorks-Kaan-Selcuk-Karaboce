using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Main MainC;

    private void Awake()
    {
        MainC.OpenMenu();
    }
    public void MathFocus()
    {
        MainC.MathFocus();
    }
    public void ArcheryFocus()
    {
        MainC.ArcheryFocus();
    }
    public void Training()
    {
        MainC.OpenTraining();
    }
    public void Close()
    {
        if (gameObject.activeSelf)
        {
            Application.Quit();
        }
        else if(MainC.Game.activeSelf)
        {
            MainC.OpenMenu();
        }
        else if(!MainC.Game.activeSelf && !MainC.Menu.activeSelf)
        {
            MainC.OpenMenu();
            MainC.Durations.enabled = false;
            MainC.Durations.text = "";
        }
    }
    public void Restart()
    {
        MainC.Restart();
    }
}
