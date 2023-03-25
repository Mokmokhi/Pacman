using System;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour {
    public new Collider collider { get; private set; }
    public Movement movement { get; private set; }

    private void Awake() {
        collider = GetComponent<Collider>();
        movement = GetComponent<Movement>();
    }

    private void Start() {
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }
    
    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        ResetState();
    }

    public void ResetState() {
        enabled = true;
        collider.enabled = true;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    private void DeathSequence() {
        enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        ResetState();
    }

    public void EatPellet(Pellet pellet ) {
        GameManager.Instance.AddScore(pellet.points);
    }
    
    public void EatPowerPellet(PowerPellet powerPellet ) {
        Debug.Log("Pacman Eaten Power Pellet!");
        Ghost.GhostList.ForEach(ghost => ghost.frightened.Enable(powerPellet.duration));
        EatPellet(powerPellet);
    }
    
    public void Eaten() {
        Debug.Log("Pacman Eaten!");
        this.DeathSequence();
        GameManager.Instance.AddLives(-1);
    }
}
