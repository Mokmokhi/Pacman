using UnityEngine;

public class MainCamera : MonoBehaviour {
    
    private Vector3 pacmanInitPosition;
    private Transform pacman;
    private Vector3 offset;

    void Start() {
        pacman = GameObject.FindWithTag("Pacman").transform;
        pacmanInitPosition = pacman.position;
        offset = pacman.position - pacmanInitPosition;
        Debug.Log("pacmanInitPosition: " + pacmanInitPosition + "");
        Debug.Log("pacman.position: " + pacman.position + "");
        Debug.Log("offset = " + (pacman.position - pacmanInitPosition));
    }

    void Update() {
        transform.position = pacman.position - offset + new Vector3(1,1.5f,-1);
    }
}
