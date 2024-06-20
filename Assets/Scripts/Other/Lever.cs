using UnityEngine;
using TMPro;

public class LeverInteraction : MonoBehaviour
{
    public GameObject doorToOpen; // Obiekt drzwi, które mają się otworzyć
    public KeyCode interactKey = KeyCode.E; // Klawisz interakcji
    public TMP_Text interactText; // TextMeshPro do wyświetlania komunikatu
    public string interactMessage = "PRESS E TO INTERACT";
    public Transform leverHandle; // Uchwyt dźwigni
    public float leverRotationAngle = 45f; // Kąt obrotu dźwigni
    public float leverRotationSpeed = 2f; // Szybkość obrotu dźwigni

    private Animator doorAnimator;
    private Quaternion initialLeverRotation;
    private Quaternion finalLeverRotation;
    private bool doorOpen = false;
    private bool playerInRange = false;
    private bool isLeverRotating = false;

    void Start()
    {
        // Pobieranie animatora drzwi
        doorAnimator = doorToOpen.GetComponent<Animator>();
        // Ustawienie początkowej i końcowej rotacji dźwigni
        initialLeverRotation = leverHandle.localRotation;
        finalLeverRotation = initialLeverRotation * Quaternion.Euler(-leverRotationAngle, 0, 0); // Obrót w dół
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ToggleDoor();
        }

        // Obracanie dźwigni
        if (isLeverRotating)
        {
            if (doorOpen)
            {
                leverHandle.localRotation = Quaternion.Lerp(leverHandle.localRotation, finalLeverRotation, Time.deltaTime * leverRotationSpeed);
                if (Quaternion.Angle(leverHandle.localRotation, finalLeverRotation) < 0.1f)
                {
                    leverHandle.localRotation = finalLeverRotation;
                    isLeverRotating = false;
                }
            }
            else
            {
                leverHandle.localRotation = Quaternion.Lerp(leverHandle.localRotation, initialLeverRotation, Time.deltaTime * leverRotationSpeed);
                if (Quaternion.Angle(leverHandle.localRotation, initialLeverRotation) < 0.1f)
                {
                    leverHandle.localRotation = initialLeverRotation;
                    isLeverRotating = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            DisplayMessage(interactMessage);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ClearMessage();
        }
    }

    void ToggleDoor()
    {
        doorOpen = !doorOpen;
        doorAnimator.SetBool("isOpen", doorOpen);

        // Rozpoczęcie obrotu dźwigni
        isLeverRotating = true;
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
