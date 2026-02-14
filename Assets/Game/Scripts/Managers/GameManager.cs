using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configs")]
     public GameConfig gameConfig;

    [Header("Core Systems")]
    public bool canGameRun=true;
    public InputManager inputManager;
     public PoolManager poolManager;
     public PlayerController playerController;
     public SnakeManager snakeManager;
     public SoundManager soundManager;




     [SerializeField] private GameObject winPanel;
     

   

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
       // HapticManager.Initialize();
    }

    private void InitializeSystems()
    {
        playerController.Initialize(gameConfig, inputManager);
    }

    public void WinLevel()
    {
        winPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}