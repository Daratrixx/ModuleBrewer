using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class MissileInstance : MonoBehaviour {

        public MissileData data;

        //public Animator animatorController;
        public AudioSource audioSource;
        public Character owner;
        public Transform target = null;

        public float currentTime;
        public Vector3 currentSpeed;
        private Vector3 lastPosition;

        private ICollection<Attackable> hitTargets = new List<Attackable>();

        private void Start() {
            currentSpeed = transform.rotation * data.initialSpeed;
            DoStart();
        }

        private void Update() {
            if (UpdateTime()) {
                DoEnd();
            } else {
                UpdateSpeed();
                UpdateDirection();
                UpdatePosition();
                if (TryHit() && data.endOnHit)
                    DoEnd();
            }
        }

        private bool UpdateTime() {
            currentTime += Time.deltaTime;
            return currentTime >= data.duration;
        }
        private void UpdateSpeed() {
            if (data.useGravity)
                currentSpeed.y -= 10 * data.gravityFactor * Time.deltaTime;
        }
        private void UpdateDirection() {
            if (data.followTarget && target != null) {
                currentSpeed = Vector3.Slerp((currentSpeed).normalized,
                    (target.position - transform.position).normalized,
                    data.followRotationAdjustementRate * Time.deltaTime).normalized * currentSpeed.magnitude;
            }
        }
        private void UpdatePosition() {
            lastPosition = transform.position;
            transform.position += currentSpeed * Time.deltaTime;
        }
        private bool TryHit() {
            switch (data.hitMethod) {
                case CollisionType.Capsule:
                    return DoHit(DamagePattern.TargetsInCapsule(lastPosition, transform.position, data.hitSize));
                case CollisionType.Sphere:
                    return DoHit(DamagePattern.TargetsInSphere(transform.position, data.hitSize));
                case CollisionType.Cube:
                    return DoHit(DamagePattern.TargetsInCube(transform.position, data.hitSize));
                default:
                    return false;
            }
        }

        #region PATTERNS

        private void DoStart() {
            foreach (Pattern p in data.onStart)
                DoPattern(p);
        }

        private bool DoHit(ICollection<Collider> targets) {
            bool atLeastOneTarget = false;
            foreach (Collider c in targets) {
                Attackable a = c.GetComponent<Attackable>();
                if (a != null && a != (Attackable)owner && !hitTargets.Contains(a.GetAttackableRoot())) {
                    DoHit(a);
                    hitTargets.Add(a.GetAttackableRoot());
                    atLeastOneTarget = true;
                }
            }
            return atLeastOneTarget;
        }
        private void DoHit(Attackable target) {
            foreach (MissileEffect e in data.onHit) {
                switch (e.type) {
                    case MissileEffectType.Buff:
                        if (target is Character) {
                            Character c = (Character)target;
                            c.AddBuff(new BuffInstance() { buff = e.buffData, duration = e.buffDuration, origin = owner, target = c, canExpire = true });
                        }
                        break;
                    case MissileEffectType.Damage:
                        target.Damage(e.damageAmount, e.damageType);
                        break;
                    case MissileEffectType.ParticleBurst:
                        break;
                    default:
                        break;
                }
            }
        }

        private void DoExplode() {
            foreach (Pattern p in data.onExplode)
                DoPattern(p);
        }

        private void DoEnd() {
            if (data.explodeOnEnd)
                DoExplode();
            foreach (Pattern p in data.onEnd)
                DoPattern(p);
            Destroy(gameObject);
        }

        private void DoPattern(Pattern p, Attackable target = null) {
            p.DoPattern(owner, transform, null);
        }

        #endregion

    }
}
