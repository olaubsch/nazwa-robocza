using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace OLMJ{
public class EscapeMenuActivator : MonoBehaviour
{
    public GameObject inGameMenu;
    public Button escapeButton;
    public Button toMainMenuButton;
    public bool escapeMenuOpen;

    // Previous time scale before pausing
    private float previousTimeScale;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleEscapeMenu();
        }
    }

    public void ToggleEscapeMenu()
    {
        escapeMenuOpen = !escapeMenuOpen;
        inGameMenu.SetActive(escapeMenuOpen);

        // Pause or resume the game when the escape menu is toggled
        if (escapeMenuOpen)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Pauza ruchu kamery
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerManager playerController = player.GetComponent<PlayerManager>();
                if (playerController != null)
                {
                    playerController.SetPauseState(true);
                }
            }
        }
        else
        {
            Time.timeScale = previousTimeScale;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        
            // Wznowienie ruchu kamery
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerManager playerController = player.GetComponent<PlayerManager>();
                if (playerController != null)
                {
                    playerController.SetPauseState(false);
                }
            }
        }
    }

    public void Escape()
    {
        Application.Quit();
    }

    public void OnExitMenuButtonClick()
    {
        // Resume the game before loading the main menu
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Print a message to ensure the method is called
        Debug.Log("Exit to Main Menu Button Clicked");

        SceneManager.LoadScene("Main Menu");
    }
}
}