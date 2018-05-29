using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [CreateAssetMenu(fileName = "New ItemConsumable", menuName = "Heck/ItemConsumable")]
    public class ItemConsumable : Item {
        ItemConsumable() : base() {
            isStackable = true;
            itemType = ItemType.Consumable;
        }

        public bool destoyOnConsume = true;
        public Spell onConsume;

        public void Consume(Character user) {
            if (onConsume != null) {
                user.StartCoroutine(onConsume.OnTriggerEffects(user, null));
            }
        }

        public void Equip(Character user, ItemInstance item, int slot) {

        }

        public void Unequip(Character user, ItemInstance item) {

        }

    }
}
