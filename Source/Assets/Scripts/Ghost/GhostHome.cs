using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostHome : GhostBehavior
{
    public Transform inside;
    public Transform outside;

    private void Start() {
        inside = GameObject.FindWithTag("GhostHome").transform.GetChild(0).transform;
        outside = GameObject.FindWithTag("GhostHome").transform.GetChild(1).transform;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        ghost.movement.speedMultiplier = 2f;
    }

    private void OnDisable()
    {
        // Check for active self to prevent error when object is destroyed
        if (gameObject.activeInHierarchy) {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // Reverse direction everytime the ghost hits a wall to create the
        // effect of the ghost bouncing around the home
        if (enabled && collision.gameObject.CompareTag("Wall")) {
            ghost.movement.SetDirection(-ghost.movement.direction, true);
        }
    }

    private IEnumerator ExitTransition()
    {
        // Turn off movement while we manually animate the position
        ghost.movement.SetDirection(new Vector3(0,0,1), true);
        ghost.movement.rigidbody.isKinematic = true;
        ghost.movement.enabled = false;
        ghost.GetComponent<Collider>().enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0f;

        // Animate to the starting point
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Animate exiting the ghost home
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Pick a random direction left or right and re-enable movement
        ghost.movement.speedMultiplier = 1f;
        ghost.GetComponent<Collider>().enabled = true;
        ghost.movement.SetDirection(new Vector3(Random.value < 0.5f ? -1f : 1f, 0f, 0), true);
        ghost.movement.rigidbody.isKinematic = false;
        ghost.movement.enabled = true;
    }

}
