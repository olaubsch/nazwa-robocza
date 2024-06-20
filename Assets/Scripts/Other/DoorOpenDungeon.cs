using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorInteraction1 : MonoBehaviour
{
    public TMP_Text interactText; // Referencja do komponentu Text UI, aby wyświetlić komunikat
    public string interactMessageOpen = "Naciśnij E, aby otworzyć";
    public string interactMessageClose = "Naciśnij E, aby zamknąć";
    public Animator doorAnimator; // Animator drzwi

    private bool isPlayerInRange = false; // Czy gracz znajduje się w zasięgu interakcji
    private bool isOpen = false; // Czy drzwi są otwarte
    public GameObject keyRequired; 
    private bool hasKey = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Sprawdź, czy gracz wszedł w zasięg
        {
            isPlayerInRange = true;
            UpdateInteractMessage(); // Zaktualizuj wyświetlany komunikat
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

    void UpdateInteractMessage()
    {
        // Zaktualizuj tekst komunikatu w zależności od stanu drzwi i dostępności klucza
        if (hasKey)
        {
            interactText.text = isOpen ? interactMessageClose : interactMessageOpen;
        }
        else
        {
            interactText.text = "YOU NEED KEY";
        }
    }

    void Update()
    {
        // Sprawdź, czy gracz jest w zasięgu i nacisnął przycisk E
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (hasKey)
            {
                if (isOpen)
                {
                    // Jeśli drzwi są otwarte, zamknij je
                    CloseDoor();
                }
                else
                {
                    // Jeśli drzwi są zamknięte, otwórz je
                    OpenDoor();
                }
            }
            else
            {
                Debug.Log("Potrzebujesz klucza!"); // Debug log - brak klucza
            }
        }
    }

    void ClearMessage()
    {
        interactText.text = ""; // Wyczyść tekst komunikatu
    }

    public void PickUpKey()
    {
        hasKey = true; // Ustaw flagę, że gracz ma klucz
        UpdateInteractMessage(); // Zaktualizuj komunikat
    }

    void OpenDoor()
    {
        isOpen = true; // Ustaw stan drzwi na otwarte
        doorAnimator.SetBool("isOpen", true); // Ustaw parametr Animatora
        UpdateInteractMessage(); // Zaktualizuj wyświetlany komunikat
    }

    // Metoda zamykająca drzwi
    void CloseDoor()
    {
        isOpen = false; // Ustaw stan drzwi na zamknięte
        doorAnimator.SetBool("isOpen", false); // Ustaw parametr Animatora
        UpdateInteractMessage(); // Zaktualizuj wyświetlany komunikat
    }
}
