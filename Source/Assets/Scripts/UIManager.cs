using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * UIManager class
 *
 * used for managing UI elements and UI elements' events
 */

public class UIManager : Singleton<UIManager> {
    private PanelSwitcher panelSwitcher;
    public TMP_Text scoreText;
    // public TMP_Text livesText;
    public TMP_Text coinText;
    public TMP_Text powerText;
    public GameObject livesHearts;
    public TMP_Text highScoreText;
    public Toggle[] difficultyToggles;

    private void Start() {
        panelSwitcher = GetComponent<PanelSwitcher>();
        
        // listen to the difficulty buttons click event
        difficultyToggles[0].onValueChanged.AddListener(delegate { OnDifficultyToggleChanged(0); });
        difficultyToggles[1].onValueChanged.AddListener(delegate { OnDifficultyToggleChanged(1); });
        difficultyToggles[2].onValueChanged.AddListener(delegate { OnDifficultyToggleChanged(2); });
        
        AddSFXToAllButtons(); // add sfx to all buttons when clicked
    }

    private void Update() {
        powerText.text = "Powerlevel " + DataBaseManager.Instance.profile.PowerLevel.ToString();;
        coinText.text = "Coin " + DataBaseManager.Instance.profile.Coins.ToString();
        scoreText.text = "" + GameManager.Instance.score.ToString();
        highScoreText.text = "Record " + GameManager.Instance.highScore.ToString();
        if (GameManager.Instance.lives == 2) livesHearts.transform.GetChild(2).GetComponent<Image>().enabled = false;
        if (GameManager.Instance.lives == 1) livesHearts.transform.GetChild(1).GetComponent<Image>().enabled = false;
        if (GameManager.Instance.lives == 0) livesHearts.transform.GetChild(0).GetComponent<Image>().enabled = false;
    }

    public void ResetInGameUI() {
        // reset lives
        for (int i = 0; i < livesHearts.transform.childCount; i++) {
            livesHearts.transform.GetChild(i).GetComponent<Image>().enabled = true;
        }
    }
    
    // change the difficulty of the game
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
    
    // show the game over panel
    public void ShowRespawning() {
        panelSwitcher.SwitchActivePanelByName("2.4-Respawning");
        Invoke(nameof(AddDot), 1);
        Invoke(nameof(AddDot), 2);
        StartCoroutine(Wait3SecondsToReset());
    }
    // wait 3 seconds to reset the UI panel to InGame panel
    IEnumerator Wait3SecondsToReset() {
        yield return new WaitForSeconds(3);
        string text =
            panelSwitcher.GetPanelByName("2.4-Respawning").transform.GetChild(1).GetComponent<TMP_Text>().text =
                "Respawning.";
        panelSwitcher.SwitchActivePanelByName("2-InGame");
    }

    // for the respawning panel, add a dot to the text
    private void AddDot() {
        panelSwitcher.GetPanelByName("2.4-Respawning").transform.GetChild(1).GetComponent<TMP_Text>().text += ".";
    }
    
    private void AddSFXToAllButtons() {
        foreach (var button in Resources.FindObjectsOfTypeAll<Button>()) {
            button.onClick.AddListener(() => AudioManager.Instance.PlaySfx("button1SFX"));
        }
    }
    
    // determine to go to main menu or pause menu when setting is finished
    public void FinishSetting() {
        if (GameManager.Instance.isPlaying) {
            panelSwitcher.SwitchActivePanelByName("2.1-PauseMenu");
        } else {
            panelSwitcher.SwitchActivePanelByName("1-Main");
        }
    }

}