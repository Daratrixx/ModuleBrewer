using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Heck {

    public class MenuEquipement : Menu {

        public MenuItemSelection menuItemSelection;
        public Character character;
        protected Inventory inventory {
            get { return character.inventory; }
        }

        #region HANDLERS
        public void SelectWR1() {
            OpenSelectionMenu(ItemMenuSlot.WR1);
        }
        public void SelectWR2() {
            OpenSelectionMenu(ItemMenuSlot.WR2);
        }
        public void SelectWL1() {
            OpenSelectionMenu(ItemMenuSlot.WL1);
        }
        public void SelectWL2() {
            OpenSelectionMenu(ItemMenuSlot.WL2);
        }
        public void SelectAR() {
            OpenSelectionMenu(ItemMenuSlot.AR);
        }
        public void SelectNC() {
            OpenSelectionMenu(ItemMenuSlot.NC);
        }
        public void SelectR1() {
            OpenSelectionMenu(ItemMenuSlot.R1);
        }
        public void SelectR2() {
            OpenSelectionMenu(ItemMenuSlot.R2);
        }
        public void SelectI1() {
            OpenSelectionMenu(ItemMenuSlot.I1);
        }
        public void SelectI2() {
            OpenSelectionMenu(ItemMenuSlot.I2);
        }
        public void SelectI3() {
            OpenSelectionMenu(ItemMenuSlot.I3);
        }
        public void SelectI4() {
            OpenSelectionMenu(ItemMenuSlot.I4);
        }
        public void SelectI5() {
            OpenSelectionMenu(ItemMenuSlot.I5);
        }
        #endregion // HANDLERS

        public void OpenSelectionMenu(ItemMenuSlot slot) {
            switch (slot) {
                case ItemMenuSlot.WR1: menuItemSelection.BuildList(inventory, inventory.itemsWeapons, slot, 0); break;
                case ItemMenuSlot.WR2: menuItemSelection.BuildList(inventory, inventory.itemsWeapons, slot, 1); break;
                case ItemMenuSlot.WL1: menuItemSelection.BuildList(inventory, inventory.itemsWeapons, slot, 2); break;
                case ItemMenuSlot.WL2: menuItemSelection.BuildList(inventory, inventory.itemsWeapons, slot, 3); break;
                case ItemMenuSlot.AR: menuItemSelection.BuildList(inventory, inventory.itemsArmors, slot, 0); break;
                case ItemMenuSlot.NC: menuItemSelection.BuildList(inventory, inventory.itemsNecklaces, slot, 0); break;
                case ItemMenuSlot.R1: menuItemSelection.BuildList(inventory, inventory.itemsRings, slot, 0); break;
                case ItemMenuSlot.R2: menuItemSelection.BuildList(inventory, inventory.itemsRings, slot, 1); break;
                case ItemMenuSlot.I1: menuItemSelection.BuildList(inventory, inventory.itemsConsumables, slot, 0); break;
                case ItemMenuSlot.I2: menuItemSelection.BuildList(inventory, inventory.itemsConsumables, slot, 1); break;
                case ItemMenuSlot.I3: menuItemSelection.BuildList(inventory, inventory.itemsConsumables, slot, 2); break;
                case ItemMenuSlot.I4: menuItemSelection.BuildList(inventory, inventory.itemsConsumables, slot, 3); break;
                case ItemMenuSlot.I5: menuItemSelection.BuildList(inventory, inventory.itemsConsumables, slot, 4); break;
                default: break;
            }
        }

        public override void RecoverFocus() {
            for (int i = 0; i < 4; ++i) {
                if (inventory.weapons[i] != null && inventory.weapons[i].item != null) {
                    navigationLayout[i].element.GetComponentsInChildren<Image>()[1].enabled = true;
                    navigationLayout[i].element.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(inventory.weapons[i].itemIconPath);
                } else navigationLayout[i].element.GetComponentsInChildren<Image>()[1].enabled = false;
            }
            if (inventory.armour != null && inventory.armour.item != null) {
                navigationLayout[4].element.GetComponentsInChildren<Image>()[1].enabled = true;
                navigationLayout[4].element.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(inventory.armour.itemIconPath);
            } else navigationLayout[4].element.GetComponentsInChildren<Image>()[1].enabled = false;
            if (inventory.necklace != null && inventory.necklace.item != null) {
                navigationLayout[5].element.GetComponentsInChildren<Image>()[1].enabled = true;
                navigationLayout[5].element.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(inventory.necklace.itemIconPath);
            } else navigationLayout[5].element.GetComponentsInChildren<Image>()[1].enabled = false;
            for (int i = 0; i < 2; ++i) {
                if (inventory.rings[i] != null && inventory.rings[i].item != null) {
                    navigationLayout[i + 6].element.GetComponentsInChildren<Image>()[1].enabled = true;
                    navigationLayout[i + 6].element.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(inventory.rings[i].itemIconPath);
                } else navigationLayout[i + 6].element.GetComponentsInChildren<Image>()[1].enabled = false;
            }
            for (int i = 0; i < 5; ++i) {
                if (inventory.consumables[i] != null && inventory.consumables[i].item != null) {
                    navigationLayout[i + 8].element.GetComponentsInChildren<Image>()[1].enabled = true;
                    navigationLayout[i + 8].element.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(inventory.consumables[i].itemIconPath);
                } else navigationLayout[i + 8].element.GetComponentsInChildren<Image>()[1].enabled = false;
            }

        }
    }


    public enum ItemMenuSlot {
        WR1, WR2, WL1, WL2, AR, NC, R1, R2, I1, I2, I3, I4, I5
    }

}
