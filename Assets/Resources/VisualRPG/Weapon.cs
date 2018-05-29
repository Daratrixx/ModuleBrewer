using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualRPG {

    public class Weapon {

        public Weapon(string name = "undefined") {
            this.name = name;
        }

        public string name;

        public Attribute physicalDamage = new Attribute();
        public Attribute lightDamage = new Attribute();
        public Attribute arcanDamage = new Attribute();
        public Attribute mentalDamage = new Attribute();
        public Attribute moralDamage = new Attribute();

        public void AddBuff(Buff buff, Character source = null) {
            buffs.Add(new BuffInstance(buff, source));
            buff.OnGained(source, this);
        }

        public bool HasBuff(Buff buff) {
            foreach (BuffInstance instance in buffs)
                if (instance.buff == buff)
                    return true;
            return false;
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

        private HashSet<BuffInstance> buffs = new HashSet<BuffInstance>();

        public int GetDamage(Character user, Character target) {
            int totalDamage = 0;
            totalDamage += Fight.DamageCalculation(physicalDamage.GetValue(),
                user.attributes.GetSecondaryAttributeValue(SecondaryAttribute.PhysicalDamage),
                target.attributes.GetSecondaryAttributeValue(SecondaryAttribute.PhysicalResistance));
            totalDamage += Fight.DamageCalculation(lightDamage.GetValue(),
                user.attributes.GetSecondaryAttributeValue(SecondaryAttribute.LightDamage),
                target.attributes.GetSecondaryAttributeValue(SecondaryAttribute.LightResistance));
            totalDamage += Fight.DamageCalculation(arcanDamage.GetValue(),
                user.attributes.GetSecondaryAttributeValue(SecondaryAttribute.ArcanDamage),
                target.attributes.GetSecondaryAttributeValue(SecondaryAttribute.ArcanResistance));
            totalDamage += Fight.DamageCalculation(mentalDamage.GetValue(),
                user.attributes.GetSecondaryAttributeValue(SecondaryAttribute.MentalDamage),
                target.attributes.GetSecondaryAttributeValue(SecondaryAttribute.MentalResistance));
            totalDamage += Fight.DamageCalculation(moralDamage.GetValue(),
                user.attributes.GetSecondaryAttributeValue(SecondaryAttribute.MoralDamage),
                target.attributes.GetSecondaryAttributeValue(SecondaryAttribute.MoralResistance));
            //return "Using " + name + ", " + user.name + " deals a total of " + 0 + " damage to " + target.name + ".";
            return totalDamage;
        }

        public static Weapon GetUnarmedWeapon() {
            Weapon output = new Weapon("No weapon");
            output.physicalDamage.SetValue(10);
            return output;
        }

        public static Weapon GetSteelSword() {
            Weapon output = new Weapon("Steel sword");
            output.physicalDamage.SetValue(30);
            return output;
        }

        public static Weapon GetGoldenHope() {
            Weapon output = new Weapon("Golden Hope");
            output.physicalDamage.SetValue(20);
            output.lightDamage.SetValue(20);
            return output;
        }

    }
}
