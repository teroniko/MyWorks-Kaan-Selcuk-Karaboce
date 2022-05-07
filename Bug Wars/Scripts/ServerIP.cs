using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerIP : MonoBehaviour
{
    public TMP_Text EnteredIP;
    public TMP_Text Title;
    public static bool WrongIP = false;
    private void Awake()
    {
        if (WrongIP)
        {
            Title.text += "\nWrong IP";
        }
    }
    public void Play()
    {
        NetworkManager.IP = EnteredIP.text;
        
        SceneManager.LoadScene("GamePlay",LoadSceneMode.Single);
    }
}
