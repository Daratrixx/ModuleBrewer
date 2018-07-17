using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Heck {
    public class ConsumableDisplayer : MonoBehaviour {

        // Use this for initialization
        void Start() {
            if (character != null && inventory == null)
                inventory = character.inventory;
        }

        public Character character;

        public Image currentConsumable = null;

        private Inventory inventory;
        private int currentSlot;
        private ICollection<ItemInstance> consumables = new ItemInstance[0];

        public void UpdateConsumableList() {
            consumables = inventory.consumables.Where(x => x != null).ToList();
            UpdateConsumableList();
        }

        public void SwitchConsumable() {
            currentSlot++;
            UpdateDisplay();
        }

        public void UpdateDisplay() {
            if (currentSlot > consumables.Count) currentSlot = 0;

            if(consumables.Count > currentSlot) {
                ResourceRequest req = Resources.LoadAsync<Sprite>(consumables.ElementAt(currentSlot).itemIconPath);
                req.completed += delegate {
                    if (req.asset != null)
                        currentConsumable.sprite = (Sprite)req.asset;
                };
                currentConsumable.enabled = true;
            } else {
                currentConsumable.enabled = false;
            }
        }
    }
}
