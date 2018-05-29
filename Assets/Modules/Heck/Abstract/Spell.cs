using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    public abstract class Spell : ScriptableObject {

        public string spellName;
        [TextArea]
        public string spellDescription;
        public string spellIconPath;

        public float affinity; // 0.0 to 2.0 define effectiveness of stats and res for effect adjustement
        public float fatigue = 10; // energy debuff on cast. decrease maximum energy for that amount. recover 1 every seconds.


        public float castDuration = 0.2f;

        public abstract IEnumerator OnTriggerEffects(Character caster, Character target);



        public static float ComputeDamage(Character origin, Character target, DamageType type, float amount, float affinity) {
            return DamageFormula(amount, affinity, 0, 0);
        }



        private static float DamageFormula(float damage, float affinity, float boost, float reduction) {
            return damage * (1 + affinity * (boost - reduction));
        }
    }
}
