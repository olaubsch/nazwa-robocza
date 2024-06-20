using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 2f; // Zasięg interakcji
    private PickupableObject currentObject; // Aktualnie podnoszony przedmiot

    void Update()
    {
        // Sprawdzanie czy gracz naciska klawisz interakcji (np. E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Wykonaj metodę do interakcji
            Interact();
        }

        // Upuszczenie przedmiotu
        if (Input.GetKeyDown(KeyCode.Q) && currentObject != null)
        {
            currentObject.Drop();
            currentObject = null;
        }
    }

    void Interact()
    {
        // Wykonaj raycast w kierunku, w którym patrzy gracz
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            // Sprawdź, czy trafiony obiekt ma skrypt PickupableObject
            PickupableObject pickupObject = hit.collider.GetComponent<PickupableObject>();
            if (pickupObject != null)
            {
                // Podnieś przedmiot, przekazując rodzica (gracza)
                pickupObject.PickUp(transform);
                currentObject = pickupObject;
            }
        }
    }
}
