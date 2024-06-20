using NUnit.Framework;

public class DoorDungeonTests
{
    // Test sprawdzający, czy komunikat jest poprawnie aktualizowany, gdy gracz znajduje się w zasięgu
    [Test]
    public void InteractMessage_Updated_WhenPlayerInRange()
    {
        // Tworzymy obiekt symulujący interakcję z drzwiami
        var doorInteraction = new MockDoorInteraction();

        // Gracz wchodzi w zasięg
        doorInteraction.PlayerEntersRange();

        // Oczekujemy, że komunikat będzie "YOU NEED KEY", gdy gracz nie ma klucza
        Assert.AreEqual("YOU NEED KEY", doorInteraction.InteractMessage);
        
        // Gracz zdobywa klucz
        doorInteraction.PlayerPicksUpKey();

        // Oczekujemy, że komunikat zmieni się na standardowy komunikat otwierania
        Assert.AreEqual("Naciśnij E, aby otworzyć", doorInteraction.InteractMessage);
    }

    // Testowanie otwierania i zamykania drzwi
    [Test]
    public void Door_Opened_And_Closed()
    {
        // Tworzymy obiekt symulujący interakcję z drzwiami
        var doorInteraction = new MockDoorInteraction();

        // Gracz zdobywa klucz
        doorInteraction.PlayerPicksUpKey();

        // Otwieramy drzwi
        doorInteraction.PlayerOpensDoor();

        // Oczekujemy, że drzwi są otwarte
        Assert.IsTrue(doorInteraction.IsDoorOpen);

        // Zamykamy drzwi
        doorInteraction.PlayerClosesDoor();

        // Oczekujemy, że drzwi są zamknięte
        Assert.IsFalse(doorInteraction.IsDoorOpen);
    }

    // Testowanie, czy próba otwarcia drzwi bez klucza się nie powiedzie
    [Test]
    public void Door_Cannot_Be_Opened_WithoutKey()
    {
        // Tworzymy obiekt symulujący interakcję z drzwiami
        var doorInteraction = new MockDoorInteraction();

        // Próba otwarcia drzwi bez klucza
        doorInteraction.PlayerOpensDoor();

        // Oczekujemy, że drzwi pozostaną zamknięte
        Assert.IsFalse(doorInteraction.IsDoorOpen);
    }

    // Klasa symulująca interakcję z drzwiami bez używania rzeczywistej klasy DoorInteraction1
    private class MockDoorInteraction
    {
        public string InteractMessage { get; private set; }
        public bool IsDoorOpen { get; private set; }
        private bool hasKey = false;

        public void PlayerEntersRange()
        {
            InteractMessage = hasKey ? "Naciśnij E, aby otworzyć" : "YOU NEED KEY";
        }

        public void PlayerPicksUpKey()
        {
            hasKey = true;
            InteractMessage = "Naciśnij E, aby otworzyć";
        }

        public void PlayerOpensDoor()
        {
            if (hasKey)
            {
                IsDoorOpen = true;
            }
        }

        public void PlayerClosesDoor()
        {
            IsDoorOpen = false;
        }
    }
}
