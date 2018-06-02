using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New ItemArmour", menuName = "Heck/ItemArmour")]
    public class ItemArmour : EquipableItem {

        ItemArmour() : base() {
            isStackable = false;
            itemType = ItemType.Armor;
        }
        

        public void Equip(Character user, ItemInstance item) {

        }

        public void Unequip(Character user, ItemInstance item) {
        }
    }
}
