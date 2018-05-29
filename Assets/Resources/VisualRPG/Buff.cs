using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualRPG {

    public class BuffInstance {
        public BuffInstance(Buff buff, Character source = null) {
            this.buff = buff;
            this.source = source;
            remainingTime = buff.buffDuration;
        }

        public Buff buff;
        public Character source;
        public int remainingTime;
    }

    public class Buff {
        public Buff(string name) {
            this.name = name;
        }
        public string name;

        public int buffDuration = 0; //hhmmss

        public virtual void OnGained(Character source, Character target) { }
        public virtual void OnLost(Character source, Character target) { }

        public virtual void OnGained(Character source, Weapon target) { }
        public virtual void OnLost(Character source, Weapon target) { }

        public virtual void OnGained(Weapon source, Character target) { }
        public virtual void OnLost(Weapon source, Character target) { }

        public static Buff overcharge = new OverchargeBuff();
        public static Buff blessing = new BlessingBuff();

        public static Buff battleCrySource = new BattleCrySourceBuff();
        public static Buff battleCryTarget = new BattleCryTargetBuff();
    }

    #region PALADIN_BUFFS

    public class OverchargeBuff : Buff {
        public OverchargeBuff() : base("Overcharge") {
            buffDuration = 60;
        }
        public override void OnGained(Character source, Character target) {

        }
        public override void OnLost(Character source, Character target) {

        }
    }
    public class BlessingBuff : Buff {
        public BlessingBuff() : base("Blessing") {
            buffDuration = 240000;
        }
        public override void OnGained(Character source, Weapon target) {
            target.lightDamage.AddModification(10);
        }
        public override void OnLost(Character source, Weapon target) {
            target.lightDamage.RemoveModification(10);
        }
    }

    #endregion

    #region WARRIOR_BUFFS

    public class BattleCrySourceBuff : Buff {
        public BattleCrySourceBuff() : base("Battle cry") {
            buffDuration = 500;
        }
        public override void OnGained(Character source, Character target) {
            int amount = Fight.DamageCalculation(15, source.attributes.GetMainAttributeValue(MainAttribute.Charisma), 0);
            target.attributes.AddSecondaryAttributeModification(SecondaryAttribute.PhysicalDamage, amount);
        }
        public override void OnLost(Character source, Character target) {
            int amount = Fight.DamageCalculation(15, source.attributes.GetMainAttributeValue(MainAttribute.Charisma), 0);
            target.attributes.RemoveSecondaryAttributeModification(SecondaryAttribute.PhysicalDamage, amount);
        }
    }

    public class BattleCryTargetBuff : Buff {
        public BattleCryTargetBuff() : base("Battle cry") {
            buffDuration = 500;
        }
        public override void OnGained(Character source, Character target) {
            int amount = Fight.DamageCalculation(15, source.attributes.GetMainAttributeValue(MainAttribute.Charisma),
                target.attributes.GetMainAttributeValue(MainAttribute.Charisma));
            target.attributes.AddSecondaryAttributeModification(SecondaryAttribute.PhysicalDamage, -amount);
        }
        public override void OnLost(Character source, Character target) {
            int amount = Fight.DamageCalculation(15, source.attributes.GetMainAttributeValue(MainAttribute.Charisma),
                target.attributes.GetMainAttributeValue(MainAttribute.Charisma));
            target.attributes.RemoveSecondaryAttributeModification(SecondaryAttribute.PhysicalDamage, -amount);
        }
    }

    #endregion

}
