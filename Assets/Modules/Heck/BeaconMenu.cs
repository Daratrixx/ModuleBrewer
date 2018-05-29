using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Heck {

    public class BeaconMenu : Menu {

        public GameObject buttonPrefab;

        public Button[] actions;
        public int actionIndex = 0;

        public InteractableBeacon currentBeacon;

        public TeleportationMenu teleportationMenu;

        public void SetCurrentBeacon(InteractableBeacon currentBeacon) {
            this.currentBeacon = currentBeacon;
        }

        public void Unlit() {
            currentBeacon.Unlit();
            Close(this);
        }

        public void OpenTeleportationMenu() {
            teleportationMenu.SetUser(user);
            teleportationMenu.SetCurrentBeacon(currentBeacon);
            teleportationMenu.BuildActionList(InteractableBeacon.beacons);
            if(teleportationMenu.CanShow()) {
                Close(this);
                Open(teleportationMenu);
            }
        }

        public override void InputAction() {
            ExecuteEvents.Execute(actions[actionIndex].gameObject, baseEventData, ExecuteEvents.submitHandler);
        }
        public override void InputAlt() { }
        public override void InputBack() {
            Menu.Close(this);
        }

        public override void InputUp() {
            actionIndex--;
            if (actionIndex < 0)
                actionIndex = actions.Length - 1;
            actions[actionIndex].Select();
        }
        public override void InputDown() {
            actionIndex++;
            if (actionIndex == actions.Length)
                actionIndex = 0;
            actions[actionIndex].Select();
        }
        public override void InputLeft() { }
        public override void InputRight() { }

        public override void OnShow() {
            actionIndex = 0;
            if (actions.Length > 0)
                actions[actionIndex].Select();
        }
        public override void OnHide() {
            currentBeacon = null;
        }
    }

}
