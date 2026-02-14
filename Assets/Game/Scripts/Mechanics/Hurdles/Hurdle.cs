using UnityEngine;

public class Hurdle : MonoBehaviour, IInteractable
{
    [SerializeField] private int value = 10; // Positive amount to subtract

    [Tooltip("Test Purpose Only"), SerializeField] private bool randomizeNo = false;
    private int rndVal = 50; // min val=5, max 50 for hurdles

    private void Start()
    {
        if (randomizeNo)
        {
            value = UnityEngine.Random.Range(5, rndVal + 1);
        }
        
        var numberView = GetComponent<NumberViewBase>();
        if (numberView != null)
        {
            numberView.SetValue(-value); // Show negative value
        }
    }

    public void Interact(SnakeManager snake)
    {
        snake.SubtractValue(value); // Subtract from head and adjust followers
        gameObject.SetActive(false); // Deactivate / return to pool
    }

    public int GetValue() => value; // Positive subtract amount
}