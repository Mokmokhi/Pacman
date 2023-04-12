using System.Net.NetworkInformation;
using System.Linq.Expressions;
using UnityEngine;

public class PowerPellet : Pellet {
    public int level = 1;
    public float duration = 8f+level*1f;

    // level would be changed by shop function
    // duration would be updated when level is changed

    override void start{
        EventBus.Subscribe(GameEvent.START, OnGameStart);
        // get level from database
    }

    protected override void Eaten() {
        GameObject.FindWithTag("Pacman").GetComponent<Movement>().speedMultiplier=1f+level*0.1f;
        GameObject.FindWithTag("Pacman").GetComponent<Pacman>().EatPowerPellet(this);
        GameObject.FindWithTag("Pacman").GetComponent<Movement>().speedMultiplier=1f;
        gameObject.SetActive(false);
    }
}
