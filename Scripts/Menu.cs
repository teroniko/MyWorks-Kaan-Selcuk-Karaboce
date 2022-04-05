using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public GameObject High_Score, Score;
    public int high_score = 0, score;
    private void Start()
    {
        High_Score.GetComponent<Text>().text = "High Score : "+ PlayerPrefs.GetInt("highscore", high_score);
        Score.GetComponent<Text>().text = "Score : " + (int)Main.score0 + "";
    }
    public void Play() {
        SceneManager.LoadScene("Game");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
