using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class AttackableObject : MonoBehaviour, Attackable {

        public bool CanDamage() {
            return true;
        }

        public void Damage(int damage) {
            Debug.Log("Object was attacked for " + damage + " damages.");
            Die();
        }
        public void Damage(float amount, DamageType type) {
            Debug.Log("Object was attacked for " + (int)amount + " " + type.ToString() + " damages.");
            Die();
        }

        public virtual void Die() {
            FadeObject();
        }

        public Attackable GetAttackableRoot() {
            return this;
        }

        public void FadeObject() {
            StartCoroutine(DoFadeObject());
        }

        private IEnumerator DoFadeObject() {
            GetComponent<Collider>().enabled = false;
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            yield return new WaitForSeconds(0.3f);
            for (float i = 1; i > 0; i -= Time.fixedDeltaTime * 3) {
                foreach (Material m in renderer.materials) {
                    m.SetColor("_EmissionColor", new Color(i / 2, i / 2, i / 2));
                }
                yield return new WaitForFixedUpdate();
            }
            for (float i = 1; i > 0; i -= Time.fixedDeltaTime * 3) {
                foreach (Material m in renderer.materials) {
                    m.color = new Color(m.color.r, m.color.g, m.color.b, i);
                }
                yield return new WaitForFixedUpdate();
            }
            Destroy(this.gameObject);
        }

    }

}
