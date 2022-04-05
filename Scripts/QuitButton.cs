using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour {

    public void Quit()
    {
        Debug.Log("has quit game");
        Application.Quit();
    }
}
