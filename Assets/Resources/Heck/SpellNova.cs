using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New SpellNova", menuName = "Heck/SpellNova")]
    public class SpellNova : Spell {

        public NovaDamageProfile[] damages = new NovaDamageProfile[0];

        public override IEnumerator OnTriggerEffects(Character caster, Character target) {
            foreach (NovaDamageProfile nova in damages) {
                if (nova.particleBurst != null)
                    caster.StartCoroutine(DoNovaParticle(caster, nova));
                caster.StartCoroutine(DoNovaDamage(caster, nova));
            }
            yield return null;
        }

        private IEnumerator DoNovaDamage(Character caster, NovaDamageProfile nova) {
            yield return new WaitForSeconds(nova.damageDelay);
            foreach (Character c in Character.characters
                .Where(x =>
                    x.CanDamage()
                    && x != caster
                    && x.team != caster.team
                    && Vector3.Distance(caster.transform.position + caster.transform.rotation * nova.centerOffset, x.transform.position) <= nova.damageRadius)) {
                c.Damage((int)ComputeDamage(caster, c, nova.damageType, nova.damageAmount, nova.damageAffinity));
            }
        }

        private IEnumerator DoNovaParticle(Character caster, NovaDamageProfile nova) {
            yield return new WaitForSeconds(nova.particleDelay);
            ParticleSystem burst = GameObject.Instantiate(nova.particleBurst,
                caster.transform.position + caster.transform.rotation * nova.centerOffset,
                caster.transform.rotation)
                .GetComponent<ParticleSystem>();
            burst.Play(true);
            GameObject.Destroy(burst.gameObject, burst.main.duration);
        }

    }

    [System.Serializable]
    public struct NovaDamageProfile {
        public DamageType damageType;
        public float damageAmount;
        public float damageAffinity;
        public float damageRadius;
        public float damageDelay;
        public GameObject particleBurst;
        public float particleDelay;
        public Vector3 centerOffset;
    }
}
