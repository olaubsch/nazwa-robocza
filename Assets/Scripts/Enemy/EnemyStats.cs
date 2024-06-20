using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace OLMJ
{
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        private bool isAlive = true;

        Animator animator;
        Rigidbody rigidbody;
        Collider collider;
        NavMeshAgent navMeshAgent;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (!isAlive) return;

            currentHealth -= damage;

            animator.Play("TakeDamage01");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isAlive = false;
                animator.Play("Death01_A");

                StartCoroutine(DelayedDestroy(2.0f));
            }
        }

        private void HandleEnemyDeath()
        {
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
                rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }

            if (collider != null)
            {
                collider.enabled = false;
            }

            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
            }
        }

        IEnumerator DelayedDestroy(float delay)
        {
            yield return new WaitForSeconds(delay);

            HandleEnemyDeath();

            Destroy(gameObject);
        }
    }
}