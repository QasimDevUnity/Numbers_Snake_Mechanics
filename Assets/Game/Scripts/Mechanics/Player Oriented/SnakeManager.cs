using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    #region Variables And Declarations

    private GameConfig config;
    private PlayerController playerController;
    public int totalCounting = 0; // for tracking..

    private Transform head;

    private List<Transform> followerTransforms = new(); // Store transforms only
    private List<int> followerValues = new();
    private List<NumberViewBase> followerNumberViews = new(); // Cache for updates
    
    #endregion

    #region Executions

    private void Start()
    {
        config = GameManager.Instance.gameConfig;
        playerController = GameManager.Instance.playerController;
        head = this.transform;
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.canGameRun)
            UpdateFollowerPositions();
    }

    private void UpdateFollowerPositions()
    {
        Transform currentTarget = head;
        for (int i = 0; i < followerTransforms.Count; i++)
        {
            Transform follower = followerTransforms[i];
            Vector3 direction = currentTarget.position - follower.position;
            Vector3 desiredPosition = currentTarget.position - direction.normalized * config.segmentSpacing;
            desiredPosition.y = 0.067f;

            follower.position = Vector3.Lerp(
                follower.position,
                desiredPosition,
                config.followSpeed * Time.fixedDeltaTime
            );

            currentTarget = follower; // Chain to next
        }
    }

    public void AddFollower(int numberValue)
    {
        // ALWAYS update head number first
        playerController.SetNewNumber(numberValue);

        // If we reached visual limit → just update numbers and return
        if (followerTransforms.Count >= config.maxFollowers)
        {
            UpdateFollowerNumbers();
            return;
        }

        // ---- Spawn follower (only if below limit) ----
        PooledObject followerObj = GameManager.Instance.poolManager.GetFromPool("Follower");
        followerObj.transform.localScale = Vector3.one;
        followerObj.gameObject.SetActive(true);

        // No SnakeFollower script needed; we handle movement here
        // Remove SnakeFollower component if pooled object has it
        SnakeFollower oldScript = followerObj.GetComponent<SnakeFollower>();
        if (oldScript != null) Destroy(oldScript);

        Transform target = followerTransforms.Count == 0 ? head : followerTransforms[followerTransforms.Count - 1];

        Vector3 spawnPos = target.position - target.forward * config.segmentSpacing;
        spawnPos.y = 0.067f;
        followerObj.transform.position = spawnPos;

        // Assign follower number
        int followerNumber = playerController.currentNumber - (followerTransforms.Count + 1);
        if (followerNumber < 0) followerNumber = 0;

        NumberViewBase numberViewBase = followerObj.GetComponent<NumberViewBase>();
        if (numberViewBase != null)
            numberViewBase.SetValue(followerNumber);

        followerTransforms.Add(followerObj.transform);
        followerNumberViews.Add(numberViewBase);
        followerValues.Add(followerNumber);

        // Update existing follower numbers too
        UpdateFollowerNumbers();
    }

    // Update all followers numbers dynamically after head number changes
    public void UpdateFollowerNumbers()
    {
        for (int i = 0; i < followerTransforms.Count; i++)
        {
            int newValue = playerController.currentNumber - (i + 1);
            if (newValue < 0) newValue = 0;
            UpdateFollowerValue(i, newValue);
        }
    }

    public void UpdateFollowerValue(int index, int newValue)
    {
        if (index < 0 || index >= followerTransforms.Count) return;

        if (followerNumberViews[index] != null)
            followerNumberViews[index].SetValue(newValue);

        followerValues[index] = newValue;
    }

    public List<int> GetFollowerValues() => new List<int>(followerValues);
    
    public void SubtractValue(int amount)
    {
        if (amount <= 0) return;

        playerController.SetNewNumber(-amount);
        
        int spawnCount = 5;
        float baseOffset = 0.035f;
        for (int i = 0; i < spawnCount; i++)
        {
            PooledObject deadNumber = GameManager.Instance.poolManager.GetFromPool("dead_number");

            // Alternate left and right: even indices go right, odd go left
            float xOffset = (i % 2 == 0 ? 1 : -1) * baseOffset * ((i / 2) + 1);
            float zOffset = Random.Range(-0.3f, 0.3f);

            Vector3 spawnPos = transform.position + new Vector3(xOffset, 0.067f, zOffset);
            deadNumber.transform.position = spawnPos;
            deadNumber.transform.localScale = Vector3.one;
            deadNumber.gameObject.SetActive(true);
            
        }

        UpdateFollowerNumbers();
    }
    
    
    #endregion
}