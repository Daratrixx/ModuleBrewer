using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New ItemArmour", menuName = "Heck/ItemArmour")]
    public class ItemArmour : Item {

        ItemArmour() : base() {
            isStackable = false;
            itemType = ItemType.Armor;
        }
        
        public Buff[] buffs = new Buff[0];

        public void Equip(Character user, ItemInstance item) {

        }

        public void Unequip(Character user, ItemInstance item) {
            GameObject.Destroy(user.weapon.gameObject);
            user.weapon = null;
        }
    }
}
