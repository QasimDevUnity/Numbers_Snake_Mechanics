using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    private GameConfig config;
    private PlayerController playerController;
    public int totalCounting = 0; // for tracking..

    private Transform head;

    private List<SnakeFollower> followers = new();
    private List<int> followerValues = new();

    private void Start()
    {
        config = GameManager.Instance.gameConfig;
        playerController = GameManager.Instance.playerController;
        head = this.transform;
    }

    public void AddFollower(int numberValue)
    {
        // ALWAYS update head number first
        playerController.SetNewNumber(numberValue);

        // If we reached visual limit → just update numbers and return
        if (followers.Count >= config.maxFollowers)
        {
            UpdateFollowerNumbers();
            return;
        }

        // ---- Spawn follower (only if below limit) ----
        PooledObject followerObj = GameManager.Instance.poolManager.GetFromPool("Follower");
        followerObj.transform.localScale = Vector3.one;
        followerObj.gameObject.SetActive(true);

        SnakeFollower follower = followerObj.GetComponent<SnakeFollower>();

        Transform target = followers.Count == 0 ? head : followers[followers.Count - 1].transform;

        Vector3 spawnPos = target.position - target.forward * config.segmentSpacing;
        spawnPos.y = 0.067f;
        followerObj.transform.position = spawnPos;

        follower.Initialize(target, config);

        // Assign follower number
        int followerNumber = playerController.currentNumber - (followers.Count + 1);
        if (followerNumber < 0) followerNumber = 0;

        NumberViewBase numberViewBase = follower.GetComponent<NumberViewBase>();
        if (numberViewBase != null)
            numberViewBase.SetValue(followerNumber);

        followers.Add(follower);
        followerValues.Add(followerNumber);

        // Update existing follower numbers too
        UpdateFollowerNumbers();
    }


    // Update all followers numbers dynamically after head number changes
    public void UpdateFollowerNumbers()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            int newValue = playerController.currentNumber - (i + 1);
            if (newValue < 0) newValue = 0;
            UpdateFollowerValue(i, newValue);
        }
    }

    public void UpdateFollowerValue(int index, int newValue)
    {
        if (index < 0 || index >= followers.Count) return;

        NumberViewBase numberViewBase = followers[index].GetComponent<NumberViewBase>();
        if (numberViewBase != null)
            numberViewBase.SetValue(newValue);

        followerValues[index] = newValue;
    }

    public List<int> GetFollowerValues() => new List<int>(followerValues);
}
