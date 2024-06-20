using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TMPro;

public class LeverInteractionTest
{
    private GameObject player;
    private GameObject door;
    private GameObject leverHandle;
    private TMP_Text interactText;
    private bool playerInRange;
    private bool doorOpen;
    private bool isLeverRotating;
    private Quaternion initialLeverRotation;
    private Quaternion finalLeverRotation;
    private float leverRotationAngle = 45f;
    private float leverRotationSpeed = 2f;

    [SetUp]
    public void Setup()
    {
        // Tworzenie obiektu gracza
        player = new GameObject("Player");
        player.tag = "Player";
        var playerCollider = player.AddComponent<BoxCollider>();
        playerCollider.isTrigger = true;

        // Tworzenie obiektu drzwi
        door = new GameObject("Door");
        var doorAnimator = door.AddComponent<Animator>();

        // Tworzenie uchwytu dźwigni
        leverHandle = new GameObject("LeverHandle");
        leverHandle.transform.localRotation = Quaternion.identity;

        // Tworzenie tekstu interakcji
        GameObject textObject = new GameObject("InteractText");
        interactText = textObject.AddComponent<TextMeshProUGUI>();

        // Inicjalizacja zmiennych
        playerInRange = false;
        doorOpen = false;
        isLeverRotating = false;
        initialLeverRotation = leverHandle.transform.localRotation;
        finalLeverRotation = initialLeverRotation * Quaternion.Euler(-leverRotationAngle, 0, 0);
    }

    [UnityTest]
    public IEnumerator PlayerEntersTrigger_ShowsInteractMessage()
    {
        // Symulacja wejścia gracza w zasięg interakcji
        SimulateTriggerEnter();
        yield return null;

        // Sprawdzenie, czy wiadomość interakcji została wyświetlona
        Assert.AreEqual("PRESS E TO INTERACT", interactText.text);
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
    public IEnumerator PlayerPressesE_TogglesDoorState()
    {
        // Symulacja wejścia gracza w zasięg interakcji
        SimulateTriggerEnter();
        yield return null;

        // Symulacja naciśnięcia przycisku E przez gracza
        SimulateKeyPress(KeyCode.E);
        yield return null;

        // Sprawdzenie, czy drzwi zostały otwarte
        var doorAnimator = door.GetComponent<Animator>();
        Assert.IsFalse(doorAnimator.GetBool("!isOpen"));

        // Symulacja ponownego naciśnięcia przycisku E przez gracza
        SimulateKeyPress(KeyCode.E);
        yield return null;

        // Sprawdzenie, czy drzwi zostały zamknięte
        Assert.IsFalse(doorAnimator.GetBool("!isOpen"));
    }

    [UnityTest]
    public IEnumerator LeverRotatesWhenToggled()
    {
        SimulateTriggerEnter();
        yield return null;

        SimulateKeyPress(KeyCode.E);
        yield return new WaitForSeconds(1f); 

        // Sprawdzenie, czy dźwignia obróciła się do końcowej pozycji
        Assert.AreEqual(finalLeverRotation.eulerAngles, leverHandle.transform.localRotation.eulerAngles);

        // Symulacja ponownego naciśnięcia przycisku E przez gracza
        SimulateKeyPress(KeyCode.E);
        yield return new WaitForSeconds(1f); // Czekanie na powrót dźwigni

        // Sprawdzenie, czy dźwignia wróciła do początkowej pozycji
        Assert.AreEqual(initialLeverRotation.eulerAngles, leverHandle.transform.localRotation.eulerAngles);
    }

    private void SimulateTriggerEnter()
    {
        playerInRange = true;
        DisplayMessage("PRESS E TO INTERACT");
    }

    private void SimulateTriggerExit()
    {
        playerInRange = false;
        ClearMessage();
    }

    private void SimulateKeyPress(KeyCode key)
    {
        if (playerInRange && key == KeyCode.E)
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        doorOpen = !doorOpen;
        var doorAnimator = door.GetComponent<Animator>();
        doorAnimator.SetBool("isOpen", doorOpen);

        // Rozpoczęcie obrotu dźwigni
        isLeverRotating = true;
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

    private void Update()
    {
        if (isLeverRotating)
        {
            if (doorOpen)
            {
                leverHandle.transform.localRotation = Quaternion.Lerp(leverHandle.transform.localRotation, finalLeverRotation, Time.deltaTime * leverRotationSpeed);
                if (Quaternion.Angle(leverHandle.transform.localRotation, finalLeverRotation) < 0.1f)
                {
                    leverHandle.transform.localRotation = finalLeverRotation;
                    isLeverRotating = false;
                }
            }
            else
            {
                leverHandle.transform.localRotation = Quaternion.Lerp(leverHandle.transform.localRotation, initialLeverRotation, Time.deltaTime * leverRotationSpeed);
                if (Quaternion.Angle(leverHandle.transform.localRotation, initialLeverRotation) < 0.1f)
                {
                    leverHandle.transform.localRotation = initialLeverRotation;
                    isLeverRotating = false;
                }
            }
        }
    }
}
