using UnityEngine;

/*
 * MainCamera class
 *
 * used for controlling the main render camera
 */

public class MainCamera : MonoBehaviour {
    
    private Vector3 pacmanInitPosition; // pacman's initial position
    private Transform pacman; // pacman transform (e.g. position, rotation, scale)
    private Vector3 offset; // offset between pacman and pacman initial position

    void Start() {
        pacman = GameObject.FindWithTag("Pacman").transform;
        pacmanInitPosition = pacman.position;
        offset = pacman.position - pacmanInitPosition;
    }

    private float timer;
    void Update() {
        transform.position = pacman.position - offset + new Vector3(1,1.5f,-1); // the last vector is the offset between pacman and camera
        
        // vibrate position.y linearly slowly irregularly
        timer += Time.deltaTime;
        timer += Random.Range(-0.005f, 0.005f);
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(timer * 4) * 0.02f, transform.position.z);
        
        // reset Timer
        if (timer > 2 * Mathf.PI) timer = 0;
    }
}
