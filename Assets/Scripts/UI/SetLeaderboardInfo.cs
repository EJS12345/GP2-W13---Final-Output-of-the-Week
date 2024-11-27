using TMPro;
using UnityEngine;

public class SetLeaderboardItemInfo : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI driverNameText;

    public void SetPositionText(string newPosition)
    {
        if (positionText != null)
        {
            positionText.text = newPosition;
        }
    }

    public void SetDriverNameText(string newDriverName)
    {
        if (driverNameText != null)
        {
            driverNameText.text = newDriverName;
        }
    }
}

