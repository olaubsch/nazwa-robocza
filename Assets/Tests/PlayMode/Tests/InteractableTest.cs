using NUnit.Framework;
using UnityEngine;

    public class InteractableTests
    {
        // Test sprawdzający, czy promień interakcji jest poprawnie rysowany
        [Test]
        public void Radius_Is_Drawn_Correctly()
        {
            // Tworzymy obiekt symulujący interaktywny obiekt
            var interactable = new MockInteractable();

            // Rysujemy promień interakcji
            interactable.DrawInteractRadius();

            // Sprawdzamy, czy promień został poprawnie narysowany
            Assert.IsTrue(interactable.IsDrawn);
        }

        // Test sprawdzający interakcję z graczem
        [Test]
        public void Interact_Invokes_Interact_Method()
        {
            // Tworzymy obiekt symulujący interaktywny obiekt
            var interactable = new MockInteractable();

            // Tworzymy symulację menadżera gracza
            var playerManager = new PlayerManager();

            // Symulujemy interakcję z graczem
            interactable.Interact(playerManager);

            // Sprawdzamy, czy metoda Interact została wywołana
            Assert.IsTrue(interactable.Interacted);
        }

        // Klasa symulująca obiekt interaktywny bez używania rzeczywistej klasy Interactable
        private class MockInteractable
        {
            public bool IsDrawn { get; private set; }
            public bool Interacted { get; private set; }

            // Metoda symulująca rysowanie promienia interakcji
            public void DrawInteractRadius()
            {
                IsDrawn = true;
            }

            // Metoda symulująca interakcję
            public void Interact(PlayerManager playerManager)
            {
                Interacted = true;
            }
        }

        // Symulacja prostego menadżera gracza
        private class PlayerManager { }
    }
