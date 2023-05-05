using System;
using UnityEngine;

/*
 * PlayerController class
 *
 * used for controlling the pacman and handling input
 */

public class PlayerController : MonoBehaviour {
    public float turnCD = 0.1f; // turn cooldown (not using)
    [SerializeField]
    private GameObject pacman; // pacman game object

    private Movement movement; // pacman movement script
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
        else if(Input.GetKeyDown(KeyCode.Space)) {
            if (FindObjectOfType<Pacman>().isEaten) return;
            EventBus.Publish(GameEvent.PAUSE);
            FindObjectOfType<PanelSwitcher>().SwitchActivePanelByName("2.1-PauseMenu");
        }

        // turn cooldown
        if (timer > turnCD) timer = 0f;
        timer += Time.deltaTime;
    }
}
