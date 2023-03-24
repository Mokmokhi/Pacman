using UnityEngine;

public class Ghost : MonoBehaviour {
    
    public Movement movement { get; private set; }
    public Transform target;
    public int points = 200;
    
    private void Awake() {
        movement = GetComponent<Movement>();
    }

}
