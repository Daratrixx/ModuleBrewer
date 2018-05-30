using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {
    [System.Serializable]
    [CreateAssetMenu(fileName = "New StatisticSheet", menuName = "Heck/StatisticSheet")]
    public class StatisticSheet : PrimaryStatisticList {

        private void Awake() {
            PropagateSecondaryStatistics();
        }

        public int level = 0;

        public Statistic health;
        public Statistic energy;

        public Statistic physicalAmplification; // increase physical damage output
        public Statistic physicalReduction; // decrease physical damage received
        public Statistic strenghAmplification; // increase physical damage output
        public Statistic dexterityAmplification; // increase physical damage output

        public Statistic magicalAmplification; // increase magical damage output
        public Statistic magicalReduction; // decrease magical damage received
        public Statistic darkAmplification; // increase dark damage output
        public Statistic darkReduction; // decrease dark damage received
        public Statistic elementalAmplification; // increase elemental damage output. TODO : add heat/cold/shock
        public Statistic elementalReduction; // decrease elemental damage received.  TODO : add heat/cold/shock
        public Statistic holyAmplification; // increase holy damage and heal output
        public Statistic holyReduction; // decrease holy damage received

        public Statistic globalAmplification; // increase all damage output
        public Statistic globalReduction; // decrease all damage received

        public Statistic consumableEfficiency; // increase consumable effect power
        public Statistic consumableSpeed; // increase consumable utilisation speed

        public void UpdateStatistics(StatisticSheet stats, int modifier = 1) {
            level += stats.level * modifier;
            UpdatePrimaryStatistics(stats, modifier);
            UpdateSecondaryStatistics(stats, modifier);
            PropagateSecondaryStatistics();
        }

        public void UpdatePrimaryStatistics(PrimaryStatisticList stats, int modifier = 1) {
            UpdateConstitution(stats, modifier);
            UpdateStamina(stats, modifier);
            UpdateStrength(stats, modifier);
            UpdateDexterity(stats, modifier);
            UpdateIntelligence(stats, modifier);
            UpdateFaith(stats, modifier);
            UpdateIngenuity(stats, modifier);
            UpdateNegation(stats, modifier);
        }

        public void UpdateSecondaryStatistics(StatisticSheet stats, int modifier = 1) {
            UpdateHealth(stats, modifier);
            UpdateEnergy(stats, modifier);
            UpdatePhysicalAmplification(stats, modifier);
            UpdatePhysicalReduction(stats, modifier);
            UpdateStrengthAmplification(stats, modifier);
            UpdateDexterityAmplification(stats, modifier);

            UpdateMagicalAmplification(stats, modifier);
            UpdateMagicalReduction(stats, modifier);
            UpdateDarkAmplification(stats, modifier);
            UpdateDarkReduction(stats, modifier);
            UpdateElementalAmplification(stats, modifier);
            UpdateElementalReduction(stats, modifier);
            UpdateHolyAmplification(stats, modifier);
            UpdateHolyReduction(stats, modifier);

            UpdateGlobalAmplification(stats, modifier);
            UpdateGloballReduction(stats, modifier);

            UpdateConsumableEfficiency(stats, modifier);
            UpdateConsumableSpeed(stats, modifier);
        }

        public void PropagateSecondaryStatistics() {
            health.baseValue = 500 + 15 * constitution.totalValue;
            energy.baseValue = 100 + 3 * stamina.totalValue;

            physicalReduction.baseValue = (int)(constitution.totalValue * 0.25)
                + (int)(stamina.totalValue * 0.25)
                + (int)(strength.totalValue * 0.5);
            strenghAmplification.baseValue = (int)(strength.totalValue * 1);
            dexterityAmplification.baseValue = (int)(dexterity.totalValue * 1.5);

            magicalAmplification.baseValue = (int)(intelligence.totalValue * 2);
            magicalReduction.baseValue = (int)(intelligence.totalValue * 1);
            darkAmplification.baseValue = (int)(intelligence.totalValue * 2);
            darkReduction.baseValue = (int)(intelligence.totalValue * 1);
            elementalAmplification.baseValue = (int)(faith.totalValue * 2);
            elementalReduction.baseValue = (int)(faith.totalValue * 1);
            holyAmplification.baseValue = (int)(faith.totalValue * 2);
            holyReduction.baseValue = (int)(faith.totalValue * 1);

            globalAmplification.baseValue = (int)(level * 0.1);
            globalReduction.baseValue = (int)(level * 0.1)
                + (int)(ingenuity.totalValue * 0.5)
                + (int)(negation.totalValue * 1.5);

            consumableEfficiency.baseValue = (int)(ingenuity.totalValue * 1);
            consumableSpeed.baseValue = (int)(ingenuity.totalValue * 0.1);

        }

        #region PRIMARY STATISTICS

        private void UpdateConstitution(PrimaryStatisticList stats, int modifier) {
            if (stats.constitution.baseValue == 0 && stats.constitution.bonusValue == 0)
                return;
            this.constitution.baseValue += stats.constitution.baseValue * modifier;
            this.constitution.bonusValue += stats.constitution.bonusValue * modifier;
        }

        private void UpdateStamina(PrimaryStatisticList stats, int modifier) {
            if (stats.stamina.baseValue == 0 && stats.stamina.bonusValue == 0)
                return;
            this.stamina.baseValue += stats.stamina.baseValue * modifier;
            this.stamina.bonusValue += stats.stamina.bonusValue * modifier;
        }

        private void UpdateStrength(PrimaryStatisticList stats, int modifier) {
            if (stats.strength.baseValue == 0 && stats.strength.bonusValue == 0)
                return;
            this.strength.baseValue += stats.strength.baseValue * modifier;
            this.strength.bonusValue += stats.strength.bonusValue * modifier;
        }

        private void UpdateDexterity(PrimaryStatisticList stats, int modifier) {
            if (stats.dexterity.baseValue == 0 && stats.dexterity.bonusValue == 0)
                return;
            this.dexterity.baseValue += stats.dexterity.baseValue * modifier;
            this.dexterity.bonusValue += stats.dexterity.bonusValue * modifier;
        }

        private void UpdateIntelligence(PrimaryStatisticList stats, int modifier) {
            if (stats.intelligence.baseValue == 0 && stats.intelligence.bonusValue == 0)
                return;
            this.intelligence.baseValue += stats.intelligence.baseValue * modifier;
            this.intelligence.bonusValue += stats.intelligence.bonusValue * modifier;
        }

        private void UpdateFaith(PrimaryStatisticList stats, int modifier) {
            if (stats.faith.baseValue == 0 && stats.faith.bonusValue == 0)
                return;
            this.faith.baseValue += stats.faith.baseValue * modifier;
            this.faith.bonusValue += stats.faith.bonusValue * modifier;
        }

        private void UpdateIngenuity(PrimaryStatisticList stats, int modifier) {
            if (stats.ingenuity.baseValue == 0 && stats.ingenuity.bonusValue == 0)
                return;
            this.ingenuity.baseValue += stats.ingenuity.baseValue * modifier;
            this.ingenuity.bonusValue += stats.ingenuity.bonusValue * modifier;
        }

        private void UpdateNegation(PrimaryStatisticList stats, int modifier) {
            if (stats.faith.baseValue == 0 && stats.faith.bonusValue == 0)
                return;
            this.negation.baseValue += stats.negation.baseValue * modifier;
            this.negation.bonusValue += stats.negation.bonusValue * modifier;
        }
        #endregion // PRIMARY STATISTICS

        #region SECONDARY STATISTICS

        private void UpdateHealth(StatisticSheet stats, int modifier) {
            if (stats.health.bonusValue == 0)
                return;
            this.health.bonusValue += stats.health.bonusValue * modifier;
        }

        private void UpdateEnergy(StatisticSheet stats, int modifier) {
            if (stats.energy.bonusValue == 0)
                return;
            this.energy.bonusValue += stats.energy.bonusValue * modifier;
        }

        private void UpdatePhysicalAmplification(StatisticSheet stats, int modifier) {
            if (stats.physicalAmplification.bonusValue == 0)
                return;
            this.physicalAmplification.bonusValue += stats.physicalAmplification.bonusValue * modifier;
        }

        private void UpdatePhysicalReduction(StatisticSheet stats, int modifier) {
            if (stats.physicalReduction.bonusValue == 0)
                return;
            this.physicalReduction.bonusValue += stats.physicalReduction.bonusValue * modifier;
        }

        private void UpdateStrengthAmplification(StatisticSheet stats, int modifier) {
            if (stats.strenghAmplification.bonusValue == 0)
                return;
            this.strenghAmplification.bonusValue += stats.strenghAmplification.bonusValue * modifier;
        }

        private void UpdateDexterityAmplification(StatisticSheet stats, int modifier) {
            if (stats.dexterityAmplification.bonusValue == 0)
                return;
            this.dexterityAmplification.bonusValue += stats.dexterityAmplification.bonusValue * modifier;
        }

        private void UpdateMagicalAmplification(StatisticSheet stats, int modifier) {
            if (stats.magicalAmplification.bonusValue == 0)
                return;
            this.magicalAmplification.bonusValue += stats.magicalAmplification.bonusValue * modifier;
        }

        private void UpdateMagicalReduction(StatisticSheet stats, int modifier) {
            if (stats.magicalReduction.bonusValue == 0)
                return;
            this.magicalReduction.bonusValue += stats.magicalReduction.bonusValue * modifier;
        }

        private void UpdateDarkAmplification(StatisticSheet stats, int modifier) {
            if (stats.darkAmplification.bonusValue == 0)
                return;
            this.darkAmplification.bonusValue += stats.darkAmplification.bonusValue * modifier;
        }

        private void UpdateDarkReduction(StatisticSheet stats, int modifier) {
            if (stats.darkReduction.bonusValue == 0)
                return;
            this.darkReduction.bonusValue += stats.darkReduction.bonusValue * modifier;
        }

        private void UpdateElementalAmplification(StatisticSheet stats, int modifier) {
            if (stats.elementalAmplification.bonusValue == 0)
                return;
            this.elementalAmplification.bonusValue += stats.elementalAmplification.bonusValue * modifier;
        }

        private void UpdateElementalReduction(StatisticSheet stats, int modifier) {
            if (stats.elementalReduction.bonusValue == 0)
                return;
            this.elementalReduction.bonusValue += stats.elementalReduction.bonusValue * modifier;
        }

        private void UpdateHolyAmplification(StatisticSheet stats, int modifier) {
            if (stats.holyAmplification.bonusValue == 0)
                return;
            this.holyAmplification.bonusValue += stats.holyAmplification.bonusValue * modifier;
        }

        private void UpdateHolyReduction(StatisticSheet stats, int modifier) {
            if (stats.holyReduction.bonusValue == 0)
                return;
            this.holyReduction.bonusValue += stats.holyReduction.bonusValue * modifier;
        }

        private void UpdateGlobalAmplification(StatisticSheet stats, int modifier) {
            if (stats.globalAmplification.bonusValue == 0)
                return;
            this.globalAmplification.bonusValue += stats.globalAmplification.bonusValue * modifier;
        }

        private void UpdateGloballReduction(StatisticSheet stats, int modifier) {
            if (stats.globalReduction.bonusValue == 0)
                return;
            this.globalReduction.bonusValue += stats.globalReduction.bonusValue * modifier;
        }

        private void UpdateConsumableEfficiency(StatisticSheet stats, int modifier) {
            if (stats.consumableEfficiency.bonusValue == 0)
                return;
            this.consumableEfficiency.bonusValue += stats.consumableEfficiency.bonusValue * modifier;
        }

        private void UpdateConsumableSpeed(StatisticSheet stats, int modifier) {
            if (stats.consumableSpeed.bonusValue == 0)
                return;
            this.consumableSpeed.bonusValue += stats.consumableSpeed.bonusValue * modifier;
        }

        #endregion // SECONDARY STATISTICS
    }

    public class PrimaryStatisticList : ScriptableObject {
        public Statistic constitution;
        public Statistic stamina;
        public Statistic strength;
        public Statistic dexterity;
        public Statistic intelligence;
        public Statistic faith;
        public Statistic ingenuity;
        public Statistic negation;
    }

    [System.Serializable]
    public struct Statistic {
        public int baseValue;
        public int bonusValue;
        [HideInInspector]
        public int totalValue {
            get { return baseValue + bonusValue; }
        }

        public Statistic(int baseValue = 0) { this.baseValue = baseValue; bonusValue = 0; }
    }

}
