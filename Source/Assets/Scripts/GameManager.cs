using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start() {
        EventBus.Subscribe(GameEvent.START, OnGameStart);
        EventBus.Publish(GameEvent.START);
    }

    private void Update() {
        if (lives <= 0 && Input.anyKeyDown) {
           
        }
    }
    
    private void SetLives(int lives) {
        this.lives = lives;
    }

    private void SetScore(int score) {
        this.score = score;
    }

    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        SetLives(3);
        SetScore(0);
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
    }
    
    private void OnGamePause() {
        EventBus.Unsubscribe(GameEvent.PAUSE, OnGamePause);
        EventBus.Subscribe(GameEvent.RESUME, OnGameResume);
    }
    
    private void OnGameResume() {
        EventBus.Unsubscribe(GameEvent.RESUME, OnGameResume);
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
    }

    private void OnGameStop() {
        EventBus.Unsubscribe(GameEvent.STOP, OnGameStop);
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }
}
