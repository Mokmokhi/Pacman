using System;
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public bool eaten { get; private set; }
    
    public override void Enable(float duration) {
        base.Enable(duration);
        Invoke(nameof(Flash), duration / 2f);
    }

    public override void Disable()
    {
        base.Disable();
    }

    private void Eaten()
    {
        eaten = true;
        ghost.SetPosition(ghost.home.inside.position);
        ghost.home.Enable(duration);
    }

    private void Flash()
    {
        if (!eaten) {
            // white.GetComponent<AnimatedSprite>().Restart();
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    private void OnEnable()
    {
        // blue.GetComponent<AnimatedSprite>().Restart();
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        ghost.movement.speedMultiplier = 0.5f;
        eaten = false;

        // Print "ghost.frightened.enabled" to the console with time stamp
        //Debug.Log(Time.time + ": " + "ghost.frightened.enabled: " + enabled);
    }

    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f;
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = ghost.initColor;
        eaten = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector3 direction = Vector3.zero;
            float maxDistance = float.MinValue;

            // Find the available direction that moves farthest from pacman
            foreach (Vector3 availableDirection in node.availableDirections)
            {
                // If the distance in this direction is greater than the current
                // max distance then this direction becomes the new farthest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.z);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            // Print the direction to the console
            //Debug.Log(Time.time + ": " + "Frightened Direction: " + direction);

            ghost.movement.SetDirection(direction);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pacman")) {
            if (enabled) {
                Eaten();
            }
        }
    }

}
