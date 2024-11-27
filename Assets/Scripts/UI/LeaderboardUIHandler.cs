using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab; // Prefab for leaderboard item
    private List<SetLeaderboardItemInfo> leaderboardItems = new List<SetLeaderboardItemInfo>();
    private bool isInitialized = false;

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false; // Initially hide leaderboard

        // Hook up events safely
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameStateChanged += OnGameStateChanged;
        }
    }

    void Start()
    {
        // Initialize leaderboard UI
        InitializeLeaderboard();
    }

    void InitializeLeaderboard()
    {
        if (isInitialized) return;

        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
        if (leaderboardLayoutGroup == null)
        {
            Debug.LogError("VerticalLayoutGroup not found in the leaderboard UI.");
            return;
        }

        CarLapCounter[] carLapCounters = FindObjectsOfType<CarLapCounter>();
        leaderboardItems.Clear(); // Clear the list to avoid duplicates

        // Create leaderboard items dynamically
        foreach (CarLapCounter carLapCounter in carLapCounters)
        {
            GameObject leaderboardItem = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);
            SetLeaderboardItemInfo itemInfo = leaderboardItem.GetComponent<SetLeaderboardItemInfo>();
            if (itemInfo != null)
            {
                leaderboardItems.Add(itemInfo);
            }
        }

        Canvas.ForceUpdateCanvases(); // Ensure the layout updates correctly
        isInitialized = true;
    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        if (!isInitialized) return;

        for (int i = 0; i < leaderboardItems.Count; i++)
        {
            if (i < lapCounters.Count)
            {
                leaderboardItems[i].SetPositionText($"{i + 1}.");
                leaderboardItems[i].SetDriverNameText(lapCounters[i].gameObject.name);
            }
            else
            {
                // Hide unused leaderboard items if the list size changes
                leaderboardItems[i].gameObject.SetActive(false);
            }
        }
    }

    void OnGameStateChanged(GameManager gameManager)
    {
        Debug.Log("Game state changed to: " + gameManager.GetGameState());

        if (gameManager.GetGameState() == GameStates.countDown || gameManager.GetGameState() == GameStates.running)
        {
            canvas.enabled = true; // Show leaderboard during countdown and race
        }
        else if (gameManager.GetGameState() == GameStates.raceOver)
        {
            canvas.enabled = true; // Keep it visible at the end of the race
        }
        else
        {
            canvas.enabled = false; // Hide leaderboard in other states
        }
    }

    void OnDestroy()
    {
        // Unhook events safely
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}







