using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heck {
    [System.Serializable]
    public class Inventory {

        private Character OWNER = null;
        public Character owner {
            get { return OWNER; }
            set { OWNER = value; }
        }

        // equiped
        public ItemInstance[] weapons = new ItemInstance[] { null, null, null, null };
        public ItemInstance armour = null;
        public ItemInstance necklace = null;
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
            if (w != null && w.item != null) {
                if (weapons[slot] == null || weapons[slot].item == null) { // equip
                    if (w.isEquiped)
                        weapons[w.equipSlot] = null;
                    else
                        ApplyItemEffect((EquipableItem)w.item);
                    w.equipSlot = slot;
                    w.isEquiped = true;
                    weapons[slot] = w;
                } else if (w != weapons[slot]) { // swap
                    if (w.isEquiped) {
                        weapons[slot].equipSlot = w.equipSlot;
                        weapons[w.equipSlot] = weapons[slot];
                    } else { // regular equip, kick old item
                        RemoveItemEffect((EquipableItem)weapons[slot].item);
                        weapons[slot].equipSlot = -1;
                        weapons[slot].isEquiped = false;
                        w.isEquiped = true;
                        ApplyItemEffect((EquipableItem)w.item);
                    }
                    weapons[slot] = w;
                    weapons[slot].equipSlot = slot;
                } else { // unequip
                    RemoveItemEffect((EquipableItem)weapons[slot].item);
                    weapons[slot].equipSlot = -1;
                    weapons[slot].isEquiped = false;
                    weapons[slot] = null;
                }
            } else if (weapons[slot] != null && weapons[slot].item != null) { // unequip
                RemoveItemEffect((EquipableItem)weapons[slot].item);
                weapons[slot].equipSlot = -1;
                weapons[slot].isEquiped = false;
                weapons[slot] = null;
            }
        }
        public void EquipArmour(ItemInstance a) {
            if (a != null && a.item != null) {
                if (armour == null || armour.item == null) { // equip
                    a.equipSlot = 0;
                    a.isEquiped = true;
                    armour = a;
                    ApplyItemEffect((EquipableItem)a.item);
                } else if (a != armour) {
                    RemoveItemEffect((EquipableItem)armour.item);
                    armour.equipSlot = -1;
                    armour.isEquiped = false;
                    armour = a;
                    armour.equipSlot = 0;
                    armour.isEquiped = true;
                    ApplyItemEffect((EquipableItem)a.item);
                } else { // unequip
                    RemoveItemEffect((EquipableItem)armour.item);
                    armour.equipSlot = -1;
                    armour.isEquiped = false;
                    armour = null;
                }
            } else if (armour != null && armour.item != null) { // unequip
                RemoveItemEffect((EquipableItem)armour.item);
                armour.equipSlot = -1;
                armour.isEquiped = false;
                armour = null;
            }
        }
        public void EquipNecklace(ItemInstance n) {
            if (n != null && n.item != null) {
                if (necklace == null || necklace.item == null) { // equip
                    n.equipSlot = 0;
                    n.isEquiped = true;
                    necklace = n;
                    ApplyItemEffect((EquipableItem)n.item);
                } else if (n != necklace) {
                    RemoveItemEffect((EquipableItem)necklace.item);
                    necklace.equipSlot = -1;
                    necklace.isEquiped = false;
                    necklace = n;
                    necklace.equipSlot = 0;
                    necklace.isEquiped = true;
                    ApplyItemEffect((EquipableItem)n.item);
                } else { // unequip
                    RemoveItemEffect((EquipableItem)necklace.item);
                    necklace.equipSlot = -1;
                    necklace.isEquiped = false;
                    necklace = null;
                }
            } else if (necklace != null && necklace.item != null) { // unequip
                RemoveItemEffect((EquipableItem)necklace.item);
                necklace.equipSlot = -1;
                necklace.isEquiped = false;
                necklace = null;
            }
        }
        public void EquipRing(ItemInstance r, int slot) {
            if (r != null && r.item != null) {
                if (rings[slot] == null || rings[slot].item == null) { // equip
                    if (r.isEquiped)
                        rings[r.equipSlot] = null;
                    else
                        ApplyItemEffect((EquipableItem)r.item);
                    r.equipSlot = slot;
                    r.isEquiped = true;
                    rings[slot] = r;
                } else if (r != rings[slot]) { // swap
                    if (r.isEquiped) {
                        rings[slot].equipSlot = r.equipSlot;
                        rings[r.equipSlot] = rings[slot];
                    } else { // regular equip, kick old item
                        RemoveItemEffect((EquipableItem)rings[slot].item);
                        rings[slot].equipSlot = -1;
                        rings[slot].isEquiped = false;
                        r.isEquiped = true;
                        ApplyItemEffect((EquipableItem)r.item);
                    }
                    rings[slot] = r;
                    rings[slot].equipSlot = slot;
                } else { // unequip
                    RemoveItemEffect((EquipableItem)rings[slot].item);
                    rings[slot].equipSlot = -1;
                    rings[slot].isEquiped = false;
                    rings[slot] = null;
                }
            } else if (rings[slot] != null && rings[slot].item != null) { // unequip
                RemoveItemEffect((EquipableItem)rings[slot].item);
                rings[slot].equipSlot = -1;
                rings[slot].isEquiped = false;
                rings[slot] = null;
            }
        }
        public void EquipConsumable(ItemInstance c, int slot) {
            if (c != null && c.item != null) {
                if (consumables[slot] == null || consumables[slot].item == null) { // equip
                    if (c.isEquiped)
                        consumables[c.equipSlot] = null;
                    c.equipSlot = slot;
                    c.isEquiped = true;
                    consumables[slot] = c;
                } else if (c != consumables[slot]) { // swap
                    if (c.isEquiped) {
                        consumables[slot].equipSlot = c.equipSlot;
                        consumables[c.equipSlot] = consumables[slot];
                    } else { // regular equip, kick old item
                        consumables[slot].equipSlot = -1;
                        consumables[slot].isEquiped = false;
                        c.isEquiped = true;
                    }
                    consumables[slot] = c;
                    consumables[slot].equipSlot = slot;
                } else { // unequip
                    consumables[slot].equipSlot = -1;
                    consumables[slot].isEquiped = false;
                    consumables[slot] = null;
                }
            } else if (consumables[slot] != null && consumables[slot].item != null) { // unequip
                consumables[slot].equipSlot = -1;
                consumables[slot].isEquiped = false;
                consumables[slot] = null;
            }
        }

        private void ApplyItemEffect(EquipableItem item) {
            if (item == null) return;
            //foreach (Buff buff in item.buffs)
            //    owner.AddBuff(new BuffInstance() { buff = buff, duration = -1, origin = owner, target = owner, canExpire = false });
            if (item.stats != null)
                OWNER.UpdateStatistics(item.stats, 1);
        }

        private void RemoveItemEffect(EquipableItem item) {
            if (item == null) return;
            if (item.stats != null)
                OWNER.UpdateStatistics(item.stats, -1);
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
                if (i.stackCount <= 0) {
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
