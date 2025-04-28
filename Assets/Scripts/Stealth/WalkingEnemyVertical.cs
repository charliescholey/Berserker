using System.Linq;
using UnityEngine;

public class WalkingEnemyController : MonoBehaviour {

    // Movement parameters.
    [SerializeField] float moveSpeed = 2f;
    // Distance the enemy moves in each phase before turning.
    [SerializeField] float moveDistance = 10f;
    // Rotation (turning) parameters: speed for the 180° turn.
    [SerializeField] float turnSpeed = 90f;

    GameObject visionCone;
    GameObject pivot;
    GameObject enemySprite;

    // Records the starting position for each movement phase.
    Vector3 phaseStart;
    bool movingForward = true;
    private enum State { Moving, Rotating }
    private State currentState = State.Moving;
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private Quaternion flippedRotation;

    void Start() {
        // Automatically find the VisionCone among all child transforms.
        visionCone = GetComponentsInChildren<Transform>()
            .FirstOrDefault(t => t.name == "VisionCone")?.gameObject;
        if (visionCone == null) {
            Debug.LogError($"VisionCone GameObject not found under {gameObject.name}");
            return;
        }

        // Find the Enemy sprite child.
        enemySprite = transform.Find("Enemy")?.gameObject;
        if (enemySprite == null) {
            Debug.LogError($"Enemy GameObject not found under {gameObject.name}");
            return;
        }

        // Create a pivot at the desired rotation center: enemy.x + 0.5, enemy.y - 0.6
        Vector3 pivotPos = enemySprite.transform.position + new Vector3(0.5f, -0.6f, 0f);
        pivot = new GameObject("CharacterPivot");
        pivot.transform.position = pivotPos;

        // Re-parent sprite and vision cone to pivot, preserving world positions
        enemySprite.transform.SetParent(pivot.transform, true);
        visionCone.transform.SetParent(pivot.transform, true);

        // After parenting, compute local offset for the sprite so it stays at its original position
        Vector3 spriteLocal = enemySprite.transform.position - pivot.transform.position;
        enemySprite.transform.localPosition = spriteLocal;

        // Re-parent the pivot under this controller for organizational hierarchy
        pivot.transform.SetParent(transform, true);

        // Attach PlayerDetection to the Triangle child within VisionCone
        Transform triangle = visionCone.transform.Find("Triangle");
        if (triangle != null) {
            if (triangle.GetComponent<PlayerDetection>() == null)
                triangle.gameObject.AddComponent<PlayerDetection>();
        }
        else Debug.LogError("Triangle child not found under VisionCone");

        // Store rotations for movement flipping
        initialRotation = pivot.transform.rotation;
        flippedRotation = initialRotation * Quaternion.Euler(0, 0, 180f);
        targetRotation = flippedRotation;

        // Set the starting position for movement
        phaseStart = pivot.transform.position;
    }

    void Update() {
        switch (currentState) {
            case State.Moving:
                pivot.transform.Translate(0, moveSpeed * Time.deltaTime, 0);
                if (Vector3.Distance(pivot.transform.position, phaseStart) >= moveDistance) {
                    targetRotation = movingForward ? flippedRotation : initialRotation;
                    currentState = State.Rotating;
                }
                break;

            case State.Rotating:
                pivot.transform.rotation = Quaternion.RotateTowards(
                    pivot.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                if (Quaternion.Angle(pivot.transform.rotation, targetRotation) < 0.1f) {
                    pivot.transform.rotation = targetRotation;
                    movingForward = !movingForward;
                    phaseStart = pivot.transform.position;
                    currentState = State.Moving;
                }
                break;
        }
    }
}