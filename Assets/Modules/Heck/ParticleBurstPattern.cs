using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    [CreateAssetMenu(fileName = "New ParticleBurstPattern", menuName = "Heck/ParticleBurstPattern")]
    public class ParticleBurstPattern : Pattern {
        public override bool CanCancel() {
            return false;
        }
        public override IEnumerator DoPattern(Character owner, Transform origin, MoveInteraction interaction = null) {
            yield return new WaitForSeconds(interaction.timeStep.x);
            Vector3 translation = (end - start) / (burstCount + 1);
            int currentBurst = 0;
            float interval = interaction.timeStep.y / (burstCount + 1);
            float elapsedTime = 0;
            float nextBurstTime = 0;
            GameObject particleHolder = new GameObject();
            particleHolder.transform.position = origin.position;
            particleHolder.transform.rotation = origin.rotation;
            GameObject instance = Instantiate(bursterPrefab, particleHolder.transform);
            ParticleSystem particle = instance.GetComponent<ParticleSystem>();
            while (currentBurst < burstCount) {
                nextBurstTime = currentBurst * interval;
                while (elapsedTime < nextBurstTime) {
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                particle.Stop(true);
                instance.transform.localPosition = start + translation * currentBurst;
                particle.Play(true);
                currentBurst++;
            }
            Destroy(particleHolder, particle.main.duration / particle.main.simulationSpeed);
        }
        public Vector3 start;
        public Vector3 end;
        public int burstCount;
        public GameObject bursterPrefab;
    }
}

