using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    [CreateAssetMenu(fileName = "New DamagePattern", menuName = "Heck/DamagePattern")]
    public class DamagePattern : Pattern {
        public override bool CanCancel() {
            return false;
        }
        public override IEnumerator DoPattern(Character owner, Transform origin, MoveInteraction interaction = null) {
            float elapsedTime = 0;
            float progression = 0;
            float previousProgression = 0;
            Vector3 startPoint = origin.transform.TransformPoint(start);
            Vector3 endPoint = origin.transform.TransformPoint(end);
            Vector3 direction = endPoint - startPoint;
            yield return new WaitForSeconds(interaction.timeStep.x);
            HashSet<Attackable> targets = new HashSet<Attackable>();
            while (elapsedTime < interaction.timeStep.y) {
                previousProgression = progression;
                elapsedTime += Time.deltaTime;
                progression = elapsedTime / interaction.timeStep.y;
                switch (collisionType) {
                    case CollisionType.Capsule:
                        Damage(TargetsInCapsule(startPoint + direction * previousProgression,
                            startPoint + direction * progression, damageAreaSize), owner, targets);
                        break;
                    case CollisionType.Sphere:
                        Damage(TargetsInSphere(startPoint + direction * progression, damageAreaSize), owner, targets);
                        break;
                    case CollisionType.Cube:
                        Damage(TargetsInCube(startPoint + direction * progression, damageAreaSize), owner, targets);
                        break;
                    default:
                        break;
                }
                yield return null;
            }
        }
        public Vector3 start;
        public Vector3 end;
        public float damageAreaSize;
        public int damageAmount;
        public DamageType damageType;
        public CollisionType collisionType = CollisionType.Sphere;

        private void Damage(Collider[] colliders, Attackable origin, ICollection<Attackable> targets) {
            foreach (Collider collider in colliders) {
                Attackable attackable = collider.GetComponent<Attackable>();
                if (attackable == null || attackable == origin)
                    continue;
                Attackable attackableRoot = attackable.GetAttackableRoot();
                if (!targets.Contains(attackableRoot) && attackable.CanDamage()) {
                    attackable.Damage(damageAmount, damageType);
                    targets.Add(attackableRoot);
                }
            }
        }

        public static Collider[] TargetsInCapsule(Vector3 start, Vector3 end, float damageAreaSize) {
            return Physics.OverlapCapsule(start, end, damageAreaSize, 1 << 8);
        }

        public static Collider[] TargetsInSphere(Vector3 center, float damageAreaSize) {
            return Physics.OverlapSphere(center, damageAreaSize, 1 << 8);
        }

        public static Collider[] TargetsInCube(Vector3 center, float damageAreaSize) {
            return Physics.OverlapBox(center, new Vector3(damageAreaSize, damageAreaSize, damageAreaSize), Quaternion.identity, 1 << 8);
        }
    }

    public enum CollisionType {
        Cube, Sphere, Capsule
    }

}

