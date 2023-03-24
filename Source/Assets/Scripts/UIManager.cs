using System;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager> {
    public TMP_Text scoreText;
    public TMP_Text livesText;

    private void Update() {
        scoreText.text = "Score:"+GameManager.Instance.score.ToString();
        livesText.text = "Lives:"+GameManager.Instance.lives.ToString();
    }
}
