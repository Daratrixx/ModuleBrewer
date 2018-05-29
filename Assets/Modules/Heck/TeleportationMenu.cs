using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Heck {

    public class TeleportationMenu : Menu {

        public GameObject buttonPrefab;

        public Button[] actions;
        public int actionIndex = 0;

        public InteractableBeacon currentBeacon;

        public GameObject layout;

        public void SetCurrentBeacon(InteractableBeacon currentBeacon) {
            this.currentBeacon = currentBeacon;
        }
        public void BuildActionList(ICollection<InteractableBeacon> beacons) {
            actions = new Button[beacons.Contains(currentBeacon) ? beacons.Count - 1 : beacons.Count];
            actionIndex = 0;
            foreach (InteractableBeacon beacon in beacons) {
                if (beacon != currentBeacon) {
                    GameObject button = Instantiate(buttonPrefab, layout.transform);
                    (actions[actionIndex] = button.GetComponent<Button>()).onClick.AddListener(delegate () {
                        Close(this);
                        beacon.Teleport(user);
                        if(user.controller != null) {
                            user.controller.ResetCamera();
                        }
                    });
                    GameObject text = button.transform.GetChild(0).gameObject;
                    text.GetComponent<Text>().text = "" + beacon.type + "(" + beacon.area + ")";
                    actionIndex++;
                }
            }
            actionIndex = 0;
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
            if (actions.Length > 0)
                actions[actionIndex].Select();
        }

        public bool CanShow() {
            return actions.Length > 0;
        }

        public override void OnHide() {
            foreach (Button button in actions) {
                button.transform.SetParent(null);
                Destroy(button);
            }
            currentBeacon = null;
        }
    }

}
