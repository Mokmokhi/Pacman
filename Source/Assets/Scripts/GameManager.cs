using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public int pelletsNum = 1;

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; } = 0;
    public int highScore = 0;
    public int lives { get; private set; } = 3;
    
    private Level[] levels;
    public Level currentLevel;

    public bool isLose = false;
    public bool isFinish = false;

    public override void Awake() {
        base.Awake();
        levels = FindObjectsOfType<Level>();
    }

    private void Start() {
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("0-Login");
       
        Time.timeScale = 0;
    }

    private void Update() {
        if (pelletsNum <= 0 && !isFinish) {
            GameSuccess();
        }
    }

    public void StartLevel(int level) {
        currentLevel = levels[level];

        // Find number of pellets given that pellets have the tag "Pellet"
        pelletsNum = GameObject.FindGameObjectsWithTag("Pellet").Length;

        SetLives(3);
        SetScore(0);
        isLose = false;
        isFinish = false;
        EventBus.Subscribe(GameEvent.START, OnGameStart);
        
        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
        
        Time.timeScale = 1;

        EventBus.Publish(GameEvent.START);
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2-InGame");
    }

    public void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
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
        pelletsNum = GameObject.FindGameObjectsWithTag("Pellet").Length;

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

    public void GameOver() {
        isLose = true;
        isFinish = true;
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2.2-GameOver");
        EventBus.Publish(GameEvent.STOP);
    }

    public void SaveHighScore() {
        if (score > highScore) {
            highScore = score;
        }
    }

    public void QuitApp(){
        Application.Quit();
    }

}
