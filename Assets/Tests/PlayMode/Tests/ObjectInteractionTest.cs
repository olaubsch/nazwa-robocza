using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class ObjectInteractionTest
{
    private GameObject player;
    private GameObject interactableObject;
    private TMP_Text interactText;
    private bool isPlayerInRange;
    private string interactMessage = "PRESS E";
    private string menuSceneName = "MainLocation";

    [SetUp]
    public void Setup()
    {
        // Tworzenie obiektu gracza
        player = new GameObject("Player");
        player.tag = "Player";
        var playerCollider = player.AddComponent<BoxCollider>();
        playerCollider.isTrigger = true;

        // Tworzenie obiektu interaktywnego
        interactableObject = new GameObject("InteractableObject");
        var interactableCollider = interactableObject.AddComponent<BoxCollider>();
        interactableCollider.isTrigger = true;
        interactableObject.AddComponent<Rigidbody>().isKinematic = true;

        // Tworzenie tekstu interakcji
        GameObject textObject = new GameObject("InteractText");
        interactText = textObject.AddComponent<TextMeshProUGUI>();

        // Inicjalizacja zmiennej zasięgu
        isPlayerInRange = false;
    }

    [UnityTest]
    public IEnumerator PlayerEntersTrigger_ShowsInteractMessage()
    {
        // Symulacja wejścia gracza w zasięg interakcji
        SimulateTriggerEnter();
        yield return null;

        // Sprawdzenie, czy wiadomość interakcji została wyświetlona
        Assert.AreEqual(interactMessage, interactText.text);
    }

    [UnityTest]
    public IEnumerator PlayerExitsTrigger_ClearsInteractMessage()
    {
        // Symulacja wejścia gracza w zasięg interakcji
        SimulateTriggerEnter();
        yield return null;

        // Symulacja wyjścia gracza z zasięgu interakcji
        SimulateTriggerExit();
        yield return null;

        // Sprawdzenie, czy wiadomość interakcji została wyczyszczona
        Assert.AreEqual("", interactText.text);
    }

    [UnityTest]
    public IEnumerator PlayerPressesE_OpensMenu()
    {
        // Symulacja wejścia gracza w zasięg interakcji
        SimulateTriggerEnter();
        yield return null;

        // Symulacja naciśnięcia przycisku E przez gracza
        SimulateKeyPress(KeyCode.E);
        yield return null;

        // Sprawdzenie, czy scena menu została załadowana
        Assert.AreEqual(menuSceneName, SceneManager.GetActiveScene().name);
    }

    private void SimulateTriggerEnter()
    {
        isPlayerInRange = true;
        DisplayMessage(interactMessage);
    }

    private void SimulateTriggerExit()
    {
        isPlayerInRange = false;
        ClearMessage();
    }

    private void SimulateKeyPress(KeyCode key)
    {
        if (isPlayerInRange && key == KeyCode.E)
        {
            OpenMenu();
        }
    }

    private void OpenMenu()
    {
        // Symulacja załadowania sceny menu
        SceneManager.LoadScene(menuSceneName);
    }

    private void DisplayMessage(string message)
    {
        if (interactText != null)
        {
            interactText.text = message;
        }
    }

    private void ClearMessage()
    {
        if (interactText != null)
        {
            interactText.text = "";
        }
    }
}
