using UnityEngine;

/*
 * Game Manager
 * This class inherits from Singleton and is mainly used for handling
 * 1. In-Game Logic Management
 * 2. Application control
 */

// Difficulty is limited to 3 levels
public enum Difficulty {
    EASY,
    NORMAL,
    HARD
}

public class GameManager : Singleton<GameManager> {
    public Ghost[] ghosts; // an array of all ghosts
    public Pacman pacman; // player pacman
    public Transform pellets; // pellet prefab
    public PlayerController playercontroller; // handles player input
    public int pelletsNum = 1; // number of pellets in the map

    public int ghostMultiplier { get; private set; } = 1; // ghost speed multiplier
    public int score { get; private set; } = 0; // player score
    public int highScore = 0; // player history highest score
    public int lives { get; private set; } = 3; // player lives
    public Difficulty currentDifficulty = Difficulty.NORMAL; // current difficulty
    public bool isPlaying = false; // is the game playing or is pausing
    public GameObject[] Maps; // an array of all maps
    public int currentMapIndex = 0;

    private void Start() {
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("0-Login"); // Switch UI to Login Panel
        highScore = DataBaseManager.Instance.GetComponent<DataBaseManager>().profile.HighestScore; // Get highest score from database
        playercontroller.gameObject.SetActive(false); // Disable player input
        Time.timeScale = 0; // Pause the game
        
        AudioManager.Instance.PlayMusic("bgm1"); 
        var audioClip = Resources.Load<AudioClip>("Audio/bgm1");
        // Debug.Log("LOAD TEST" + audioClip.name);
    }

    private void Update() {
        
        // keep track of the number of pellets
        if (pelletsNum <= 0 && isPlaying) {
            GameSuccess();
        }
        
        // update history high score
        if (score >= highScore) {
            highScore = score;
        }
    }

    public void StartLevel(int mapIndex) {
        
        // Change Map
        currentMapIndex = mapIndex;
        if (mapIndex == 0) {
            Maps[1].SetActive(false);
            Maps[0].SetActive(true);
        }
        else if (mapIndex == 1) {
            Maps[0].SetActive(false);
            Maps[1].SetActive(true);
        }
        // Find number of pellets given that pellets have the tag "Pellet"
        pelletsNum = GameObject.FindGameObjectsWithTag("Pellet").Length;
    
        // reset data
        SetLives(3);
        SetScore(0);
        isPlaying = true;
        EventBus.Subscribe(GameEvent.START, OnGameStart); // subscribe event "START"
        pacman.WearSkin();
        
        // activate all pellets
        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }
        
        // reset pacman and ghosts
        ResetState();
        
        // reset time scale
        Time.timeScale = 1;

        EventBus.Publish(GameEvent.START); // publish event "START"
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2-InGame"); // Switch UI to InGame Panel
        playercontroller.gameObject.SetActive(true); // Enable player input
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
    
    public void SetDifficulty(Difficulty difficulty) {
        Debug.Log("set difficulty: " + difficulty);
        currentDifficulty = difficulty;
    }

    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart); // When Game is started, unsubscribe event "START"
        Time.timeScale = 1;
        
        // Find number of pellets given that pellets have the tag "Pellet"
        pelletsNum = GameObject.FindGameObjectsWithTag("Pellet").Length;
        
        SetLives(3);
        SetScore(0);
        highScore = DataBaseManager.Instance.GetComponent<DataBaseManager>().profile.HighestScore;
        UIManager.Instance.ResetInGameUI();
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("Demented-Nightmare-MP3"); // change bgm
        
        // Subscribe event "PAUSE" and "STOP"
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
        EventBus.Unsubscribe(GameEvent.PAUSE, OnGamePause);
        isPlaying = false;
        playercontroller.gameObject.SetActive(false); // Disable player input
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("bgm1"); // change bgm
        Time.timeScale = 0;
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }

    private void GameSuccess() {
        isPlaying = false;
        SaveScoreAndCoins(); // Save score and coins to database
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2.3-GameSuccess");
        EventBus.Publish(GameEvent.STOP);
    }

    public void GameOver() {
        isPlaying = false;
        AudioManager.Instance.PlaySfx("screamSFX");
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2.2-GameOver");
        EventBus.Publish(GameEvent.STOP);
    }

    public void RestartGame(){
        EventBus.Publish(GameEvent.STOP);
        StartLevel(currentMapIndex);
    }

    public void SaveScoreAndCoins() {
        DataBaseManager.Instance.GetComponent<LeaderBoardManager>().UpdateScore(score);
        DataBaseManager.Instance.GetComponent<ShopManager>().SaveCoins(
            DataBaseManager.Instance.GetComponent<ShopManager>().CalCoins(score, lives));
    }

    public void ResumeGame(){
        EventBus.Publish(GameEvent.RESUME);
        FindObjectOfType<PanelSwitcher>().SwitchActivePanelByName("2-InGame");
    }
    
    public void GoToMenu(){
        EventBus.Publish(GameEvent.STOP); // publish event "STOP"
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("1-Main");
    }

    public void QuitApp(){
        DataBaseManager.Instance.GetComponent<DataBaseManager>().SaveAndQuit();
    }

}
