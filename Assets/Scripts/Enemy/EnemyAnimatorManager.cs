using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLMJ
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            if (delta > 0)
    {
        enemyManager.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidBody.velocity = velocity;
    }
            
        }
    }
}