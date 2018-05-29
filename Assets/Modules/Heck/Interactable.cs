using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {


    public abstract class Interactable : MonoBehaviour, System.IComparable {

        public BoxCollider interactionArea;
        public int priority = 0;
        public string interactionText;
        public float interactionDuration;

        public string GetInteractText() {
            return interactionText;
        }

        public abstract void Interact(Character interactor);


        public static Interactable operator <(Interactable a, Interactable b) {
            if (a.priority < b.priority)
                return a;
            return b;
        }
        public static Interactable operator >(Interactable a, Interactable b) {
            if (a.priority > b.priority)
                return a;
            return b;
        }

        public int CompareTo(object obj) {
            if (!(obj is Interactable))
                return 0;
            Interactable b = (Interactable)obj;
            if (this.priority < b.priority)
                return -1;
            if (this.priority > b.priority)
                return 1;
            return 0;
        }

        public float Dot(Vector3 pos) {
            return Vector3.Dot(transform.forward, (pos - transform.position).normalized);
        }

    }
}
