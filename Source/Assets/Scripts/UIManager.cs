using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text highScoreText;

    private void Update() {
        scoreText.text = "Score:" + GameManager.Instance.score.ToString();
        livesText.text = "Lives:" + GameManager.Instance.lives.ToString();
        highScoreText.text = "High Score:" + GameManager.Instance.highScore.ToString();
    }
    
    public void QuitApp(){
        Application.Quit();
    }
}
