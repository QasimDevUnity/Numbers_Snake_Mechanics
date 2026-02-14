using System;
using UnityEngine;

public class PickableNumber : MonoBehaviour, IInteractable
{
    [SerializeField] private int value = 1;

    [Tooltip("Test Perpose Only"),SerializeField] private bool randomizeNo = false;
   private int rndVal = 50; // min val=5, max 100

    private void Start()
    {
        if (randomizeNo)
        {
            value = UnityEngine.Random.Range(5, 100);
        }
        var numberView = GetComponent<NumberViewBase>();
        numberView.SetValue(value);
    }


    public void Interact(SnakeManager snake)
    {
        snake.AddFollower(value); // Trigger follower addition
        gameObject.SetActive(false); // Return to pool later if needed
    }

    public int GetValue() => value;
}