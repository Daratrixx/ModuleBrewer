using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New ItemWeapon", menuName = "Heck/ItemWeapon")]
    public class ItemWeapon : EquipableItem {

        ItemWeapon() : base() {
            isStackable = false;
            itemType = ItemType.Weapon;
        }

        public Weapon weapon;
        

        public void Show(Character user, ItemInstance item) {
            user.weapon = GameObject.Instantiate(weapon).GetComponent<Weapon>();
        }

        public void Hide(Character user, ItemInstance item) {
            GameObject.Destroy(user.weapon.gameObject);
            user.weapon = null;
        }
    }
}
