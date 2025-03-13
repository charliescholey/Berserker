using UnityEngine;

public class RotateEnemy : MonoBehaviour {
    GameObject visionCone;
    GameObject enemySprite;
    
    [SerializeField] float rotationSpeed = 10f;
    // Define the rotation limits for each side.
    [SerializeField] float clockwiseLimit = 50f;
    [SerializeField] float counterClockwiseLimit = 50f;
    
    // Set this field to a positive value for initial clockwise rotation (i.e., decreasing angle)
    // or a negative value for initial counterclockwise rotation (i.e., increasing angle).
    [SerializeField] int rotationSign = 1; 

    GameObject pivot;
    private float currentAngle = 0f;
    // rotationDirection: -1 means rotating toward negative angles (clockwise), +1 means toward positive angles (counterclockwise)
    private int rotationDirection;

    void Start() {
        // Find VisionCone and Enemy child objects.
        visionCone = transform.Find("VisionCone")?.gameObject;
        if (visionCone == null) {
            Debug.LogError("VisionCone GameObject not found as a child of " + gameObject.name);
            return;
        }

        enemySprite = transform.Find("Enemy")?.gameObject;
        if (enemySprite == null) {
            Debug.LogError("Enemy GameObject not found as a child of " + gameObject.name);
            return;
        }
        
        // Create a pivot at the enemy sprite's current position (the intended rotation centre)
        pivot = new GameObject("CharacterPivot");
        pivot.transform.position = enemySprite.transform.position;
        
        // Re-parent the enemy sprite and the vision cone to the pivot, preserving world positions.
        enemySprite.transform.SetParent(pivot.transform, true);
        visionCone.transform.SetParent(pivot.transform, true);
        
        // Reset the enemy sprite's local position so that the pivot is exactly at its centre.
        enemySprite.transform.localPosition = Vector3.zero;
        
        // Re-parent the pivot back to this object so that the overall structure is maintained.
        pivot.transform.SetParent(transform, true);
        
        // Attach the EnemyDetection script to the Triangle collider within VisionCone.
        Transform triangle = visionCone.transform.Find("Triangle");
        if (triangle != null) {
            if (triangle.GetComponent<PlayerDetection>() == null)
                triangle.gameObject.AddComponent<PlayerDetection>();
        }
        else {
            Debug.LogError("Triangle GameObject not found as a child of VisionCone.");
        }
        
        // Set the initial rotation direction based on the rotationSign:
        // If rotationSign is positive, start rotating clockwise (i.e., decreasing angle: -1).
        // If negative, start rotating counterclockwise (i.e., increasing angle: +1).
        rotationDirection = (rotationSign > 0) ? -1 : 1;
    }

    void Update() {
        // Calculate the incremental rotation for this frame.
        float deltaAngle = rotationSpeed * Time.deltaTime * rotationDirection;
        currentAngle += deltaAngle;

        // Check if we've reached the rotation limits and reverse if necessary.
        // Clockwise rotations are negative angles.
        if (currentAngle <= -clockwiseLimit) {
            currentAngle = -clockwiseLimit;
            rotationDirection = 1;  // Reverse to counterclockwise.
        }
        else if (currentAngle >= counterClockwiseLimit) {
            currentAngle = counterClockwiseLimit;
            rotationDirection = -1; // Reverse to clockwise.
        }

        // Apply the updated rotation to the pivot so that the enemy and vision cone rotate around the enemy's centre.
        pivot.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
