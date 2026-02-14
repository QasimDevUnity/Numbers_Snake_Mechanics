using System;
using UnityEngine;

public class PickableNumber : MonoBehaviour, IInteractable
{
    [SerializeField] private int value = 1;

    private void Start()
    {
        var numberView = GetComponent<NumberView>();
        numberView.SetValue(value);
    }


    public void Interact(SnakeManager snake)
    {
        snake.AddFollower(value); // Trigger follower addition
        gameObject.SetActive(false); // Return to pool later if needed
    }

    public int GetValue() => value;
}