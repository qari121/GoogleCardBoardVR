using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject playUIPrefab; // Assign the play UI prefab in the Inspector
    private GameObject playUIInstance;
    public bool shouldSpawnPlayUI = false; // Flag to determine if play UI should be spawned
    public Button backbutton;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we should spawn the play UI after transitioning back to Scene 0
        if (shouldSpawnPlayUI && SceneManager.GetActiveScene().buildIndex == 0)
        {
            SpawnPlayUI();
            shouldSpawnPlayUI = false; // Reset the flag after spawning the UI
        }
    }

    public void CreateNewPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    // Method to spawn the play UI on the canvas in Scene 0
    public void SpawnPlayUI()
    {
        if (playUIPrefab != null && playUIInstance == null)
        {
            // Find the canvas in scene 0
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                // Instantiate the play UI as a child of the canvas
                playUIInstance = Instantiate(playUIPrefab, canvas.transform);
                TextMeshProUGUI statusText = playUIInstance.transform.Find("StatusText").GetComponent<TextMeshProUGUI>();
                statusText.text = "Done";
                StartCoroutine(WaitAMin());
            }
            else
            {
                Debug.LogError("Canvas not found in Scene 0!");
            }
        }
    }

    IEnumerator WaitAMin()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);

    }

    public void LoadEnvironmentScene()
    {
        SceneManager.LoadScene(2);

    }
}