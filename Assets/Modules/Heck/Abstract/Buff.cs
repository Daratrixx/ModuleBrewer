using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    public abstract class Buff : ScriptableObject {

        public string buffName;
        [TextArea]
        public string buffDescription;
        public string buffIconPath;

        public float periodInterval = 1f;

        public GameObject startBurstParticle;
        public Vector3 startBurstOffset;
        public bool startBurstAttach;
        public GameObject startLoopParticle;
        public Vector3 startLoopOffset;
        public GameObject periodBurstParticle;
        public Vector3 periodBurstOffset;
        public bool periodBurstAttach;
        public GameObject endBurstParticle;
        public Vector3 endBurstOffset;
        public bool endBurstAttach;

        public virtual IEnumerator OnStart(BuffInstance instance) {
            OnStartBurst(instance);
            OnStartLoop(instance);
            instance.period = instance.target.StartCoroutine(OnPeriod(instance));
            yield return null;
        }

        public virtual IEnumerator OnPeriod(BuffInstance instance) {
            while (!instance.canExpire || instance.duration > 0) {
                OnPeriodBurst(instance);
                instance.target.StartCoroutine(DoOnPeriod(instance));
                yield return new WaitForSeconds(periodInterval);
                instance.duration -= periodInterval;
            }
            instance.Stop();
        }

        protected virtual IEnumerator DoOnPeriod(BuffInstance instance) {
            yield return null;
        }

        public virtual IEnumerator OnEnd(BuffInstance instance) {
            OnEndBurst(instance);
            yield return null;
        }



        #region PARTICLES
        protected void OnStartBurst(BuffInstance instance) {
            if (startBurstParticle != null) {
                if (startBurstAttach) {
                    ParticleSystem ps = GameObject.Instantiate(startBurstParticle,
                        instance.target.transform).
                        GetComponent<ParticleSystem>();
                    ps.transform.localPosition = startBurstOffset;
                    ps.Play(true);
                    GameObject.Destroy(ps.gameObject, ps.main.duration);
                } else {
                    ParticleSystem ps = GameObject.Instantiate(startBurstParticle,
                        instance.target.transform.position + instance.target.transform.rotation * startBurstOffset,
                        instance.target.transform.rotation).
                        GetComponent<ParticleSystem>();
                    ps.Play(true);
                    GameObject.Destroy(ps.gameObject, ps.main.duration);
                }
            }
        }
        protected void OnStartLoop(BuffInstance instance) {
            if (startLoopParticle != null) {
                ParticleSystem ps = GameObject.Instantiate(startLoopParticle,
                instance.target.transform).
                GetComponent<ParticleSystem>();
                ps.transform.localPosition = startLoopOffset;
                ps.Play(true);
                instance.startLoopParticle = ps.gameObject;
            }
        }
        protected void OnPeriodBurst(BuffInstance instance) {
            if (periodBurstParticle != null) {
                if (periodBurstAttach) {
                    ParticleSystem ps = GameObject.Instantiate(periodBurstParticle,
                    instance.target.transform).
                    GetComponent<ParticleSystem>();
                    ps.transform.localPosition = periodBurstOffset;
                    ps.Play(true);
                    GameObject.Destroy(ps.gameObject, ps.main.duration);
                } else {
                    ParticleSystem ps = GameObject.Instantiate(periodBurstParticle,
                        instance.target.transform.position + instance.target.transform.rotation * periodBurstOffset,
                        instance.target.transform.rotation).
                        GetComponent<ParticleSystem>();
                    ps.Play(true);
                    GameObject.Destroy(ps.gameObject, ps.main.duration);
                }
            }
        }
        protected void OnEndBurst(BuffInstance instance) {
            if (endBurstParticle != null) {
                if (endBurstAttach) {
                    ParticleSystem ps = GameObject.Instantiate(endBurstParticle,
                    instance.target.transform).
                    GetComponent<ParticleSystem>();
                    ps.transform.localPosition = endBurstOffset;
                    ps.Play(true);
                    GameObject.Destroy(ps.gameObject, ps.main.duration);
                } else {
                    ParticleSystem ps = GameObject.Instantiate(endBurstParticle,
                        instance.target.transform.position + instance.target.transform.rotation * endBurstOffset,
                        instance.target.transform.rotation).
                        GetComponent<ParticleSystem>();
                    ps.Play(true);
                    GameObject.Destroy(ps.gameObject, ps.main.duration);
                }
            }
        }
        #endregion // PARTICLES
    }

    public class BuffInstance {
        public Buff buff;
        public Character origin;
        public Character target;
        public float duration;
        public bool canExpire = false;


        public bool removed;
        public Coroutine period;
        public GameObject startLoopParticle;
        public void Start() {

        }
        public void Stop() {
            if (startLoopParticle != null)
                GameObject.Destroy(startLoopParticle);
            if (period != null)
                target.StopCoroutine(period);
            target.StartCoroutine(buff.OnEnd(this));

            target.RemoveBuff(this);
        }
    }
}
