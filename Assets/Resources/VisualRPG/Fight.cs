using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftDialogue;

namespace VisualRPG {
    public class Fight {







        public static int DamageCalculation(float baseDamage, float damageBoost, float damageReduction) {
            if (damageReduction >= damageBoost + 100)
                return 0;
            return (int)((baseDamage * (100 + (damageBoost - damageReduction) * 2)) / 100);
        }
    }

    public class DialogueSpell : DialogueBranch {

        //string spellName;

        public DialogueSpell(string spellName) : base(-1, -1) {
            //this.spellName = spellName;
        }
    }

    public class DialogueEffectDamageCharacterHandler : DialogueEffectHandler {
        public DialogueEffectDamageCharacterHandler(Character target, int damage) : base() {
            this.target = target;
            this.damage = damage;
        }

        public override void Handle(DialogueContext context) {
            target.DoDamage(damage);
        }

        private Character target;
        private int damage;
    }

    public class DialogueEffectHealCharacterHandler : DialogueEffectHandler {
        public DialogueEffectHealCharacterHandler(Character target, int heal) : base() {
            this.target = target;
            this.heal = heal;
        }

        public override void Handle(DialogueContext context) {
            target.DoHeal(heal);
        }
        private Character target;
        private int heal;
    }

    public class DialogueEffectMainAttributeModifierHandler : DialogueEffectHandler {
        public DialogueEffectMainAttributeModifierHandler(Character target, MainAttribute attribute, int mod, bool add = true) : base() {
            this.target = target;
            this.attribute = attribute;
            this.mod = mod;
            this.add = add;
        }

        public override void Handle(DialogueContext context) {
            if (add)
                target.attributes.AddMainAttributeModification(attribute, mod);
            else
                target.attributes.RemoveMainAttributeModification(attribute, mod);
        }
        private Character target;
        private MainAttribute attribute;
        private int mod;
        private bool add;
    }

    public class DialogueEffectSecondaryAttributeModifierHandler : DialogueEffectHandler {
        public DialogueEffectSecondaryAttributeModifierHandler(Character target, SecondaryAttribute attribute, int mod, bool add = true) : base() {
            this.target = target;
            this.attribute = attribute;
            this.mod = mod;
            this.add = add;
        }

        public override void Handle(DialogueContext context) {
            if (add)
                target.attributes.AddSecondaryAttributeModification(attribute, mod);
            else
                target.attributes.RemoveSecondaryAttributeModification(attribute, mod);
        }
        private Character target;
        private SecondaryAttribute attribute;
        private int mod;
        private bool add;
    }

    public class DialogueEffectBuffCharacterHandler : DialogueEffectHandler {
        public DialogueEffectBuffCharacterHandler(Character source, Character target, Buff buff, bool add = true) : base() {
            this.source = source;
            this.target = target;
            this.buff = buff;
            this.add = add;
        }

        public override void Handle(DialogueContext context) {
            if (add)
                target.AddBuff(buff, source);
            else
                target.RemoveBuff(buff);
        }
        private Character source;
        private Character target;
        private Buff buff;
        private bool add;
    }

    public class DialogueEffectBuffWeaponHandler : DialogueEffectHandler {
        public DialogueEffectBuffWeaponHandler(Character source, Weapon target, Buff buff, bool add = true) : base() {
            this.source = source;
            this.target = target;
            this.buff = buff;
            this.add = add;
        }

        public override void Handle(DialogueContext context) {
            if (add)
                target.AddBuff(buff, source);
            else
                target.RemoveBuff(buff);
        }
        private Character source;
        private Weapon target;
        private Buff buff;
        private bool add;
    }

}
