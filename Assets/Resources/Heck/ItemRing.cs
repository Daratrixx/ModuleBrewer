using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New ItemRing", menuName = "Heck/ItemRing")]
    public class ItemRing : EquipableItem {

        ItemRing() : base() {
            isStackable = false;
            itemType = ItemType.Ring;
        }

        public void Equip(Character user, ItemInstance item) {

        }

        public void Unequip(Character user, ItemInstance item) {

        }
    }
}
