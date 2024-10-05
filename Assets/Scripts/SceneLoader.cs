using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Button voidButton;
    public Button roomButton;
    public Button envButton;
    public Button cloudButton;

    void Start()
    {
        // Add listener for the Void button
        voidButton.onClick.AddListener(() => LoadScene("VOID"));

        // Add listener for the Room button
        roomButton.onClick.AddListener(() => LoadScene("ROOM"));

        // Add listener for the Env button
        envButton.onClick.AddListener(() => LoadScene("ENV"));

        // Add listener for the Cloud button
        cloudButton.onClick.AddListener(() => LoadScene("CLOUD"));
    }

    // Method to load a scene by name
    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        XRPluginSwitcher.instance.InitializeXR();
    }
}