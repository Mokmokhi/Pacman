using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float turnCD = 0.1f;
    [SerializeField]
    private GameObject pacman;

    private Movement movement;
    private void Awake() {
        movement = pacman.GetComponent<Movement>();
    }

    private float timer = 0f;
    private void Update() {
        // Set the new direction based on the current input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            movement.SetDirection(pacman.transform.forward);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            movement.SetDirection(pacman.transform.forward * -1);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            movement.SetDirection(pacman.transform.right * -1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            movement.SetDirection(pacman.transform.right);
        }

        
        if (timer > turnCD) timer = 0f;
        timer += Time.deltaTime;
    }
}
