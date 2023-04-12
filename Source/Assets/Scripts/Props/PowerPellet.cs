using UnityEngine;

public class PowerPellet : Pellet {
    public int level = 1;
    public float duration = 8f;
    
    // level would be changed by shop function
    // duration would be updated when level is changed

    protected override void Start() {
        base.Start();
    }

    protected override void Eaten() {
        base.Eaten();
        GameObject.FindWithTag("Pacman").GetComponent<Movement>().speedMultiplier=1f+level*0.1f;
        GameObject.FindWithTag("Pacman").GetComponent<Pacman>().EatPowerPellet(this);
        GameObject.FindWithTag("Pacman").GetComponent<Movement>().speedMultiplier=1f;
    }
}
