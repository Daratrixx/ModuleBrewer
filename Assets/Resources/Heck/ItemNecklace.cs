using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New ItemNecklace", menuName = "Heck/ItemNecklace")]
    public class ItemNecklace : EquipableItem {

        ItemNecklace() : base() {
            isStackable = false;
            itemType = ItemType.Necklace;
        }

        public void Equip(Character user, ItemInstance item) {

        }

        public void Unequip(Character user, ItemInstance item) {

        }
    }
}
