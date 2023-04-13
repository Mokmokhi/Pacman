using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
    private PanelSwitcher panelSwitcher;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text highScoreText;
    public Toggle[] difficultyToggles;

    private void Start() {
        panelSwitcher = GetComponent<PanelSwitcher>();
        difficultyToggles[0].onValueChanged.AddListener(delegate { OnDifficultyToggleChanged(0); });
        difficultyToggles[1].onValueChanged.AddListener(delegate { OnDifficultyToggleChanged(1); });
        difficultyToggles[2].onValueChanged.AddListener(delegate { OnDifficultyToggleChanged(2); });
    }

    private void Update() {
        scoreText.text = "Score:" + GameManager.Instance.score.ToString();
        livesText.text = "Lives:" + GameManager.Instance.lives.ToString();
        highScoreText.text = "High Score:" + GameManager.Instance.highScore.ToString();
    }
    
    public void OnDifficultyToggleChanged(int index) {
        if (difficultyToggles[index].isOn) {
            for (int i = 0; i < difficultyToggles.Length; i++) {
                if (i != index) {
                    difficultyToggles[i].isOn = false;
                }
            }
        }
        if (difficultyToggles[0].isOn) {
            GameManager.Instance.SetDifficulty(Difficulty.EASY);
        } else if (difficultyToggles[1].isOn) {
            GameManager.Instance.SetDifficulty(Difficulty.NORMAL);
        } else if (difficultyToggles[2].isOn) {
            GameManager.Instance.SetDifficulty(Difficulty.HARD);
        }
    }

    public void ShowRespawning() {
        panelSwitcher.SwitchActivePanelByName("2.4-Respawning");
        Invoke(nameof(AddDot), 1);
        Invoke(nameof(AddDot), 2);
        StartCoroutine(Wait3SecondsToReset());
    }
    IEnumerator Wait3SecondsToReset() {
        yield return new WaitForSeconds(3);
        string text =
            panelSwitcher.GetPanelByName("2.4-Respawning").transform.GetChild(1).GetComponent<TMP_Text>().text =
                "Respawning.";
        panelSwitcher.SwitchActivePanelByName("2-InGame");
    }

    private void AddDot() {
        panelSwitcher.GetPanelByName("2.4-Respawning").transform.GetChild(1).GetComponent<TMP_Text>().text += ".";
    }
}
