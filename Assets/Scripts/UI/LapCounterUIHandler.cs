using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Use TextMeshPro

public class LapCounterUIHandler : MonoBehaviour
{
    TMP_Text lapText;  // Reference to TMP_Text component

    private void Awake()
    {
        lapText = GetComponent<TMP_Text>();  // Ensure it references a TMP_Text component
    }

    public void SetLapText(string text)
    {
        lapText.text = text;  // Update lap text
    }
}

