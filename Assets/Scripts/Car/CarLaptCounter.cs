using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;  // Added for TextMeshPro usage

public class CarLapCounter : MonoBehaviour
{
    public TMP_Text carPositionText;  // Use TMP_Text for car position display
    public TMP_Text lapText;  // Use TMP_Text for lap counter

    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint = 0;

    int numberOfPassedCheckpoints = 0;

    int lapsCompleted = 0;  // Starts at 0
    const int lapsToComplete = 4;  // Total laps for the race

    bool isRaceCompleted = false;

    int carPosition = 0;

    bool isHideRoutineRunning = false;
    float hideUIDelayTime;

    // Reference to LapCounterUIHandler
    LapCounterUIHandler lapCounterUIHandler;

    // Events
    public event Action<CarLapCounter> OnPassCheckpoint;

    void Start()
    {
        if (CompareTag("Player"))
        {
            lapCounterUIHandler = FindObjectOfType<LapCounterUIHandler>();

            if (lapCounterUIHandler != null)
            {
                lapCounterUIHandler.SetLapText($"LAP {lapsCompleted}/{lapsToComplete}");  // Initialize lap display as 0/4
            }
            else
            {
                Debug.LogError("LapCounterUIHandler not found in the scene. Please ensure it is added.");
            }
        }
    }

    public void SetCarPosition(int position)
    {
        carPosition = position;
    }

    public int GetNumberOfCheckpointsPassed()
    {
        return numberOfPassedCheckpoints;
    }

    public float GetTimeAtLastCheckPoint()
    {
        return timeAtLastPassedCheckPoint;
    }

    public bool IsRaceCompleted()
    {
        return isRaceCompleted;
    }

    IEnumerator ShowPositionCO(float delayUntilHidePosition)
    {
        hideUIDelayTime += delayUntilHidePosition;

        carPositionText.text = carPosition.ToString();  // Update car position

        carPositionText.gameObject.SetActive(true);

        if (!isHideRoutineRunning)
        {
            isHideRoutineRunning = true;

            yield return new WaitForSeconds(hideUIDelayTime);
            carPositionText.gameObject.SetActive(false);

            isHideRoutineRunning = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("CheckPoint"))
        {
            if (isRaceCompleted)
                return;

            CheckPoint checkPoint = collider2D.GetComponent<CheckPoint>();

            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;
                numberOfPassedCheckpoints++;
                timeAtLastPassedCheckPoint = Time.time;

                if (checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;

                    // Update lap counter UI
                    if (lapCounterUIHandler != null)
                        lapCounterUIHandler.SetLapText($"LAP {lapsCompleted}/{lapsToComplete}");

                    if (lapsCompleted >= lapsToComplete)
                    {
                        isRaceCompleted = true;
                    }
                }

                OnPassCheckpoint?.Invoke(this);

                if (isRaceCompleted)
                {
                    StartCoroutine(ShowPositionCO(100));

                    if (CompareTag("Player"))
                    {
                        GameManager.instance.OnRaceCompleted();
                        GetComponent<CarInputHandler>().enabled = false; // Disable player control
                        GetComponent<CarAIHandler>().enabled = true;    // Optional post-race AI
                    }
                }
                else if (checkPoint.isFinishLine)
                    StartCoroutine(ShowPositionCO(1.5f)); // Briefly show position
            }
        }
    }
}



