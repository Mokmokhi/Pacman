using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * MapCamera class
 *
 * used for controlling the minimap camera
 */

public class MapCamera : MonoBehaviour {
    private Transform pacman; // get pacman position
    private Vector3 offset = new Vector3(0, 10, 1);
    private Camera mapCam;
    void Start() {
        pacman = GameObject.FindWithTag("Pacman").transform;
        mapCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        transform.position = pacman.position + offset;
        
        // change camera size according to difficulty
        // Higher the orthographic size, the more zoomed out the camera is ( see more )
        if (GameManager.Instance.currentDifficulty == Difficulty.HARD) {
            mapCam.orthographicSize = 3;
        } else if (GameManager.Instance.currentDifficulty == Difficulty.NORMAL) {
            mapCam.orthographicSize = 5;
        } else if (GameManager.Instance.currentDifficulty == Difficulty.EASY) {
            mapCam.orthographicSize = 6;
        }
    }
}
