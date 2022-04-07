using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGameMode : MonoBehaviour
{
    public static bool GameMode;//true:game,simulation;false:setting
    private void Start()
    {
        GameMode = false;
    }
    public void Change_game_mode()
    {
        GameMode = !GameMode;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Button"))
        {

            if (g.GetComponent<ButtonScript>() != null)
            {
                g.GetComponent<ButtonScript>().enabled = !GameMode;
            }
        }
    }
}
