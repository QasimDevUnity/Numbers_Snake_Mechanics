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
        if (followers.Count >= config.maxFollowers)
            return;

        // --- Spawn follower from pool ---
        PooledObject followerObj = GameManager.Instance.poolManager.GetFromPool("Follower");
        followerObj.transform.localScale = Vector3.one;
        followerObj.gameObject.SetActive(true);

        SnakeFollower follower = followerObj.GetComponent<SnakeFollower>();

        // --- Assign target (previous follower or head) ---
        Transform target = followers.Count == 0 ? head : followers[followers.Count - 1].transform;

        // --- Position follower behind target ---
        Vector3 spawnPos = target.position - target.forward * config.segmentSpacing;
        spawnPos.y = 0.067f; // fixed Y
        followerObj.transform.position = spawnPos;

        // --- Initialize follower ---
        follower.Initialize(target, config);

        // --- Update player number ---
        playerController.SetNewNumber(numberValue);

        // --- Assign follower number (head minus index) ---
        int followerNumber = playerController.currentNumber - (followers.Count + 1);
        if (followerNumber < 0) followerNumber = 0;

        NumberViewBase numberViewBase = follower.GetComponent<NumberViewBase>();
        if (numberViewBase != null)
            numberViewBase.SetValue(followerNumber);

        // --- Track follower ---
        followers.Add(follower);
        followerValues.Add(followerNumber);
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
