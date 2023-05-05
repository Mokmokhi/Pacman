using UnityEngine;

/*
 * Level class
 * not used currently
 *
 * used for storing level information
 */

public class Level : MonoBehaviour {
    public int index;
    public int numberOfGhosts;
    public int numberOfPellets;
    public Vector3 pacmanSpawnPosition;
    public Vector3[] ghostSpawnPositions;
}
