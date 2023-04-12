using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        ghost.scatter.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the ghost is frightened
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector3 direction = Vector3.zero;
            float minDistance = float.MaxValue;

            // Find the available direction that moves closest to Pac-Man
            foreach (Vector3 availableDirection in node.availableDirections)
            {
                // Ignore the direction that would make the ghost move back to its previous position
                if (availableDirection == -ghost.movement.direction)
                    continue;

                // Print the available direction to the console
                Debug.Log("Available Direction: " + availableDirection);

                // If the distance in this direction is less than the current
                // min distance, then this direction becomes the new closest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, 0, availableDirection.z);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            // Print the direction to the console
            //Debug.Log(Time.time + ": " + "Chase Direction: " + direction);

            ghost.movement.SetDirection(direction);
        }
    }


}
