using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// This attribute allows the script to run in Edit Mode.
[ExecuteInEditMode]
public class DuplicateScript : MonoBehaviour
{
    [Tooltip("Set a positive value to duplicate to the right, negative to duplicate to the left.")]
    public int x;
    [Tooltip("Set a positive value to duplicate upward, negative to duplicate downward.")]
    public int y;

    // This context menu item lets you trigger duplication from the Inspector in the editor.
    [ContextMenu("Duplicate Object")]
    public void DuplicateObject()
    {
#if UNITY_EDITOR
        // Duplicate along the x-axis.
        int copiesX = Mathf.Abs(x);
        int signX = (x >= 0) ? 1 : -1;
        for (int i = 1; i <= copiesX; i++)
        {
            Vector3 newPos = transform.position + new Vector3(i * signX, 0, 0);
            // Instantiate a new copy using the prefab if available.
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(
                PrefabUtility.GetCorrespondingObjectFromSource(gameObject) ?? gameObject);
            newObj.transform.position = newPos;
            newObj.transform.rotation = transform.rotation;
            // Remove this script from the duplicate to avoid further duplication.
            DestroyImmediate(newObj.GetComponent<DuplicateScript>());
        }

        // Duplicate along the y-axis.
        int copiesY = Mathf.Abs(y);
        int signY = (y >= 0) ? 1 : -1;
        for (int j = 1; j <= copiesY; j++)
        {
            Vector3 newPos = transform.position + new Vector3(0, j * signY, 0);
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(
                PrefabUtility.GetCorrespondingObjectFromSource(gameObject) ?? gameObject);
            newObj.transform.position = newPos;
            newObj.transform.rotation = transform.rotation;
            DestroyImmediate(newObj.GetComponent<DuplicateScript>());
        }
#endif
    }
}
