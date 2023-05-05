using System;
using UnityEngine;

/*
 * Pacman class
 *
 * used for pacman behaviour
 */

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

    public GameObject[] PacSkin;
    
    public bool isEaten = false;

    private void Awake() {
        collider = GetComponent<Collider>();
        movement = GetComponent<Movement>();
        animationState = AnimationState.IDLE;
    }

    private void Start() {
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }
    
    // chage the skin of pacman
    public void WearSkin() {
        // to be continue
        for (int i = 0; i < PacSkin.Length; i++) {
            if (i == DataBaseManager.Instance.profile.UsingSkin) {
                PacSkin[i].gameObject.SetActive(true);
            } else PacSkin[i].gameObject.SetActive(false);
        }
    }

    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        ResetState();
    }

    // reset the state of pacman
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

    // death sequence of pacman
    private void DeathSequence() {
        enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        //ResetState();
        animationState = AnimationState.DIE;
        // Diminish the size of the Pacman in 3 seconds using IEnumerator
        StartCoroutine(ScaleTo(gameObject, Vector3.zero, 3f));
    }
    
    // IEnumerator for diminishing the size of the Pacman and keep rotating when it dies
    private System.Collections.IEnumerator ScaleTo(GameObject obj, Vector3 targetScale, float duration) {
        Vector3 originalScale = obj.transform.localScale;
        float counter = 0f;
        while (counter < duration) {
            counter += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(originalScale, targetScale, counter / duration);
            obj.transform.eulerAngles = new Vector3(0, 0, obj.transform.eulerAngles.z + 360 * Time.deltaTime);
            yield return null;
        }
    }

    public void EatPellet(Pellet pellet ) {
        GameManager.Instance.AddScore(pellet.points);
    }
    
    public void EatPowerPellet(PowerPellet powerPellet ) {
        // Debug.Log("Pacman Eaten Power Pellet!");
        float amp = 1 + DataBaseManager.Instance.profile.PowerLevel * 0.05f;
        Ghost.GhostList.ForEach(ghost => ghost.frightened.Enable(powerPellet.duration * amp));
    }

    // when pacman is eaten by ghost
    public void Eaten() {
        if (isEaten) {
            return;
        }
        // Debug.Log("Pacman Eaten!");
        isEaten = true;
        
        GameManager.Instance.AddLives(-1);

        if (GameManager.Instance.lives > 0) {
            this.DeathSequence();
            Invoke(nameof(GameManager.Instance.ResetState), 3f);
            UIManager.Instance.ShowRespawning();
        } else {
            GameManager.Instance.GameOver();
        }
    }
}
