using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OLMJ
{
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
}