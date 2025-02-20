using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManager namespace

public class ReloadScene : MonoBehaviour
{
    private void Update()
    {
        // Check if the player presses the R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}