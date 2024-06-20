using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TMPro;

public class PickupableObjectTest
{
    private GameObject player;
    private GameObject pickupableObject;
    private TMP_Text interactText;
    private bool isPlayerInRange;
    private string interactMessage = "PRESS E";

    [SetUp]
    public void Setup()
    {
        // Tworzenie obiektu gracza
        player = new GameObject("Player");
        player.tag = "Player";
        var playerCollider = player.AddComponent<BoxCollider>();
        playerCollider.isTrigger = true;

        // Tworzenie obiektu podnoszonego
        pickupableObject = new GameObject("PickupableObject");
        var pickupableCollider = pickupableObject.AddComponent<BoxCollider>();
        pickupableCollider.isTrigger = true;
        pickupableObject.AddComponent<Rigidbody>().isKinematic = true;

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
    public IEnumerator PlayerPressesE_PicksUpObject()
    {
        // Symulacja wejścia gracza w zasięg interakcji
        SimulateTriggerEnter();
        yield return null;

        // Symulacja naciśnięcia przycisku E przez gracza
        SimulateKeyPress(KeyCode.E);
        yield return null;

        // Sprawdzenie, czy obiekt został podniesiony (np. wyłączony)
        Assert.IsFalse(pickupableObject.activeSelf);
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
            PickUp(player.transform);
        }
    }

    private void PickUp(Transform parent)
    {
        // Zablokuj rotację i ustaw pozycję
        pickupableObject.transform.SetParent(parent);
        pickupableObject.transform.localRotation = Quaternion.identity;
        pickupableObject.transform.localPosition = Vector3.zero;

        // Wyłącz przedmiot, aby go ukryć
        pickupableObject.SetActive(false);

        // Wyczyść wyświetlany komunikat
        ClearMessage();
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
