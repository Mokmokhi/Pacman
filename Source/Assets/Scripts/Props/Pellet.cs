using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Pellet class
 *
 * used for handling pellet behaviour
 */

[RequireComponent(typeof(Collider))]
public class Pellet : MonoBehaviour
{
    public int points = 10; // points for eating this pellet
    public bool isEaten = false;

    protected virtual void Start() {
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }
    
    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        EventBus.Subscribe(GameEvent.STOP, OnGameStop);
        ResetState();
    }
    
    private void OnGameStop() {
        EventBus.Unsubscribe(GameEvent.STOP, OnGameStop);
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }
    
    public void ResetState() {
        if (gameObject.GetComponent<MeshRenderer>() != null) gameObject.GetComponent<MeshRenderer>().enabled = true;
        else gameObject.GetComponent<ParticleSystem>().Play(); // play the particle system for Power Pellet
        gameObject.GetComponent<Collider>().enabled = true;
        isEaten = false;
    }

    protected virtual void Eaten() {
        GameObject.FindWithTag("Pacman").GetComponent<Pacman>().EatPellet(this);
        if (gameObject.GetComponent<MeshRenderer>() != null) gameObject.GetComponent<MeshRenderer>().enabled = false;
        else gameObject.GetComponent<ParticleSystem>().Stop(); // stop the particle system for Power Pellet
        gameObject.GetComponent<Collider>().enabled = false;
        isEaten = true;
        
        AudioManager.Instance.PlaySfx("eatSFX");
        GameManager.Instance.pelletsNum--;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Pacman")) {
            Eaten();
        }
    }
}
