using System;
using UnityEngine;

public enum AnimationState {
    IDLE,
    WALK,
    DIE
}

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour {
    public new Collider collider { get; private set; }
    public Movement movement { get; private set; }
    
    public AnimationState animationState { get; private set; }
    
    public bool isEaten = false;

    private void Awake() {
        collider = GetComponent<Collider>();
        movement = GetComponent<Movement>();
        animationState = AnimationState.IDLE;
    }

    private void Start() {
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }

    private void Update() {
        if (animationState == AnimationState.WALK) {
            BiteAnimation();
        }
    }
    
    private void BiteAnimation() {
        // to be continue
    }

    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        ResetState();
    }

    public void ResetState() {
        enabled = true;
        collider.enabled = true;
        movement.enabled = true;
        isEaten = false;
        movement.ResetState();
        switch (GameManager.Instance.currentDifficulty) {
            case Difficulty.EASY:
                movement.speed = 2f;
                break;
            default:
                movement.speed = 2f;
                break;
        }
        gameObject.SetActive(true);
        animationState = AnimationState.WALK;
    }

    private void DeathSequence() {
        enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        //ResetState();
        animationState = AnimationState.DIE;
    }

    public void EatPellet(Pellet pellet ) {
        GameManager.Instance.AddScore(pellet.points);
    }
    
    public void EatPowerPellet(PowerPellet powerPellet ) {
        Debug.Log("Pacman Eaten Power Pellet!");
        Ghost.GhostList.ForEach(ghost => ghost.frightened.Enable(powerPellet.duration));
        // EatPellet(powerPellet);
    }
    
    /*
    public void Eaten() {
        Debug.Log("Pacman Eaten!");
        this.DeathSequence();
        GameManager.Instance.AddLives(-1);
    }
    */

    public void Eaten() {
        if (isEaten) {
            return;
        }
        Debug.Log("Pacman Eaten!");
        isEaten = true;

        this.DeathSequence();

        GameManager.Instance.AddLives(-1);

        if (GameManager.Instance.lives > 0) {
            Invoke(nameof(GameManager.Instance.ResetState), 3f);
            UIManager.Instance.ShowRespawning();
        } else {
            GameManager.Instance.GameOver();
        }
    }
}
