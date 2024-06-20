using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace OLMJ
{
  public class DoorInteraction : MonoBehaviour
  {
    public TMP_Text interactText; // Referencja do komponentu Text UI, aby wyświetlić komunikat
    public string interactMessageOpen = "Naciśnij E, aby otworzyć";
    public string interactMessageClose = "Naciśnij E, aby zamknąć";
    public Animator doorAnimator; // Animator drzwi

    private bool isPlayerInRange = false; // Czy gracz znajduje się w zasięgu interakcji
    private bool isOpen = false; // Czy drzwi są otwarte

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
      // Zaktualizuj tekst komunikatu w zależności od stanu drzwi
      interactText.text = isOpen ? interactMessageClose : interactMessageOpen;
    }

    void Update()
    {
      Debug.Log("Update method called.");
      // Sprawdź, czy gracz jest w zasięgu i nacisnął przycisk E
      if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
      {
        Debug.Log("E key was pressed."); // Debug log for key press

        isOpen = !isOpen; // Zmień stan drzwi
        doorAnimator.SetBool("isOpen", isOpen); // Ustaw parametr Animatora

        Debug.Log("Door state changed to: " + isOpen); // Debug log for door state

        UpdateInteractMessage(); // Zaktualizuj wyświetlany komunikat
      }
    }

    void ClearMessage()
    {
      interactText.text = ""; // Wyczyść tekst komunikatu
    }
  }
}
