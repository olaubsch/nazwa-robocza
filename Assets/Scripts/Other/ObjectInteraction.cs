using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ObjectInteraction : MonoBehaviour
{
    public string menuSceneName = "MenuScene";  // Nazwa sceny menu
    private bool isPlayerInRange = false;
    public TMP_Text interactText;
    public string interactMessage = "PRESS E";


    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenMenu();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            DisplayMessage(interactMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            ClearMessage();
        }
    }


    private void OpenMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

     void DisplayMessage(string message)
    {
        // Wyświetl komunikat na ekranie
        interactText.text = message;
    }

    void ClearMessage()
    {
        // Wyczyść komunikat z ekranu
        interactText.text = "";
    }

}
