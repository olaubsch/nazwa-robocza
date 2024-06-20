using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


  public class DestructibleWall : MonoBehaviour
  {
    public int health = 25;

    public void TakeDamage(int damage)
    {
      health -= damage;
      if (health <= 0)
      {
        Destroy(gameObject);
      }
    }
  }

public class DestructibleWallTests
{
    private GameObject wallObject;
    private DestructibleWall destructibleWall;

    [SetUp]
    public void SetUp()
    {
        wallObject = new GameObject();
        destructibleWall = wallObject.AddComponent<DestructibleWall>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(wallObject);
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        // Arrange
        int initialHealth = destructibleWall.health;
        int damage = 10;

        // Act
        destructibleWall.TakeDamage(damage);

        // Assert
        Assert.AreEqual(initialHealth - damage, destructibleWall.health);
    }

    [UnityTest]
    public IEnumerator TakeDamage_DestroysGameObject_WhenHealthZeroOrBelow()
    {
        // Arrange
        destructibleWall.health = 5;
        int damage = 10;

        // Act
        destructibleWall.TakeDamage(damage);

        // Wait for end of frame to allow object destruction
        yield return null;

        // Assert
        Assert.IsTrue(destructibleWall == null); // Object should be destroyed
    }
}
