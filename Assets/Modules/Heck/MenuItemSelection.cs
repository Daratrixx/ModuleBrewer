using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Heck {

    public class MenuItemSelection : Menu {

        public GameObject[] buttons = new GameObject[0];
        public Transform layout;
        public GameObject buttonPrefab;
        public Text title;

        protected int currentIndex = 0;

        public void BuildList(Inventory inv, ICollection<ItemInstance> items, ItemMenuSlot itemSlot, int slot = 0) {
            ClearList();
            if (items.Count == 0) return;
            int i = 0;
            buttons = new GameObject[items.Count];
            GameObject currentEquipedItem = null;
            foreach (ItemInstance item in items) {
                GameObject go = Instantiate(buttonPrefab, layout);
                Text[] texts = go.GetComponentsInChildren<Text>();
                texts[0].text = item.itemName;
                texts[1].text = item.itemResumed;
                texts[2].text = item.itemCondition;
                texts[1].enabled = false;
                Image image = go.GetComponentsInChildren<Image>()[1];
                //image.sprite = Resources.Load<Sprite>(item.itemIconPath);
                ResourceRequest req = Resources.LoadAsync<Sprite>(item.itemIconPath);
                req.completed += delegate {
                    if (req.asset != null)
                        image.sprite = (Sprite)req.asset;
                };
                Button button = go.GetComponent<Button>();
                button.onClick.AddListener(delegate { OnItemSelected(inv, item, itemSlot, slot); });
                if (item.equipSlot == slot) {
                    currentIndex = i;
                    currentEquipedItem = go;
                }
                buttons[i] = go;
                ++i;
            }
            if (currentEquipedItem == null) {
                currentIndex = 0;
                currentEquipedItem = buttons[0];
            }
            switch (itemSlot) {
                case ItemMenuSlot.WR1: title.text = "Select Weapon Right 1"; break;
                case ItemMenuSlot.WR2: title.text = "Select Weapon Right 2"; break;
                case ItemMenuSlot.WL1: title.text = "Select Weapon Left 1"; break;
                case ItemMenuSlot.WL2: title.text = "Select Weapon Left 2"; break;
                case ItemMenuSlot.AR: title.text = "Select Armour"; break;
                case ItemMenuSlot.NC: title.text = "Select Necklace"; break;
                case ItemMenuSlot.R1: title.text = "Select Ring 1"; break;
                case ItemMenuSlot.R2: title.text = "Select Ring 2"; break;
                case ItemMenuSlot.I1: title.text = "Select Item 1"; break;
                case ItemMenuSlot.I2: title.text = "Select Item 2"; break;
                case ItemMenuSlot.I3: title.text = "Select Item 3"; break;
                case ItemMenuSlot.I4: title.text = "Select Item 4"; break;
                case ItemMenuSlot.I5: title.text = "Select Item 5"; break;
                default: title.text = "ERROR - unknown ItemMenuSlot"; break;
            }
            Open();
        }

        public override void InputAlt() {
            foreach (GameObject b in buttons) {
                Text[] texts = b.GetComponentsInChildren<Text>();
                texts[0].enabled = !texts[0].enabled;
                texts[1].enabled = !texts[1].enabled;
                texts[2].enabled = !texts[2].enabled;
            }
        }

        protected void ClearList() {
            layout.DetachChildren();
            foreach (GameObject button in buttons) {
                Destroy(button);
            }
        }


        public void OnItemSelected(Inventory inv, ItemInstance item, ItemMenuSlot itemSlot, int slot) {
            switch (itemSlot) {
                case ItemMenuSlot.WR1: inv.EquipWeapon(item, slot); break;
                case ItemMenuSlot.WR2: inv.EquipWeapon(item, slot); break;
                case ItemMenuSlot.WL1: inv.EquipWeapon(item, slot); break;
                case ItemMenuSlot.WL2: inv.EquipWeapon(item, slot); break;
                case ItemMenuSlot.AR: inv.EquipArmour(item); break;
                case ItemMenuSlot.NC: inv.EquipNecklace(item); break;
                case ItemMenuSlot.R1: inv.EquipRing(item, slot); break;
                case ItemMenuSlot.R2: inv.EquipRing(item, slot); break;
                case ItemMenuSlot.I1: inv.EquipConsumable(item, slot); break;
                case ItemMenuSlot.I2: inv.EquipConsumable(item, slot); break;
                case ItemMenuSlot.I3: inv.EquipConsumable(item, slot); break;
                case ItemMenuSlot.I4: inv.EquipConsumable(item, slot); break;
                case ItemMenuSlot.I5: inv.EquipConsumable(item, slot); break;
                default: break;
            }
            Close();
        }

    }

}
