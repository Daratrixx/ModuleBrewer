using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace Heck {

    [System.Serializable]
    public struct DamageElement {
        public int damageAmount;
        public DamageType damageType;
        public BoxCollider collider;
    }


    public enum DamageType {
        Impact,
        Cut,
        Pierce,
        Arcan,
        Dark,
        Holy,
        Fire,
        Lightning,
        Ice,
        Poison
    }
}
