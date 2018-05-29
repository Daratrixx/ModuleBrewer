using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftDialogue;


namespace VisualRPG {

    public abstract class Ability {
        public Ability(string name) {
            this.name = name;
        }
        public string name;
        public string description;
        public int energyCost = 0;

        public virtual bool IsUsable(Character caster) { return true; }
        public abstract DialogueSpell GetSpellEffectDialogue(Character caster, Character target);

        public static Ability attack = new AttackAbility();
        public static Ability fury = new FuryAbility();
        public static Ability blessing = new BlessingAbility();

        public static Ability battleCry = new BattleCryAbility();
        public static Ability dank = new DankAbility();
    }

    #region SHARED_ABILITIES

    public class AttackAbility : Ability {
        public AttackAbility() : base("Attack") {
            description = "A normal attack using your weapon, or your hands.";
        }

        public override DialogueSpell GetSpellEffectDialogue(Character caster, Character target) {
            DialogueSpell output = new DialogueSpell(name);
            int damage = caster.weapon.GetDamage(caster, target);
            string text;
            if (caster.HasWeapon())
                text = "Using " + caster.weapon.name + ", " + caster.name + " attacks " + target.name + ".";
            else
                text = "Using their own hands as a weapon, " + caster.name + " attacks " + target.name + ".";
            output.AddNode(new DialogueSpeech(text));
            output.AddNode(new DialogueSpeech(damage + " damages!")
                    .AddEffect()
                    .AddHandler(new DialogueEffectDamageCharacterHandler(target, damage)));
            return output;
        }
    }

    #endregion

    #region PALADIN_ABILITIES

    public class BlessingAbility : Ability {
        public BlessingAbility() : base("Blessing") {
            description = "Bless the weapon of the user, adding extra light damage to each strike.";
            energyCost = 15;
        }

        public override bool IsUsable(Character caster) {
            return true; // caster.HasWeapon() && !caster.weapon.HasBuff(Buff.blessing);
        }

        public override DialogueSpell GetSpellEffectDialogue(Character caster, Character target) {
            DialogueSpell output = new DialogueSpell(name);
            string text;
            if (caster == target)
                text = caster.name + " impregnates " + caster.weapon.name + " with holy energy.";
            else
                text = caster.name + " impregnates " + target.name + "'s " + target.weapon.name + " with holy energy.";
            output.AddNode(new DialogueSpeech(text)
                .AddEffect()
                .AddHandler(new DialogueEffectBuffWeaponHandler(caster, target.weapon, Buff.blessing)));
            return output;
        }
    }

    public class FuryAbility : Ability {
        public FuryAbility() : base("Fury") {
            description = "Strike with openant with a furious blow, dealing extra physical damage based on Strength.\nIf the weapon is Blessed, deals extra light damage based on Spirit.";
            energyCost = 15;
        }

        public override DialogueSpell GetSpellEffectDialogue(Character caster, Character target) {
            DialogueSpell output = new DialogueSpell(name);
            bool blessing = caster.weapon.HasBuff(Buff.blessing);

            caster.weapon.physicalDamage.AddModification(caster.attributes.GetMainAttributeValue(MainAttribute.Strength));
            if (blessing)
                caster.weapon.lightDamage.AddModification(caster.attributes.GetMainAttributeValue(MainAttribute.Spirit));
            int damage = caster.weapon.GetDamage(caster, target);
            caster.weapon.physicalDamage.RemoveModification(caster.attributes.GetMainAttributeValue(MainAttribute.Strength));
            if (blessing)
                caster.weapon.lightDamage.RemoveModification(caster.attributes.GetMainAttributeValue(MainAttribute.Spirit));

            output.AddNode(new DialogueSpeech(caster.name + " strikes with a furious blow, crushing " + target.name + "."));
            output.AddNode(new DialogueSpeech(damage + " damages!")
                .AddEffect()
                .AddHandler(new DialogueEffectDamageCharacterHandler(target, damage)));
            return output;
        }
    }

    #endregion

    #region WARRIOR_ABILITIES

    public class BattleCryAbility : Ability {
        public BattleCryAbility() : base("Battle cry") {
            description = "Call's upon a warrior's spirit through a fierce battle cry. Increases your physical damage, and decreases the physical damage of your opponent, depending of your Charisma.";
            energyCost = 15;
        }

        public override DialogueSpell GetSpellEffectDialogue(Character caster, Character target) {
            DialogueSpell output = new DialogueSpell(name);
            output.AddNode(new DialogueSpeech(caster.name + " shouts a battle cry!"));
            output.AddNode(new DialogueSpeech(caster.name + " physical damages are increased.")
                .AddEffect()
                .AddHandler(new DialogueEffectBuffCharacterHandler(caster, caster, Buff.battleCrySource)));
            output.AddNode(new DialogueSpeech(target.name + " physical damages are decreased.")
                .AddEffect()
                .AddHandler(new DialogueEffectBuffCharacterHandler(caster, target, Buff.battleCryTarget)));
            return output;
        }
    }

    public class DankAbility : Ability {
        public DankAbility() : base("Dank") {
            description = "Memes are weapons. Don't do memes, kids.";
            energyCost = 15;
        }

        public override DialogueSpell GetSpellEffectDialogue(Character caster, Character target) {
            DialogueSpell output = new DialogueSpell(name);
            int damage = Fight.DamageCalculation(35, caster.attributes.GetSecondaryAttributeValue(SecondaryAttribute.PhysicalDamage), 0);

            output.AddNode(new DialogueSpeech("DANK DUNK! " + caster.name + " hits through " + target.name + "'s armor!"));
            output.AddNode(new DialogueSpeech(damage + " points of damage!!!")
                .AddEffect()
                .AddHandler(new DialogueEffectDamageCharacterHandler(target, damage)));
            return output;
        }
    }

    #endregion

}
