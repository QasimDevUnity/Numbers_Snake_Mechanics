using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables Declarations

    private GameConfig config;
    private InputManager inputManager;
    private NumberViewBase numberViewBase;

    public int currentNumber = 1; // head number

    private float currentX;
    private float targetX;
    #endregion
    
    

    public void Initialize(GameConfig config, InputManager inputManager)
    {
        this.config = config;
        this.inputManager = inputManager;
    }


    #region Execution

    #region Monobehaviour Events

     private void Start()
     {
         numberViewBase = GetComponent<NumberViewBase>(); numberViewBase.SetValue(currentNumber);
     }

     private void Update()
     {
        HandleMovement();
     }

    #endregion



    #region Contol Executions

    

    

    public void SetNewNumber(int number)
    {
        
        GameManager.Instance.soundManager.PlaySFX(ReturnSfxVal(number));
        currentNumber += number;
        numberViewBase.SetValue(currentNumber);

        // Update all follower numbers
        GameManager.Instance.snakeManager.UpdateFollowerNumbers(); 
    }

    string ReturnSfxVal(int newVal)
    {
        int gap = Mathf.Abs(newVal - currentNumber);

        string sfxType = gap < 20 
            ? "soft_impact" 
            : (gap < 50 ? "medium_impact" : "high_impact");

        return sfxType;
    }

    
   
    private void HandleMovement()  //..........Movement Handling.......//
    {
        // ---- Forward Auto Move ----
        transform.Translate(Vector3.forward * config.forwardSpeed * Time.deltaTime);

        // ---- Horizontal Input ----
        float input = inputManager.GetHorizontal();

        targetX += input * config.horizontalSpeed * Time.deltaTime;

        targetX = Mathf.Clamp(targetX, -config.horizontalLimit, config.horizontalLimit);

        currentX = Mathf.Lerp(currentX, targetX, config.horizontalSmoothness * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = currentX;
        transform.position = pos;
    }

    // ---- Trigger Detection ----
    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact(GameManager.Instance.snakeManager);
        }
    }
    #endregion
    
    #endregion
    
}