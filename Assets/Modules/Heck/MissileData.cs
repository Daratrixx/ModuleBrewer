using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    [CreateAssetMenu(fileName = "New MissileData", menuName = "Heck/MissileData")]
    public class MissileData : ScriptableObject {
        public GameObject missilePrefab;
        public float duration = 2;

        public bool endOnHit;
        public bool explodeOnEnd;
        public CollisionType hitMethod;
        public float hitSize;

        public Pattern[] onStart; // when the instance is created
        public MissileEffect[] onHit; // first time the instance hit something
        public Pattern[] onExplode; // when the instance explode
        public Pattern[] onEnd; // when the instance is destroyed

        public Vector3 initialSpeed;
        public bool useGravity = false;
        public float gravityFactor = 0;
        public bool followTarget = false;
        public float followRotationAdjustementRate = 1;

    }

    [System.Serializable]
    public struct MissileEffect {
        public MissileEffectType type;

        public float damageAmount;
        public DamageType damageType;

        public Buff buffData;
        public float buffDuration;

        public string particleBurstObjectName;
        ParticleBurstPosition particleBurstPosition;
    }

    public enum MissileEffectType {
        Damage, Buff, ParticleBurst
    }

    public enum ParticleBurstPosition {
        MissileCenter, ImpactPoint, TargetCenter
    }
}

