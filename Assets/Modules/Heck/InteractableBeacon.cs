using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class InteractableBeacon : Interactable {

        public BeaconMenu beaconMenu;
        public bool isActive = false;

        public BeaconType type;
        public Area area;

        private void Start() {
            if (isActive)
                Lit();
            else
                Unlit();
        }

        public void Lit() {
            isActive = true;
            foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
                p.Play();
            beacons.Add(this);
            interactionText = "Kneel";
        }

        public void Unlit() {
            isActive = false;
            foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
                p.Stop();
            beacons.Remove(this);
            interactionText = "Lit";
        }

        public override void Interact(Character interactor) {
            if (isActive) {
                ShowMenu(interactor);
            } else {
                Lit();
            }
        }

        public void ShowMenu(Character interactor) {
            Menu.Open(beaconMenu);
            beaconMenu.SetUser(interactor);
            beaconMenu.SetCurrentBeacon(this);
        }

        public void Teleport(Character obj) {
            obj.transform.position = interactionArea.transform.position
                + interactionArea.transform.forward * interactionArea.center.magnitude;
            obj.transform.forward = -interactionArea.transform.forward;
        }


        public static HashSet<InteractableBeacon> beacons = new HashSet<InteractableBeacon>();

        public static bool IsBeaconActive(InteractableBeacon beacon) {
            return beacons.Contains(beacon);
        }

    }



    public enum BeaconType {
        Dark, Light
    }

    public enum Area {
        First, Second
    }

}
