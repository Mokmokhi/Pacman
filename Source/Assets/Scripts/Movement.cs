using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector3 initialDirection;

    public new Rigidbody rigidbody { get; private set; }
    public Animator animator { get; private set; }
    public Vector3 direction { get; private set; }
    public Vector3 nextDirection { get; private set; }
    public Vector3 rotation { get; private set; }
    public Vector3 startingPosition { get; private set; }
    public Vector3 startingRotation { get; private set; }
    public Vector3 startingScale { get; private set; }

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        try {animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();}
        catch (Exception e) { Debug.Log(e);}
        startingPosition = transform.position;
        startingRotation = transform.eulerAngles;
        startingScale = transform.localScale;
    }

    private void Start() {
        this.enabled = false;
        ResetState();
        
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }
    
    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        ResetState();
    }

    public void ResetState() {
        transform.position = startingPosition;
        transform.rotation = Quaternion.Euler(startingRotation);
        transform.localScale = startingScale;
        speedMultiplier = 1f;
        SetDirection(initialDirection, true);
        nextDirection = Vector3.zero;
        rigidbody.isKinematic = false;
        enabled = true;
    }

    private void Update() {
        if (nextDirection != Vector3.zero) {
            SetDirection(nextDirection);
        }

        if (isBlocked(direction) && gameObject.CompareTag("Pacman")) {
            if (animator != null) animator.enabled = false;
        }
        else if (gameObject.CompareTag("Pacman")) {
            if (animator != null) animator.enabled = true;
        }
    }
    private void FixedUpdate() {
        Vector3 position = rigidbody.position;
        Vector3 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        // vibrate position.y linearly slowly irregularly
        if (!isBlocked(direction) && gameObject.CompareTag("Pacman")) translation.y += Mathf.Sin(Time.time * 4) * 0.005f;
        //Vector3 translation = transform.forward.normalized * (speed * speedMultiplier * Time.deltaTime);
        rigidbody.MovePosition(position + translation);
        // rigidbody.MoveRotation(Quaternion.Euler(rotation));
        
        // Turn the player to face the direction he is moving slowly
        /*
        if ((transform.eulerAngles - rotation).magnitude > 0.05f) {
            // Debug.Log("euler angle " + transform.eulerAngles);
            // Debug.Log("rotation " + rotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.75f);
        }*/
        
        // Turn instantly
        rigidbody.MoveRotation(Quaternion.Euler(rotation));
    }
    
    public void SetDirection(Vector3 targetDir, bool forced = false) {
        Debug.Log("SetDirection() called with target direction: " + targetDir.ToString() + " and forced: " + forced.ToString());

        if (isBlocked(targetDir) && !forced) {
            nextDirection = targetDir;
            Debug.Log("Path is blocked. Setting nextDirection to: " + nextDirection.ToString());
        }
        else {
            nextDirection = targetDir;
            direction = targetDir;
            nextDirection = targetDir;
            Debug.Log("Path is not blocked. Setting direction and nextDirection to: " + direction.ToString());

            rotation = new Vector3(0, Mathf.Atan2(direction.z, -1 * direction.x) * Mathf.Rad2Deg - 90, 0);
            if (rotation.y < 0) rotation += new Vector3(0, 360, 0);
            if (rotation.y >= 360) rotation -= new Vector3(0, 360, 0);
            // Force rotation.y to become 0 or 90 or 180 or 270
            rotation = new Vector3(0, Mathf.Round(rotation.y / 90) * 90, 0);
            Debug.Log("Calculated rotation: " + rotation.ToString());
        }

        // Add this debug line to log the current direction, nextDirection, and rotation
        Debug.Log("Current direction: " + direction.ToString() + ", nextDirection: " + nextDirection.ToString() + ", rotation: " + rotation.ToString());
    }


    private float rayOffset = 0.45f;
    public bool isBlocked(Vector3 testDirection) {
        Ray fowardRay = new Ray(transform.position + new Vector3(rayOffset, 0, -1*rayOffset), testDirection);
        Ray leftRay = new Ray(transform.position + new Vector3(rayOffset, 0, rayOffset), testDirection);
        Ray rightRay = new Ray(transform.position + new Vector3(-1*rayOffset, 0, -1*rayOffset), testDirection);
        Ray backRay = new Ray(transform.position + new Vector3(-1*rayOffset, 0, rayOffset), testDirection);
        
        Debug.DrawRay(fowardRay.origin, fowardRay.direction, Color.red);
        Debug.DrawRay(leftRay.origin, leftRay.direction, Color.red);
        Debug.DrawRay(rightRay.origin, rightRay.direction, Color.red);
        Debug.DrawRay(backRay.origin, backRay.direction, Color.red);
        
        RaycastHit forwardHit;
        RaycastHit leftHit;
        RaycastHit rightHit;
        RaycastHit backHit;
        
        Physics.Raycast(fowardRay, out forwardHit, 0.5f);
        Physics.Raycast(leftRay, out leftHit, 0.5f);
        Physics.Raycast(rightRay, out rightHit, 0.5f);
        Physics.Raycast(backRay, out backHit, 0.5f);
        
        bool result0 = forwardHit.collider != null && forwardHit.collider.gameObject.tag == "Wall";
        bool result1 = leftHit.collider != null && leftHit.collider.gameObject.tag == "Wall";
        bool result2 = rightHit.collider != null && rightHit.collider.gameObject.tag == "Wall";
        bool result3 = backHit.collider != null && backHit.collider.gameObject.tag == "Wall";

        bool result = result0 || result1 || result2 || result3;
        return result;
    }

}
