using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualRPG {

    public class Character {
        public Character() {
            for(MainAttribute ma = MainAttribute.Constitution; ma < MainAttribute.Charisma; ma++) {
                attributes.SetMainAttributeValue(ma, mainAttributeBaseValue);
            }
            currentHealth = GetMaximumHealth();
            currentEnergy = GetMaximumEnergy();
        }

        //public CharacterInterface characterUI;
        public CharacterAttributes attributes = new CharacterAttributes();

        public string name;

        public int level;

        public int currentHealth;
        public int currentEnergy;

        public Weapon weapon;
        public bool HasWeapon() {
            return weapon != unarmedWeapon;
        }

        public int GetMaximumHealth() {
            return attributes.GetSecondaryAttributeValue(SecondaryAttribute.MaximumHealth) + baseHealth;
        }
        public int GetMaximumEnergy() {
            return attributes.GetSecondaryAttributeValue(SecondaryAttribute.MaximumEnergy) + baseEnergy;
        }

        public Ability attack = Ability.attack;
        public Ability spell1;
        public Ability spell2;
        //public List<Ability> abilities = new List<Ability>();

        public void DoDamage(int damage) {
            if (damage > currentHealth)
                currentHealth = 0;
            else
                currentHealth -= damage;
            Debug.Log(name + " takes " + damage + " damages.");
        }
        public void DoHeal(int heal) {
            if (currentHealth + heal < GetMaximumHealth())
                currentHealth = GetMaximumHealth();
            else
                currentHealth += heal;
        }

        #region BUFFS

        private HashSet<BuffInstance> buffs = new HashSet<BuffInstance>();

        public void AddBuff(Buff buff, Character source = null) {
            buffs.Add(new BuffInstance(buff, source));
            buff.OnGained(source, this);
        }

        public void RemoveBuff(Buff buff) {
            BuffInstance buffInstance = null;
            foreach (BuffInstance instance in buffs) {
                if (instance.buff == buff) {
                    buffInstance = instance;
                    break;
                }
            }
            buffs.Remove(buffInstance);
            buffInstance.buff.OnLost(buffInstance.source, this);
        }

        public bool HasBuff(Buff buff) {
            foreach (BuffInstance instance in buffs)
                if (instance.buff == buff)
                    return true;
            return false;
        }

        #endregion


        private static Weapon unarmedWeapon = Weapon.GetUnarmedWeapon();
        private const int maxLevel = 100;
        private const int mainAttributeBaseValue = 0;
        private const int mainAttributePointAtStart = 50;
        private const int mainAttributeBonusPerLevel = 0;
        private const int mainAttributePointPerLevel = 5;
        private const int baseHealth = 100;
        private const int baseEnergy = 100;

        public static Character GetAkarys() {
            Character output = new Character();
            output.name = "Akarys de Netherid";
            output.level = 10;
            output.attributes.SetMainAttributeValue(MainAttribute.Constitution, 20);
            output.attributes.SetMainAttributeValue(MainAttribute.Stamina, 20);
            output.attributes.SetMainAttributeValue(MainAttribute.Strength, 15);
            output.attributes.SetMainAttributeValue(MainAttribute.Spirit, 15);
            output.attributes.SetMainAttributeValue(MainAttribute.Calm, 15);
            output.attributes.SetMainAttributeValue(MainAttribute.Charisma, 15);
            output.currentHealth = output.GetMaximumHealth();
            output.currentEnergy = output.GetMaximumEnergy();
            output.weapon = Weapon.GetGoldenHope();
            output.spell1 = Ability.fury;
            output.spell2 = Ability.blessing;
            //output.abilities.Add(Ability.fury);
            return output;
        }

        public static Character GetKrynn() {
            Character output = new Character();
            output.name = "Krynn Bloodscale";
            output.level = 10;
            output.attributes.SetMainAttributeValue(MainAttribute.Constitution, 20);
            output.attributes.SetMainAttributeValue(MainAttribute.Stamina, 15);
            output.attributes.SetMainAttributeValue(MainAttribute.Strength, 25);
            output.attributes.SetMainAttributeValue(MainAttribute.Agility, 20);
            output.attributes.SetMainAttributeValue(MainAttribute.Calm, 10);
            output.attributes.SetMainAttributeValue(MainAttribute.Charisma, 10);
            output.currentHealth = output.GetMaximumHealth();
            output.currentEnergy = output.GetMaximumEnergy();
            output.weapon = Weapon.GetSteelSword();
            output.spell1 = Ability.dank;
            output.spell2 = Ability.battleCry;
            return output;
        }
    }
}

