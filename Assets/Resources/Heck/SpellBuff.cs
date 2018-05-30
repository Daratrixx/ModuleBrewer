using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New SpellBuff", menuName = "Heck/SpellBuff")]
    public class SpellBuff : Spell {

        public Buff buff;
        public float duration;

        public override IEnumerator OnTriggerEffects(Character caster, Character target) {
            caster.AddBuff(new BuffInstance { buff = buff, duration = duration, origin = caster, target = caster, canExpire = true });
            yield return null;
        }

    }
}
