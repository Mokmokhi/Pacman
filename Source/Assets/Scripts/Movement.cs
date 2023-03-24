using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector3 initialDirection;

    public Rigidbody rigidbody { get; private set; }
    public Vector3 direction { get; private set; }
    public Vector3 nextDirection { get; private set; }
    public Vector3 rotation { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    private void Start() {
        ResetState();
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector3.zero;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }

    private void Update() {
        if (nextDirection != Vector3.zero) {
            SetDirection(nextDirection);
        }
    }
    private void FixedUpdate() {
        Vector3 position = rigidbody.position;
        Vector3 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        //Vector3 translation = transform.forward.normalized * (speed * speedMultiplier * Time.deltaTime);
        rigidbody.MovePosition(position + translation);
        // rigidbody.MoveRotation(Quaternion.Euler(rotation));
        
        // Turn the player to face the direction he is moving slowly
        if ((transform.eulerAngles - rotation).magnitude > 0.05f) {
            Debug.Log("euler angle " + transform.eulerAngles);
            Debug.Log("rotation " + rotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.5f);
        }
            
    }

    public void SetDirection(Vector3 targetDir, bool forced = false) {

        if (isBlocked(targetDir)) {
            nextDirection = targetDir;
        }
        else {
            direction = targetDir;
            nextDirection = targetDir;
            rotation = new Vector3(0, Mathf.Atan2(direction.z, -1 * direction.x) * Mathf.Rad2Deg - 90, 0);
            if (rotation.y < 0) rotation += new Vector3(0, 360, 0);
            if (rotation.y >= 360) rotation -= new Vector3(0, 360, 0);
            // Force rotation.y to becomes 0 or 90 or 180 or 270
            rotation = new Vector3(0, Mathf.Round(rotation.y / 90) * 90, 0);
        }
    }

    private float rayOffset = 0.3f;
    public bool isBlocked(Vector3 testDirection) {
        bool result0 = Physics.Raycast(transform.position + new Vector3(rayOffset, 0, rayOffset), testDirection, 0.5f);
        bool result1 = Physics.Raycast(transform.position + new Vector3(rayOffset, 0, -1*rayOffset), testDirection, 0.5f);
        bool result2 = Physics.Raycast(transform.position + new Vector3(-1*rayOffset, 0, rayOffset), testDirection, 0.5f);
        bool result3 = Physics.Raycast(transform.position + new Vector3(-1*rayOffset, 0, -1*rayOffset), testDirection, 0.5f);
        
        bool result = result0 || result1 || result2 || result3;
        return result;
    }

}
