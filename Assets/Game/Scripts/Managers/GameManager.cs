using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configs")]
     public GameConfig gameConfig;

    [Header("Core Systems")]
     public InputManager inputManager;
     public PoolManager poolManager;
     public PlayerController playerController;
     public SnakeManager snakeManager;
     public SoundManager soundManager;

   

     private void Awake()
     {
         if (Instance == null)
         {
             Instance = this;
             DontDestroyOnLoad(gameObject); // optional if you want it persistent
         }
         else
         {
             Destroy(gameObject); // prevent duplicates
         }
     }


    private void Start()
    {
        InitializeSystems();
    }

    private void InitializeSystems()
    {
        playerController.Initialize(gameConfig, inputManager);
    }


}