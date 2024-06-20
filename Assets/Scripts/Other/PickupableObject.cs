using UnityEngine;
using TMPro;

public class PickupableObject : MonoBehaviour
{
    public TMP_Text interactText;
    public string interactMessage = "PRESS E";
    private bool isPlayerInRange = false;

    void Update()
    {
        // Obsługa interakcji tylko wtedy, gdy gracz jest w zasięgu i nacisnął klawisz E
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key was pressed."); // Debug log for key press

            // Tutaj możesz dodać kod do podnoszenia przedmiotu
            // Na razie wywołajmy metodę PickUp() dla przykładu
            PickUp(transform.parent);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Sprawdź, czy gracz wszedł w zasięg
        {
            isPlayerInRange = true;
            DisplayMessage(interactMessage); // Zaktualizuj wyświetlany komunikat
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Sprawdź, czy gracz opuścił zasięg
        {
            isPlayerInRange = false;
            ClearMessage(); // Wyczyść wyświetlany komunikat
        }
    }

    public void PickUp(Transform parent)
    {
        // Zablokuj rotację i ustaw pozycję
        transform.SetParent(parent);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;

        // Wyłącz przedmiot, aby go ukryć (opcjonalnie)
        gameObject.SetActive(false);

        FindObjectOfType<DoorInteraction1>().PickUpKey();

        // Wyczyść wyświetlany komunikat
        ClearMessage();
    }

    // Metoda do upuszczania przedmiotu
    public void Drop()
    {
        // Przywróć pierwotnego rodzica
        transform.SetParent(null);

        // Włącz ponownie przedmiot
        gameObject.SetActive(true);

        // Zaktualizuj wyświetlany komunikat
        DisplayMessage(interactMessage);
    }

    void DisplayMessage(string message)
    {
        if (interactText != null) // Sprawdź, czy interactText nie jest null
        {
            // Wyświetl komunikat na ekranie
            interactText.text = message;
        }
    }

    void ClearMessage()
    {
        if (interactText != null) // Sprawdź, czy interactText nie jest null
        {
            // Wyczyść komunikat z ekranu
            interactText.text = "";
        }
    }
}
