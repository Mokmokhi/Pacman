using UnityEngine;

public class PowerPellet : Pellet {
    public float duration = 8f;

    protected override void Eaten() {
        GameObject.FindWithTag("Pacman").GetComponent<Pacman>().EatPowerPellet(this);
        gameObject.SetActive(false);
    }

}
