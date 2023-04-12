using Unity.VisualScripting;
using UnityEngine;

public class PowerPellet : Pellet {
    public float duration = 8f;

    
    protected override void Eaten() {
        GameObject.FindWithTag("Pacman").GetComponent<Pacman>().EatPowerPellet(this);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        isEaten = true;
        GameManager.Instance.pelletsNum--;
    }

}
