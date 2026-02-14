using UnityEngine;

[CreateAssetMenu(menuName = "Game/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Movement")]
    public float forwardSpeed = 6f;
    public float horizontalSpeed = 10f;
    public float horizontalLimit = 3f;
    public float horizontalSmoothness = 15f;


    [Header("Snake")]
    public float followSpeed = 15f;
    public float segmentSpacing = 0.6f;
    public int maxFollowers = 15;
    public float digitSpacing=0.1f;
}