using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty {
    EASY,
    NORMAL,
    HARD
}

public class GameManager : Singleton<GameManager> {
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public PlayerController playercontroller;
    public int pelletsNum = 1;

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; } = 0;
    public int highScore = 0;
    public int lives { get; private set; } = 3;

    public Difficulty currentDifficulty = Difficulty.NORMAL;

    public bool isLose = false;
    public bool isPlaying = false;

    public GameObject[] Maps;
    public int currentMapIndex = 0;

    public override void Awake() {
        base.Awake();
    }

    private void Start() {
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("0-Login");
        highScore = DataBaseManager.Instance.GetComponent<DataBaseManager>().profile.HighestScore;
        playercontroller.gameObject.SetActive(false);
        Time.timeScale = 0;
        
        AudioManager.Instance.PlayMusic("bgm1");
        var audioClip = Resources.Load<AudioClip>("Audio/bgm1");
        Debug.Log("LOAD TEST" + audioClip.name);
    }

    private void Update() {
        if (pelletsNum <= 0 && isPlaying) {
            GameSuccess();
        }
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

        SetLives(3);
        SetScore(0);
        isLose = false;
        isPlaying = true;
        EventBus.Subscribe(GameEvent.START, OnGameStart);
        pacman.WearSkin();
        
        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
        
        Time.timeScale = 1;

        EventBus.Publish(GameEvent.START);
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2-InGame");
        playercontroller.gameObject.SetActive(true);
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
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        Time.timeScale = 1;
        // Find number of pellets given that pellets have the tag "Pellet"
        pelletsNum = GameObject.FindGameObjectsWithTag("Pellet").Length;
        SetLives(3);
        SetScore(0);
        highScore = DataBaseManager.Instance.GetComponent<DataBaseManager>().profile.HighestScore;
        UIManager.Instance.ResetInGameUI();
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("Demented-Nightmare-MP3");
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
        playercontroller.gameObject.SetActive(false);
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("bgm1");
        Time.timeScale = 0;
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }

    private void GameSuccess() {
        isLose = false;
        isPlaying = false;
        SaveScoreAndCoins();
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("2.3-GameSuccess");
        EventBus.Publish(GameEvent.STOP);
    }

    public void GameOver() {
        isLose = true;
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
        EventBus.Publish(GameEvent.STOP);
        UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("1-Main");
    }

    public void QuitApp(){
        DataBaseManager.Instance.GetComponent<DataBaseManager>().SaveAndQuit();
    }

}
