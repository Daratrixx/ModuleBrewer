using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New BuffRegeneration", menuName = "Heck/BuffRegeneration")]
    public class BuffRegeneration : Buff {
        
        public int healthPerPeriod = 0;
        public int energyPerPeriod = 0;
        public int fatiguePerPeriod = 0;

        protected override IEnumerator DoOnPeriod(BuffInstance instance) {
            instance.target.Heal(healthPerPeriod);
            yield return null;
        }

    }
}
