using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    void Play()
    {
        SceneManager.LoadScene("Strategy", LoadSceneMode.Single);
    }
    public void Level1()
    {
        Strategy0.level = 1;
        Play();
    }
    public void Level2()
    {
        Strategy0.level = 2;
        Play();
    }
    public void Level3()
    {
        Strategy0.level = 3;
        Play();
    }
}
