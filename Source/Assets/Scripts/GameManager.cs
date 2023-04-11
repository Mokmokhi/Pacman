using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int highScore = 0;
    public int lives { get; private set; }
    public int pellets;

    
    private Level[] levels;
    public Level currentLevel;

    public bool isLose = true;
    public bool isFinish = false;

    public override void Awake() {
        base.Awake();
        levels = FindObjectsOfType<Level>();
        currentLevel = levels[0]; // testing
    }

    private void Start() {
        // Find number of pellets given that pellets have the tag "Pellet"
        pellets = GameObject.FindGameObjectsWithTag("Pellet").Length;


        SetLives(3);
        SetScore(0);
        Time.timeScale = 0;
        EventBus.Subscribe(GameEvent.START, OnGameStart);
        // EventBus.Publish(GameEvent.START);
    }

    private void Update() {
        if (pellets <= 0 && !isFinish) {
            GameSuccess();
        }
        if (lives <= 0 && !isFinish) {
            GameOver();
        }
    }
    
    public void SetGhostMultiplier(int ghostMultiplier) {
        this.ghostMultiplier = ghostMultiplier;
    }
    
    private void SetLives(int lives) {
        this.lives = lives;
    }
    
    public void AddLives(int p_lives) {
        this.lives += p_lives;
    }

    private void SetScore(int score) {
        this.score = score;
    }

    public void AddScore(int p_score) {
        this.score += p_score;
    }

    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        Time.timeScale = 1;
        // Find number of pellets given that pellets have the tag "Pellet"
        pellets = GameObject.FindGameObjectsWithTag("Pellet").Length;

        SetLives(3);
        SetScore(0);
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
        EventBus.Subscribe(GameEvent.STOP, OnGameStop);
    }
    
    private void OnGamePause() {
        EventBus.Unsubscribe(GameEvent.PAUSE, OnGamePause);
        Time.timeScale = 0;
        EventBus.Subscribe(GameEvent.RESUME, OnGameResume);
    }
    
    private void OnGameResume() {
        EventBus.Unsubscribe(GameEvent.RESUME, OnGameResume);
        Time.timeScale = 1;
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
    }

    private void OnGameStop() {
        EventBus.Unsubscribe(GameEvent.STOP, OnGameStop);
        Time.timeScale = 0;
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }

    private void GameSuccess() {
        isLose = false;
        isFinish = true;
        SaveHighScore();
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2.3-GameSuccess");
        EventBus.Publish(GameEvent.STOP);
    }

    private void GameOver() {
        isLose = true;
        isFinish = true;
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2.2-GameOver");
        EventBus.Publish(GameEvent.STOP);
    }
    
    public void StartGame() {
        EventBus.Publish(GameEvent.START);
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2-InGame");
    }

    // After a level is completed, save the high score
    public void SaveHighScore() {
        if (score > highScore) {
            highScore = score;
        }
    }
}
