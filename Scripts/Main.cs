using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button Restart;
    public Button NextLevel;
    public Button PreviousLevel;
    public Button ArrowButton;
    public static int levelNumber = 1;
    public static int accessedLevel = 1;
    public static int ArrowCount;
    public Text SaplanmaNumber;
    public Text ObjectText2;
    public Text ObjectText3;
    public static int saplanmaNumber=5;
    void Start()
    {
        Restart.onClick.AddListener(() => RestartButton());
        NextLevel.onClick.AddListener(() => NextLevelButton());
        PreviousLevel.onClick.AddListener(() => PreviousLevelButton());
        ArrowCount = 100;
    }
    public static void RestartButton()
    {
        SceneManager.LoadScene(Main.accessedLevel - 1);
        switch (Main.accessedLevel)
        {
            case 1:
                Main.ArrowCount = 100;
                Main.saplanmaNumber = 5;
                break;
            case 2:
                Main.ArrowCount = 30;
                Main.saplanmaNumber = 10;
                break;
            case 3:
                Main.ArrowCount = 30;
                Main.saplanmaNumber = 10;
                break;
            case 4:
                Main.ArrowCount = 20;
                Main.saplanmaNumber = 10;
                break;
            case 5:
                Main.ArrowCount = 5;
                Main.saplanmaNumber = 5;
                break;
        }

    }
    public void NextLevelButton()
    {
        levelNumber++;
        if (levelNumber >= 10)
        {
            levelNumber = 0;
        }
        SceneManager.LoadScene(accessedLevel - 1);
    }
    public void PreviousLevelButton()
    {
        if (accessedLevel <= levelNumber)
        {
            levelNumber--;
            if (levelNumber <= 0)
            {
                levelNumber = 10;
            }
            SceneManager.LoadScene(accessedLevel - 1);
        }
    }
    void Update()
    {
    }

    
}