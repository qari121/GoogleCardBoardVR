using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class ScreenRotationManager : MonoBehaviour
{
    public TextMeshProUGUI noticeText; // UI Text element to display notice to the user
    public GameObject noticePanel; // UI Panel to show the notice

    private void Start()
    {
        // Show the notice initially and check for orientation change
        ShowNotice("Please rotate your device to Left");
        StartCoroutine(CheckOrientationChange());
    }

    // Coroutine to continuously check the screen orientation
    private IEnumerator CheckOrientationChange()
    {
        while (true)
        {
            // If the screen is either Landscape Left or Landscape Right
            if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                HideNotice(); // Hide the notice if the orientation is correct
                yield break;  // Exit the loop once the condition is met
            }

            // Keep checking once per second
            yield return new WaitForSeconds(1.0f);
        }
    }

    // Show the notice to the user
    private void ShowNotice(string message)
    {
        if (noticeText != null)
        {
            noticeText.text = message;
        }

        if (noticePanel != null)
        {
            noticePanel.SetActive(true); // Display the panel
        }
    }

    // Hide the notice
    private void HideNotice()
    {
        if (noticePanel != null)
        {
            noticePanel.SetActive(false); // Hide the panel
        }
    }
}