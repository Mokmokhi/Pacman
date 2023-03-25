using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pellet : MonoBehaviour
{
    public int points = 10;
    public bool isEaten { get; private set; } = false;

    protected virtual void Eaten() {
        GameObject.FindWithTag("Pacman").GetComponent<Pacman>().EatPellet(this);
        gameObject.SetActive(false);
        isEaten = true;
        GameManager.Instance.pellets--;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Pacman")) {
            Eaten();
        }
    }
}
