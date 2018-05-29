using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class Weapon : MonoBehaviour {

        public string weaponName;
        public MoveSet moveSet;
        public Character wielder;

        public DamageElement[] weaponParts;

        public void DoDamage(float damageRatio, ICollection<Attackable> targets) {
            for (int i = 0; i < weaponParts.Length; ++i) {
                foreach (Collider collider in GetCollidersInBoxCollider(weaponParts[i].collider)) {
                    Attackable attackable = collider.GetComponent<Attackable>();
                    if (attackable == null || attackable == (Attackable)wielder)
                        continue;
                    attackable = attackable.GetAttackableRoot();
                    if (!targets.Contains(attackable) && attackable.CanDamage()) {
                        targets.Add(attackable);
                        attackable.Damage(damageRatio * weaponParts[i].damageAmount, weaponParts[i].damageType);
                    }
                }
            }
        }

        private Collider[] GetCollidersInBoxCollider(BoxCollider collider) {
            return Physics.OverlapBox(collider.transform.position + collider.center,
                collider.size,
                collider.transform.rotation);
        }

    }

}
