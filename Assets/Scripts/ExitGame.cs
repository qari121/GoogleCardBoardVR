using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{

    // When the pointer enters (triggering the scene switch), stop XR and switch to Scene 2
    public void OnPointerEnter()
    {
        // Before switching the scene, deinitialize XR
        if (XRPluginSwitcher.instance != null)
        {
            XRPluginSwitcher.instance.DeinitializeXR();
        }

        GameManager.instance.LoadEnvironmentScene();

        // Load Environment Scene
        Debug.Log("Changing Scene and deinitializing XR");
    }

}