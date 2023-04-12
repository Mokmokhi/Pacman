using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour {
    private Transform pacman;
    private Vector3 offset = new Vector3(0, 10, 3);
    void Start() {
        pacman = GameObject.FindWithTag("Pacman").transform;
    }

    // Update is called once per frame
    void Update() {
        transform.position = pacman.position + offset;
    }
}
