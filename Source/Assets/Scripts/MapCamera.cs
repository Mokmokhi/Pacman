using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour {
    private Transform pacman;
    private Vector3 offset = new Vector3(0, 10, 1);
    private Camera mapCam;
    void Start() {
        pacman = GameObject.FindWithTag("Pacman").transform;
        mapCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        transform.position = pacman.position + offset;
        if (GameManager.Instance.currentDifficulty == Difficulty.HARD) {
            mapCam.orthographicSize = 3;
        } else if (GameManager.Instance.currentDifficulty == Difficulty.NORMAL) {
            mapCam.orthographicSize = 5;
        } else if (GameManager.Instance.currentDifficulty == Difficulty.EASY) {
            mapCam.orthographicSize = 6;
        }
    }
}
