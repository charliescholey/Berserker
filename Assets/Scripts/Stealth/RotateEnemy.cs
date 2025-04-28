using UnityEngine;

public class RotateEnemy : MonoBehaviour {
    GameObject visionCone;
    GameObject enemySprite;
    
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float clockwiseLimit = 50f;
    [SerializeField] float counterClockwiseLimit = 50f;
    [SerializeField] int rotationSign = 1; 

    GameObject pivot;
    private float currentAngle = 0f;
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
        
        // Create a pivot at the enemy sprite's centre offset by +0.5 on X and -0.6 on Y
        pivot = new GameObject("CharacterPivot");
<<<<<<< Updated upstream
        pivot.transform.position = enemySprite.transform.position;
=======
        Vector3 enemyPos = enemySprite.transform.position;
        pivot.transform.position = new Vector3(
            enemyPos.x + 0.5f,
            enemyPos.y - 0.6f,
            enemyPos.z
        );
>>>>>>> Stashed changes
        
        // Re-parent the enemy sprite and vision cone to the pivot, preserving world positions.
        enemySprite.transform.SetParent(pivot.transform, true);
        visionCone.transform.SetParent(pivot.transform, true);
        
<<<<<<< Updated upstream
        // Reset the enemy sprite's local position so that the pivot is exactly at its centre.
        enemySprite.transform.localPosition = Vector3.zero;
        
        // Re-parent the pivot back to this object so that the overall structure is maintained.
=======
        // Now parent the pivot under this object so hierarchy stays neat.
>>>>>>> Stashed changes
        pivot.transform.SetParent(transform, true);

        // Attach PlayerDetection to the "Triangle" inside VisionCone, if not already present.
        Transform triangle = visionCone.transform.Find("Triangle");
        if (triangle != null) {
            if (triangle.GetComponent<PlayerDetection>() == null)
                triangle.gameObject.AddComponent<PlayerDetection>();
        }
        else {
            Debug.LogError("Triangle GameObject not found as a child of VisionCone.");
        }
        
        // Determine initial rotation direction.
        rotationDirection = (rotationSign > 0) ? -1 : 1;
    }

    void Update() {
        // Compute this frame's rotation increment.
        float deltaAngle = rotationSpeed * Time.deltaTime * rotationDirection;
        currentAngle += deltaAngle;

        // Reverse direction at limits.
        if (currentAngle <= -clockwiseLimit) {
            currentAngle = -clockwiseLimit;
            rotationDirection = 1;
        }
        else if (currentAngle >= counterClockwiseLimit) {
            currentAngle = counterClockwiseLimit;
            rotationDirection = -1;
        }

        // Apply rotation around the pivot.
        pivot.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
