using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    
    public abstract class Pattern : ScriptableObject {
        public abstract bool CanCancel();
        public abstract IEnumerator DoPattern(Character owner, Transform origin, MoveInteraction interaction = null);
    }

}

