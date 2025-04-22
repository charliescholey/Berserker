using UnityEngine;

public class WalkingEnemyController : MonoBehaviour {

    
    // Child objects will be found automatically.
    [SerializeField] GameObject visionCone;
    
    // Movement parameters.
    [SerializeField] float moveSpeed = 2f;
    // Distance the enemy moves in each phase before turning.
    [SerializeField] float moveDistance = 10f;
    
    // Rotation (turning) parameters.
    // The speed (in degrees per second) for the 180° turn.
    [SerializeField] float turnSpeed = 90f;
    
    GameObject pivot;
    GameObject enemySprite;
    
    // Records the starting position for each movement phase.
    Vector3 phaseStart;
    
    // When true, enemy moves along the initial direction; when false, it moves in the opposite direction.
    bool movingForward = true;
    
    // Two states: moving (translating) or rotating.
    private enum State { Moving, Rotating }
    private State currentState = State.Moving;
    
    // The target rotation when a turn is initiated.
    private Quaternion targetRotation;
    
    // We store the pivot's initial rotation and its 180°-flipped version.
    private Quaternion initialRotation;
    private Quaternion flippedRotation;
    
    void Start() {
        // Find the VisionCone and Enemy child objects.
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
        
        // Create a pivot at the enemy sprite's position.
        pivot = new GameObject("CharacterPivot");
        pivot.transform.position = enemySprite.transform.position;
        // Record the starting position for the first movement phase.
        phaseStart = pivot.transform.position;
        
        // Re-parent the enemy sprite and vision cone to the pivot so that both move/rotate together.
        enemySprite.transform.SetParent(pivot.transform, true);
        visionCone.transform.SetParent(pivot.transform, true);
        // Reset the enemy sprite’s local position so that the pivot aligns with its centre.
        enemySprite.transform.localPosition = Vector3.zero;
        
        // Re-parent the pivot back under this GameObject.
        pivot.transform.SetParent(transform, true);
        
        // Automatically attach the detection script to the "Triangle" child within VisionCone.
        Transform triangle = visionCone.transform.Find("Triangle");
        if (triangle != null) {
            if (triangle.GetComponent<PlayerDetection>() == null)
                triangle.gameObject.AddComponent<PlayerDetection>();
        }
        else {
            Debug.LogError("Triangle GameObject not found as a child of VisionCone.");
        }
        
        // Store the pivot's initial rotation.
        initialRotation = pivot.transform.rotation;
        // Compute the flipped rotation (180° turn from initial).
        flippedRotation = initialRotation * Quaternion.Euler(0, 0, 180f);
        
        // Set the targetRotation based on the initial movement direction.
        // When moving forward the enemy should eventually flip to the flippedRotation.
        targetRotation = flippedRotation;
    }
    
    void Update() {
        switch (currentState) {
            case State.Moving:
                // Translate the pivot along its local up direction.
                pivot.transform.Translate(0, moveSpeed * Time.deltaTime, 0);
                
                // Determine how far we've moved in this phase.
                float distanceTravelled = Vector3.Distance(pivot.transform.position, phaseStart);
                
                if (distanceTravelled >= moveDistance) {
                    // When the enemy has moved the designated distance, begin the rotation phase.
                    // Use a fixed target rotation: if moving forward, turn to flippedRotation;
                    // if moving backward, turn back to the initialRotation.
                    targetRotation = movingForward ? flippedRotation : initialRotation;
                    currentState = State.Rotating;
                    enemySprite.transform.SetParent(null);
                }
                break;
                
            case State.Rotating:
                // Rotate smoothly toward the target rotation.
                pivot.transform.rotation = Quaternion.RotateTowards(pivot.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                
                // When the rotation is nearly complete...
                if (Quaternion.Angle(pivot.transform.rotation, targetRotation) < 0.1f) {
                    // Snap to the target rotation.
                    pivot.transform.rotation = targetRotation;
                    // Toggle the movement direction.
                    movingForward = !movingForward;
                    // Set up a new movement phase: record the current position as the phase start.
                    phaseStart = pivot.transform.position;
                    // Resume moving.
                    currentState = State.Moving;
                    enemySprite.transform.SetParent(pivot.transform, true);
                }
                break;
        }
    }
}
