using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heck {
    [System.Serializable]
    public class Inventory {

        Character owner;

        // equiped
        public ItemInstance[] weapons = new ItemInstance[] { null, null, null, null };
        public ItemInstance armour;
        public ItemInstance necklace;
        public ItemInstance[] rings = new ItemInstance[] { null, null };
        public ItemInstance[] consumables = new ItemInstance[] { null, null, null, null, null };


        // bag
        public List<ItemInstance> itemsAll = new List<ItemInstance>();
        public List<ItemInstance> itemsWeapons = new List<ItemInstance>();
        public List<ItemInstance> itemsArmors = new List<ItemInstance>();
        public List<ItemInstance> itemsConsumables = new List<ItemInstance>();
        public List<ItemInstance> itemsRings = new List<ItemInstance>();
        public List<ItemInstance> itemsNecklaces = new List<ItemInstance>();
        public List<ItemInstance> itemsQuests = new List<ItemInstance>();


        public void EquipWeapon(ItemInstance w, int slot) {
            if (weapons[slot] != null) {
                if (w == weapons[slot]) { // equip same => unequip
                    weapons[slot].equipSlot = -1;
                    weapons[slot].isEquiped = false;
                    weapons[slot] = null;
                    return;
                }
                if (w != null && w.isEquiped && w.equipSlot != slot) { // swap
                    weapons[slot].equipSlot = w.equipSlot;
                    weapons[w.equipSlot] = weapons[slot];
                    weapons[slot] = w;
                    weapons[slot].equipSlot = slot;
                    return;
                } else { // remove
                    weapons[slot].equipSlot = -1;
                    weapons[slot].isEquiped = false;
                    weapons[slot] = null;
                    // trigger buff and stat mod
                }
            }
            if (w != null) {
                w.equipSlot = slot;
                w.isEquiped = true;
                weapons[slot] = w;
                // trigger buff and stat mod
            }
        }
        public void EquipArmour(ItemInstance a) {
            if (armour != null) {
                if(armour == a) {
                    armour.equipSlot = -1;
                    armour.isEquiped = false;
                    armour = null;
                    return;
                }
                armour.equipSlot = -1;
                armour.isEquiped = false;
                armour = null;
                // trigger buff and stat mod
            }
            if (a != null) {
                a.equipSlot = 0;
                a.isEquiped = true;
                armour = a;
                // trigger buff and stat mod
            }
        }
        public void EquipNecklace(ItemInstance n) {
            if (necklace != null) {
                if (necklace == n) {
                    necklace.equipSlot = -1;
                    necklace.isEquiped = false;
                    necklace = null;
                    return;
                }
                necklace.equipSlot = -1;
                necklace.isEquiped = false;
                necklace = null;
                // trigger buff and stat mod
            }
            if (n != null) {
                n.equipSlot = 0;
                n.isEquiped = true;
                necklace = n;
                // trigger buff and stat mod
            }
        }
        public void EquipRing(ItemInstance r, int slot) {
            if (rings[slot] != null) {
                if(rings[slot] == r) {
                    rings[slot].equipSlot = -1;
                    rings[slot].isEquiped = false;
                    rings[slot] = null;
                    return;
                }
                if (r != null && r.isEquiped && r.equipSlot != slot) { // swap
                    rings[slot].equipSlot = r.equipSlot;
                    rings[r.equipSlot] = rings[slot];
                    rings[slot] = r;
                    rings[slot].equipSlot = slot;
                    return;
                } else { // remove
                    rings[slot].equipSlot = -1;
                    rings[slot].isEquiped = false;
                    rings[slot] = null;
                    // trigger buff and stat mod
                }
            }
            if (r != null) {
                r.equipSlot = slot;
                r.isEquiped = true;
                rings[slot] = r;
                // trigger buff and stat mod
            }
        }
        public void EquipConsumable(ItemInstance c, int slot) {
            if (consumables[slot] != null) {
                if (c == consumables[slot]) { // equip same => unequip
                    consumables[slot].equipSlot = -1;
                    consumables[slot].isEquiped = false;
                    consumables[slot] = null;
                    return;
                }
                if (c != null && c.isEquiped && c.equipSlot != slot) { // swap
                    consumables[slot].equipSlot = c.equipSlot;
                    consumables[c.equipSlot] = consumables[slot];
                    consumables[slot] = c;
                    consumables[slot].equipSlot = slot;
                    return;
                } else { // remove
                    consumables[slot].equipSlot = -1;
                    consumables[slot].isEquiped = false;
                    consumables[slot] = null;
                }
            }
            if (c != null) {
                c.equipSlot = slot;
                c.isEquiped = true;
                consumables[slot] = c;
            }
        }


        public void AddItem(Item item, int count = 1) {
            ItemInstance i = null;
            if (item.isStackable)
                i = itemsAll.Where(x => x.item == item).DefaultIfEmpty(null).First();
            if (i == null) {
                i = new ItemInstance() { item = item };
                itemsAll.Add(i);
                switch (item.itemType) {
                    case ItemType.Armor:
                        itemsArmors.Add(i);
                        break;
                    case ItemType.Consumable:
                        itemsConsumables.Add(i);
                        break;
                    case ItemType.Quest:
                        itemsQuests.Add(i);
                        break;
                    case ItemType.Ring:
                        itemsRings.Add(i);
                        break;
                    case ItemType.Necklace:
                        itemsNecklaces.Add(i);
                        break;
                    case ItemType.Weapon:
                        itemsWeapons.Add(i);
                        break;
                    default: break;
                }
            }
            i.stackCount += count;
        }
        public void RemoveItem(Item item, int count = 1) {
            ItemInstance i = itemsAll.Where(x => x.item == item).DefaultIfEmpty(null).First();
            if (i != null) {
                i.stackCount -= count;
                if (i.stackCount == 0) {
                    itemsAll.Remove(i);
                    switch (item.itemType) {
                        case ItemType.Armor:
                            itemsArmors.Remove(i);
                            break;
                        case ItemType.Consumable:
                            itemsConsumables.Remove(i);
                            break;
                        case ItemType.Quest:
                            itemsQuests.Remove(i);
                            break;
                        case ItemType.Ring:
                            itemsRings.Remove(i);
                            break;
                        case ItemType.Necklace:
                            itemsNecklaces.Remove(i);
                            break;
                        case ItemType.Weapon:
                            itemsWeapons.Remove(i);
                            break;
                        default: break;
                    }
                }
            }
        }

    }
}
