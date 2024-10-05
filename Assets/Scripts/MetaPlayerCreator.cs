using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MetaPlayerCreator : MonoBehaviour
{
    public TextMeshProUGUI heightText;
    public Button increaseHeightButton;
    public Button decreaseHeightButton;
    public Button maleButton;
    public Button femaleButton;
    public Button thinButton;
    public Button smartButton;
    public Button healthyButton;
    public Button fatButton;
    public Button createButton;

    public GameObject processingPrefab; // Prefab containing the slider and status text
    public Transform canvasTransform;   // Reference to the Canvas or UI parent to instantiate the prefab under
    public Button backButton;
    private int height;
    private string gender;
    private string bodyType;
    GameObject processingUI;

    void Start()
    {
        GameManager.instance.backbutton = GameObject.Find("BackButton").GetComponent<Button>();
        GameManager.instance.backbutton.onClick.AddListener(GameManager.instance.OnBackButtonPressed);

        // Load saved preferences if available
        height = PlayerPrefs.GetInt("PlayerHeight", 12); // Default height is 12
        gender = PlayerPrefs.GetString("PlayerGender", "Male"); // Default gender is Male
        bodyType = PlayerPrefs.GetString("PlayerBodyType", "Smart"); // Default body type is Smart

        UpdateUI();

        // Setup button listeners
        increaseHeightButton.onClick.AddListener(IncreaseHeight);
        decreaseHeightButton.onClick.AddListener(DecreaseHeight);
        maleButton.onClick.AddListener(() => SetGender("Male"));
        femaleButton.onClick.AddListener(() => SetGender("Female"));
        thinButton.onClick.AddListener(() => SetBodyType("Thin"));
        smartButton.onClick.AddListener(() => SetBodyType("Smart"));
        healthyButton.onClick.AddListener(() => SetBodyType("Healthy"));
        fatButton.onClick.AddListener(() => SetBodyType("Fat"));
        createButton.onClick.AddListener(OnCreateButtonPressed);
    }

    void IncreaseHeight()
    {
        height++;
        UpdateHeightText();
    }

    void DecreaseHeight()
    {
        if (height > 0) // Ensure height doesn't go below 0
        {
            height--;
            UpdateHeightText();
        }
    }

    void SetGender(string selectedGender)
    {
        gender = selectedGender;
    }

    void SetBodyType(string selectedBodyType)
    {
        bodyType = selectedBodyType;
    }

    void UpdateHeightText()
    {
        heightText.text = height.ToString();
    }

    void UpdateUI()
    {
        UpdateHeightText();
    }

    void OnCreateButtonPressed()
    {
        SavePlayerPreferences();
        InstantiateProcessingUI();
    }

    void SavePlayerPreferences()
    {
        PlayerPrefs.SetInt("PlayerHeight", height);
        PlayerPrefs.SetString("PlayerGender", gender);
        PlayerPrefs.SetString("PlayerBodyType", bodyType);
        PlayerPrefs.Save();
        Debug.Log("Player Preferences Saved: Height - " + height + ", Gender - " + gender + ", Body Type - " + bodyType);
    }

    void InstantiateProcessingUI()
    {
        // Instantiate the prefab
        processingUI = Instantiate(processingPrefab, canvasTransform);

        // Find the slider, text, and cross button components in the instantiated prefab
        Slider slider = processingUI.GetComponentInChildren<Slider>();
        TextMeshProUGUI statusText = processingUI.GetComponentInChildren<TextMeshProUGUI>();
        Button crossButton = processingUI.transform.Find("CrossButton").GetComponent<Button>(); // Assumes button is named "CrossButton"

        // Initially hide the cross button
        crossButton.gameObject.SetActive(false);

        // Add listener to the cross button to delete player and UI
        crossButton.onClick.AddListener(() => OnCrossButtonPressed());

        // Start the processing coroutine to simulate progress
        StartCoroutine(ProcessPlayerCreation(slider, statusText, crossButton));
    }

    // Coroutine to simulate the player creation process
    System.Collections.IEnumerator ProcessPlayerCreation(Slider slider, TextMeshProUGUI statusText, Button crossButton)
    {
        // Simulate processing (for example, taking 3 seconds to complete)
        float processingTime = 3.0f;
        float elapsedTime = 0.0f;

        statusText.text = "Processing...";

        while (elapsedTime < processingTime)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Clamp01(elapsedTime / processingTime); // Update slider value based on elapsed time
            yield return null;
        }

        // Simulate a success or failure
        bool success = Random.Range(0, 2) == 0; // Randomly succeed or fail

        if (success)
        {
            statusText.text = "Completed!";
            statusText.color = Color.green;

            // Notify GameManager to spawn Play UI in Scene 0
            GameManager.instance.shouldSpawnPlayUI = true;

            // Load Scene 0
            SceneManager.LoadScene(0);
        }
        else
        {
            statusText.text = "Failed!";
            statusText.color = Color.red;

            // Show the cross button to allow retry
            crossButton.gameObject.SetActive(true);
        }

        slider.value = 1.0f; // Set slider to full (complete)
    }

    void OnCrossButtonPressed()
    {
        // Delete the player and UI
        Debug.Log("Deleting player and UI");

        // Optionally, clear PlayerPrefs or any other player-related data
        PlayerPrefs.DeleteKey("PlayerHeight");
        PlayerPrefs.DeleteKey("PlayerGender");
        PlayerPrefs.DeleteKey("PlayerBodyType");

        // Destroy the UI
        if (processingUI != null)
        {
            Destroy(processingUI);
        }
    }
}