using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    public abstract class Item : ScriptableObject {

        public string itemName;
        [TextArea]
        public string itemDescription;
        [TextArea]
        public string itemCondition;
        public string itemIconPath = "N/A";
        public ItemType itemType = ItemType.Quest;
        public bool isStackable = false;
    }

    public enum ItemType {
        Consumable, Quest, Weapon, Armor, Ring, Necklace
    }

    [System.Serializable]
    public class ItemInstance {
        public Item item = null;
        public int stackCount = 0;

        public bool use { get { return false; } }
        public bool drop { get { return false; } }
        public bool destroy { get { return false; } }

        public bool isEquiped = false;
        public int equipSlot = -1;

        public string itemName {
            get { return item.itemName; }
        }
        public string itemDescription {
            get { return item.itemDescription; }
        }
        public string itemCondition {
            get { return item.itemCondition; }
        }
        public string itemIconPath {
            get { return item.itemIconPath; }
        }

    }
}
