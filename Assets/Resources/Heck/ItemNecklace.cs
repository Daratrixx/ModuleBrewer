using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New ItemNecklace", menuName = "Heck/ItemNecklace")]
    public class ItemNecklace : Item {

        ItemNecklace() : base() {
            isStackable = false;
            itemType = ItemType.Necklace;
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
