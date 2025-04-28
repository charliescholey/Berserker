using System.Linq;
using UnityEngine;

public class WalkingEnemyHorizontal : MonoBehaviour {
    // will be found at runtime, no need to assign in Inspector
    GameObject visionCone;
    
    // Movement parameters.
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float moveDistance = 10f;
    
    // Rotation (turning) parameters.
    [SerializeField] float turnSpeed = 90f;
    
    GameObject pivot;
    GameObject enemySprite;
    Vector3 phaseStart;
    bool movingForward = true;
    
    private enum State { Moving, Rotating }
    private State currentState = State.Moving;
    
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private Quaternion flippedRotation;
    
    void Start() {
        // --- auto-find VisionCone child by name ---
        var allTransforms = GetComponentsInChildren<Transform>();
        var vcT = allTransforms.FirstOrDefault(t => t.name == "VisionCone");
        if (vcT == null) {
            Debug.LogError($"[{name}] could not find a child named 'VisionCone'");
            return;
        }
        visionCone = vcT.gameObject;
        
        // find the Enemy sprite child
        enemySprite = transform.Find("Enemy")?.gameObject;
        if (enemySprite == null) {
            Debug.LogError($"[{name}] could not find a child named 'Enemy'");
            return;
        }
        
        // pivot offset logic as before:
        Vector3 pivotOffset = new Vector3(0.5f, -0.6f, 0f);
        Vector3 origPos = enemySprite.transform.position;
        
        pivot = new GameObject("CharacterPivot");
        pivot.transform.position = origPos + pivotOffset;
        
        enemySprite.transform.SetParent(pivot.transform, true);
        visionCone.transform.SetParent(pivot.transform, true);
        
        // shift the sprite back so its world‐pos is unchanged
        enemySprite.transform.localPosition = -pivotOffset;
        
        pivot.transform.SetParent(transform, true);
        phaseStart = pivot.transform.position;
        
        // attach detection to VisionCone/Triangle
        var tri = visionCone.transform.Find("Triangle");
        if (tri != null && tri.GetComponent<PlayerDetection>() == null) {
            tri.gameObject.AddComponent<PlayerDetection>();
        }
        else if (tri == null) {
            Debug.LogError($"[{name}] VisionCone has no 'Triangle' child");
        }
        
        // rotations
        initialRotation = pivot.transform.rotation;
        flippedRotation = initialRotation * Quaternion.Euler(0,0,180f);
        targetRotation = flippedRotation;
    }
    
    void Update() {
        switch (currentState) {
            case State.Moving:
                pivot.transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
                if (Vector3.Distance(pivot.transform.position, phaseStart) >= moveDistance) {
                    targetRotation = movingForward ? flippedRotation : initialRotation;
                    currentState = State.Rotating;
                }
                break;
            
            case State.Rotating:
                pivot.transform.rotation = Quaternion.RotateTowards(
                    pivot.transform.rotation, targetRotation, turnSpeed * Time.deltaTime
                );
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
