using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public List<Vector3> availableDirections { get; private set; }

    private void Start()
    {
        availableDirections = new List<Vector3>();

        // We determine if the direction is available by box casting to see if
        // we hit a wall. The direction is added to list if available.
        CheckAvailableDirection(new Vector3(1,0,0));
        CheckAvailableDirection(new Vector3(-1,0,0));
        CheckAvailableDirection(new Vector3(0,0,1));
        CheckAvailableDirection(new Vector3(0,0,-1));
    }

    private void CheckAvailableDirection(Vector3 direction) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out hit, 1f)) {
            if (hit.collider.CompareTag("Wall")) {
                return;
            }
        }
        availableDirections.Add(direction);
    }

}
