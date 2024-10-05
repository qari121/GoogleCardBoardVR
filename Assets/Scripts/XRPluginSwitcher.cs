using UnityEngine;
using UnityEngine.XR.Management;
using System.Collections;

public class XRPluginSwitcher : MonoBehaviour
{
    public XRLoader cardboardXRLoader; // Assign the Cardboard XR loader in the Inspector
    public static XRPluginSwitcher instance;

    private void Awake()
    {
        Log("Awake: Initializing XR Plugin Switcher...");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            LogError("Awake: Instance already exists!");
        }

        if (cardboardXRLoader == null)
        {
            LogError("Awake: cardboardXRLoader is not assigned! Please assign it in the inspector.");
        }
    }

    // Call this function to initialize XR at runtime
    public void InitializeXR()
    {
        Log("Initializing XR at runtime...");
        StartCoroutine(InitializeXRCoroutine());
    }

    private IEnumerator InitializeXRCoroutine()
    {
        if (XRGeneralSettings.Instance == null)
        {
            LogError("InitializeXRCoroutine: XRGeneralSettings.Instance is null!");
            yield break;
        }

        if (XRGeneralSettings.Instance.Manager == null)
        {
            LogError("InitializeXRCoroutine: XRGeneralSettings.Instance.Manager is null!");
            yield break;
        }

        if (cardboardXRLoader == null)
        {
            LogError("InitializeXRCoroutine: cardboardXRLoader is null! Make sure to assign it.");
            yield break;
        }

        bool isLoaderActive = false;
        foreach (var loader in XRGeneralSettings.Instance.Manager.activeLoaders)
        {
            if (loader == cardboardXRLoader)
            {
                isLoaderActive = true;
                break;
            }
        }

        if (!isLoaderActive)
        {
            Log("InitializeXRCoroutine: Adding Cardboard XR Loader...");
            bool result = XRGeneralSettings.Instance.Manager.TryAddLoader(cardboardXRLoader, 0);
            if (!result)
            {
                LogError("InitializeXRCoroutine: Failed to add Cardboard XR Loader.");
                yield break;
            }
        }

        Log("InitializeXRCoroutine: Reinitializing XR Manager with Cardboard XR Loader...");
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();

        XRGeneralSettings.Instance.Manager.StartSubsystems();
        Log("InitializeXRCoroutine: XR subsystems started for Cardboard XR Loader.");

        if (XRGeneralSettings.Instance.Manager.activeLoader == cardboardXRLoader)
        {
            Log("InitializeXRCoroutine: Successfully switched to Cardboard XR Plugin.");
        }
        else
        {
            LogError("InitializeXRCoroutine: Failed to switch to Cardboard XR Plugin.");
        }
    }

    // Method to stop and deinitialize XR when the scene changes
    public void DeinitializeXR()
    {
        Log("Deinitializing XR...");

        if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();  // Stop all active XR subsystems
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();  // Deinitialize the XR loader
            Log("XR Deinitialized successfully.");
        }
        else
        {
            LogError("DeinitializeXR: XRGeneralSettings or XRManager is null!");
        }
    }

    // Helper method to log messages
    private void Log(string message)
    {
        Debug.Log(message);
    }

    // Helper method to log errors
    private void LogError(string message)
    {
        Debug.LogError(message);
    }
}