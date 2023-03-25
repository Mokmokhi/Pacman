using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour {
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;
    public Color initColor;
    
    public static List<Ghost> GhostList = new List<Ghost>();
    
    private void Awake() {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
        initColor = GetComponent<MeshRenderer>().material.color;
    }

    private void Start() {
        GhostList.Add(this.GetComponent<Ghost>());
        ResetState();
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }
    
    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        ResetState();
    }

    public void ResetState() {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior) {
            home.Disable();
        }

        if (initialBehavior != null) {
            initialBehavior.Enable();
        }
    }

    public void SetPosition(Vector3 position) {
        // Keep the z-position the same since it determines draw depth
        position.y = transform.position.y;
        transform.position = position;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Pacman")) {
            if (frightened.enabled) {
                Eaten();
            } else {
                GameObject.FindWithTag("Pacman").GetComponent<Pacman>().Eaten();
            }
        }
        else if (collision.gameObject.CompareTag("Wall")) return;
        else if (collision.gameObject.GetComponent<Ghost>() != null) {
            movement.SetDirection(-1 * movement.direction, true);
        }
    }
    
    public void Eaten() {
        int points = this.points * GameManager.Instance.ghostMultiplier;
        
        GameManager.Instance.AddScore(points);

        GameManager.Instance.SetGhostMultiplier(GameManager.Instance.ghostMultiplier + 1);
    }

}
