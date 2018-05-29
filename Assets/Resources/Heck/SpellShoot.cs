using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New SpellShoot", menuName = "Heck/SpellShoot")]
    public class SpellShoot : Spell {

        public MissileData missileData;
        public Vector3 spawnOffset;

        public override IEnumerator OnTriggerEffects(Character caster, Character target) {
            GameObject instance = GameObject.Instantiate(missileData.missilePrefab,
                caster.transform.position + caster.transform.rotation * spawnOffset,
                caster.transform.rotation);
            MissileInstance m = instance.GetComponent<MissileInstance>();
            m.owner = caster;
            if (target != null) {
                m.target = target.transform;
                m.transform.forward = (m.target.position - caster.transform.position);
            }
            yield return null;
        }

    }
}
