using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public string sceneName;
    public int difficultyStars;
    public Vector2 playerStartPosition;
    public GameObject[] enemyPrefabs;
    public Vector2[] enemyPositions;
    public GameObject[] interactiveObjects;
    public Vector2[] objectPositions;
}