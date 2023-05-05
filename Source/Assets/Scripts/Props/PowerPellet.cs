using UnityEngine;

/*
 * PowerPellet class
 *
 * used for powerpellet Behaviour
 * inherited from Pellet class
 */

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
        AudioManager.Instance.PlaySfx("eatPowerPelletSFX");
        GameObject.FindWithTag("Pacman").GetComponent<Movement>().speedMultiplier=1f+level*0.1f; // speed up
        GameObject.FindWithTag("Pacman").GetComponent<Pacman>().EatPowerPellet(this); // eat powerpellet
        GameObject.FindWithTag("Pacman").GetComponent<Movement>().speedMultiplier=1f; // reset speed
    }
}
