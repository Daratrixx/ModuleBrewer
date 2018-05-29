using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    [CreateAssetMenu(fileName = "New SpawnMissilePattern", menuName = "Heck/SpawnMissilePattern")]
    public class SpawnMissilePattern : Pattern {
        public override bool CanCancel() {
            return false;
        }
        public override IEnumerator DoPattern(Character owner, Transform origin, MoveInteraction interaction = null) {
            yield return new WaitForSeconds(interaction.timeStep.x);
            for (int i = 0; i < missileCount; ++i) {
                GameObject instance = GameObject.Instantiate(missileData.missilePrefab, origin.position, origin.rotation);
                instance.GetComponent<MissileInstance>().owner = owner;
            }
            yield return null;
        }
        public int missileCount;
        public MissileData missileData;
    }
}

