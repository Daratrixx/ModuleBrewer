using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualRPG {

    public class CharacterAttributes {

        public CharacterAttributes() {
            constitution.AddSecondaryAttribute(maximumHealth);
            constitution.AddSecondaryAttribute(physicalResistance);
            maximumHealth.mainAttributeRatio = 2;

            stamina.AddSecondaryAttribute(maximumEnergy);
            maximumEnergy.mainAttributeRatio = 3;

            strength.AddSecondaryAttribute(physicalDamage);
            //agility.AddSecondaryAttribute(physicalDamage);

            spirit.AddSecondaryAttribute(lightResistance);
            spirit.AddSecondaryAttribute(lightDamage);


            intellect.AddSecondaryAttribute(arcanResistance);
            intellect.AddSecondaryAttribute(arcanDamage);


            calm.AddSecondaryAttribute(mentalResistance);
            calm.AddSecondaryAttribute(mentalDamage);


            charisma.AddSecondaryAttribute(moralResistance);
            charisma.AddSecondaryAttribute(moralDamage);

            mainAttributes[MainAttribute.Constitution] = constitution;
            mainAttributes[MainAttribute.Stamina] = stamina;
            mainAttributes[MainAttribute.Strength] = strength;
            mainAttributes[MainAttribute.Agility] = agility;
            mainAttributes[MainAttribute.Spirit] = spirit;
            mainAttributes[MainAttribute.Intellect] = intellect;
            mainAttributes[MainAttribute.Calm] = calm;
            mainAttributes[MainAttribute.Charisma] = charisma;

            secondaryAttributes[SecondaryAttribute.MaximumHealth] = maximumHealth;
            secondaryAttributes[SecondaryAttribute.PhysicalResistance] = physicalResistance;
            secondaryAttributes[SecondaryAttribute.MaximumEnergy] = maximumEnergy;
            secondaryAttributes[SecondaryAttribute.PhysicalDamage] = physicalDamage;
            secondaryAttributes[SecondaryAttribute.LightResistance] = lightResistance;
            secondaryAttributes[SecondaryAttribute.LightDamage] = lightDamage;
            secondaryAttributes[SecondaryAttribute.ArcanResistance] = arcanResistance;
            secondaryAttributes[SecondaryAttribute.ArcanDamage] = arcanDamage;
            secondaryAttributes[SecondaryAttribute.MentalResistance] = mentalResistance;
            secondaryAttributes[SecondaryAttribute.MentalDamage] = mentalDamage;
            secondaryAttributes[SecondaryAttribute.MoralResistance] = moralResistance;
            secondaryAttributes[SecondaryAttribute.MoralDamage] = moralDamage;
        }

        private Dictionary<MainAttribute, Attribute> mainAttributes = new Dictionary<MainAttribute, Attribute>();
        private Dictionary<SecondaryAttribute, Attribute> secondaryAttributes = new Dictionary<SecondaryAttribute, Attribute>();

        private Attribute constitution = new Attribute();
        private Attribute stamina = new Attribute();
        private Attribute strength = new Attribute();
        private Attribute agility = new Attribute();
        private Attribute spirit = new Attribute();
        private Attribute intellect = new Attribute();
        private Attribute calm = new Attribute();
        private Attribute charisma = new Attribute();

        private Attribute maximumHealth = new Attribute();
        private Attribute physicalResistance = new Attribute();
        private Attribute maximumEnergy = new Attribute();
        private Attribute physicalDamage = new Attribute();
        private Attribute lightResistance = new Attribute();
        private Attribute lightDamage = new Attribute();
        private Attribute arcanResistance = new Attribute();
        private Attribute arcanDamage = new Attribute();
        private Attribute mentalResistance = new Attribute();
        private Attribute mentalDamage = new Attribute();
        private Attribute moralResistance = new Attribute();
        private Attribute moralDamage = new Attribute();

        public void SetMainAttributeValue(MainAttribute attribute, int value) {
            mainAttributes[attribute].SetValue(value);
        }

        public void SetSecondaryAttributeValue(SecondaryAttribute attribute, int value) {
            secondaryAttributes[attribute].SetValue(value);
        }

        public int GetMainAttributeValue(MainAttribute attribute) {
            return mainAttributes[attribute].GetValue();
        }

        public int GetSecondaryAttributeValue(SecondaryAttribute attribute) {
            return secondaryAttributes[attribute].GetValue();
        }

        public void AddMainAttributeModification(MainAttribute attribute, int modification) {
            mainAttributes[attribute].AddModification(modification);
        }

        public void AddSecondaryAttributeModification(SecondaryAttribute attribute, int modification) {
            secondaryAttributes[attribute].AddModification(modification);
        }

        public void RemoveMainAttributeModification(MainAttribute attribute, int modification) {
            mainAttributes[attribute].RemoveModification(modification);
        }

        public void RemoveSecondaryAttributeModification(SecondaryAttribute attribute, int modification) {
            secondaryAttributes[attribute].RemoveModification(modification);
        }



    }

    public class Attribute {
        private int baseValue;
        private int modificationValue;
        private List<int> modifications;
        private HashSet<Attribute> secondaryAttributes;
        public int mainAttributeRatio;

        public Attribute() {
            this.baseValue = 0;
            modificationValue = 0;
            modifications = new List<int>();
            secondaryAttributes = new HashSet<Attribute>();
            mainAttributeRatio = 1;
        }

        public int GetBaseValue() {
            return baseValue;
        }

        public void SetValue(int value) {
            if (value != baseValue) {
                baseValue = value;
                UpdateSecondaryAttributes();
            }
        }

        public int GetValue() {
            return baseValue + modificationValue;
        }

        public void AddModification(int modification) {
            if (modification != 0) {
                modifications.Add(modification);
                modificationValue += modification;
                UpdateSecondaryAttributes();
            }
        }

        public void RemoveModification(int modification) {
            if (modification != 0) {
                if (modifications.Remove(modification)) {
                    modificationValue -= modification;
                    UpdateSecondaryAttributes();
                }
            }
        }

        public Attribute AddSecondaryAttribute(Attribute secondary) {
            if (secondaryAttributes == null)
                secondaryAttributes = new HashSet<Attribute>();
            secondaryAttributes.Add(secondary);
            return this;
        }

        public void RemoveAllModification() {
            modifications.RemoveRange(0, modifications.Count);
            modificationValue = 0;
        }

        private void UpdateSecondaryAttributes() {
            if (secondaryAttributes == null)
                secondaryAttributes = new HashSet<Attribute>();
            int value = GetValue();
            foreach (Attribute attribute in secondaryAttributes) {
                attribute.SetValue(value * attribute.mainAttributeRatio);
            }
        }
    }

    public enum MainAttribute : short {
        Constitution,
        Stamina,
        Strength,
        Agility,
        Spirit,
        Intellect,
        Calm,
        Charisma
    }

    public enum SecondaryAttribute : short {
        MaximumHealth,
        PhysicalResistance,
        MaximumEnergy,
        PhysicalDamage,
        LightResistance,
        LightDamage,
        ArcanResistance,
        ArcanDamage,
        MentalResistance,
        MentalDamage,
        MoralResistance,
        MoralDamage
    }

    public enum DamageAttribute {
        Physical,
        Light,
        Arcan,
        Mental,
        Moral
    }

}
