using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class TentacleController : MonoBehaviour {

        public Character character;
        public Character target;

        public float agroRange = 20f;
        public float attackRange = 7f;

        public float attackInterval = 5f;
        private float attackCooldown = 5f;

        private void Update() {
            if (character != null && character.isAlive) {
                if (target != null && target.isAlive) {
                    if (IsInRange(target)) {
                        Movement(target);
                        Attack(target);
                    }
                } else {
                    target = null;
                }
                if (attackCooldown > 0)
                    attackCooldown -= Time.fixedDeltaTime;
            }
        }

        private float ditanceToTarget;

        private bool IsInRange(Character target) {
            ditanceToTarget = Vector3.Distance(character.transform.position, target.transform.position);
            return ditanceToTarget < agroRange;
        }

        private void Movement(Character target) {
            character.Rotate(Vector3.Scale(target.transform.position - character.transform.position, new Vector3(1, 0, 1)).normalized);
            character.NoMove();
        }

        private void Attack(Character target) {
            if (ditanceToTarget < attackRange && attackCooldown <= 0) {
                attackCooldown = attackInterval;
                character.Attack();
            }
        }

    }

}
