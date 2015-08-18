using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Items
{
    [DataContract]
    public class BasicDataStats
    {
        [DataMember]
        public double? FlatArmorMod { get; set; }
        [DataMember]
        public double? FlatAttackSpeedMod { get; set; }
        [DataMember]
        public double? FlatBlockMod { get; set; }
        [DataMember]
        public double? FlatCritChanceMod { get; set; }
        [DataMember]
        public double? FlatCritDamageMod { get; set; }
        [DataMember]
        public double? FlatEXPBonus { get; set; }
        [DataMember]
        public double? FlatEnergyPoolMod { get; set; }
        [DataMember]
        public double? FlatEnergyRegenMod { get; set; }
        [DataMember]
        public double? FlatHPPoolMod { get; set; }
        [DataMember]
        public double? FlatHPRegenMod { get; set; }
        [DataMember]
        public double? FlatMPPoolMod { get; set; }
        [DataMember]
        public double? FlatMPRegenMod { get; set; }
        [DataMember]
        public double? FlatMagicDamageMod { get; set; }
        [DataMember]
        public double? FlatMovementSpeedMod { get; set; }
        [DataMember]
        public double? FlatPhysicalDamageMod { get; set; }
        [DataMember]
        public double? FlatSpellBlockMod { get; set; }
        [DataMember]
        public double? PercentArmorMod { get; set; }
        [DataMember]
        public double? PercentAttackSpeedMod { get; set; }
        [DataMember]
        public double? PercentBlockMod { get; set; }
        [DataMember]
        public double? PercentCritChanceMod { get; set; }
        [DataMember]
        public double? PercentCritDamageMod { get; set; }
        [DataMember]
        public double? PercentDodgeMod { get; set; }
        [DataMember]
        public double? PercentEXPBonus { get; set; }
        [DataMember]
        public double? PercentHPPoolMod { get; set; }
        [DataMember]
        public double? PercentHPRegenMod { get; set; }
        [DataMember]
        public double? PercentLifeStealMod { get; set; }
        [DataMember]
        public double? PercentMPPoolMod { get; set; }
        [DataMember]
        public double? PercentMPRegenMod { get; set; }
        [DataMember]
        public double? PercentMagicDamageMod { get; set; }
        [DataMember]
        public double? PercentMovementSpeedMod { get; set; }
        [DataMember]
        public double? PercentPhysicalDamageMod { get; set; }
        [DataMember]
        public double? PercentSpellBlockMod { get; set; }
        [DataMember]
        public double? PercentSpellVampMod { get; set; }
        [DataMember(Name="RFlatArmorModPerLevel")]
        public double? RFlatArmorModPerLevel { get; set; }
        [DataMember(Name = "RFlatArmorPenetrationMod")]
        public double? RFlatArmorPenetrationMod { get; set; }
        [DataMember(Name = "RFlatArmorPenetrationModPerLevel")]
        public double? RFlatArmorPenetrationModPerLevel { get; set; }
        [DataMember(Name = "RFlatCritChanceModPerLevel")]
        public double? RFlatCritChanceModPerLevel { get; set; }
        [DataMember(Name = "RFlatCritDamageModPerLevel")]
        public double? RFlatCritDamageModPerLevel { get; set; }
        [DataMember(Name = "RFlatDodgeMod")]
        public double? RFlatDodgeMod { get; set; }
        [DataMember(Name = "RFlatDodgeModPerLevel")]
        public double? RFlatDodgeModPerLevel { get; set; }
        [DataMember(Name = "RFlatEnergyModPerLevel")]
        public double? RFlatEnergyModPerLevel { get; set; }
        [DataMember(Name = "RFlatEnergyRegenModPerLevel")]
        public double? RFlatEnergyRegenModPerLevel { get; set; }
        [DataMember(Name = "RFlatGoldPer10Mod")]
        public double? RFlatGoldPer10Mod { get; set; }
        [DataMember(Name = "RFlatHPModPerLevel")]
        public double? RFlatHPModPerLevel { get; set; }
        [DataMember(Name = "RFlatHPRegenModPerLevel")]
        public double? RFlatHPRegenModPerLevel { get; set; }
        [DataMember(Name = "RFlatMPModPerLevel")]
        public double? RFlatMPModPerLevel { get; set; }
        [DataMember(Name = "RFlatMPRegenModPerLevel")]
        public double? RFlatMPRegenModPerLevel { get; set; }
        [DataMember(Name = "RFlatMagicDamageModPerLevel")]
        public double? RFlatMagicDamageModPerLevel { get; set; }
        [DataMember(Name = "RFlatMagicPenetrationMod")]
        public double? RFlatMagicPenetrationMod { get; set; }
        [DataMember(Name = "RFlatMagicPenetrationModPerLevel")]
        public double? RFlatMagicPenetrationModPerLevel { get; set; }
        [DataMember(Name = "RFlatMovementSpeedModPerLevel")]
        public double? RFlatMovementSpeedModPerLevel { get; set; }
        [DataMember(Name = "RFlatSpellBlockModPerLevel")]
        public double? RFlatSpellBlockModPerLevel { get; set; }
        [DataMember(Name = "RFlatTimeDeadMod")]
        public double? RFlatTimeDeadMod { get; set; }
        [DataMember(Name = "RFlatTimeDeadModPerLevel")]
        public double? RFlatTimeDeadModPerLevel { get; set; }
        [DataMember(Name = "RPercentArmorPenetrationMod")]
        public double? rPercentArmorPenetrationMod { get; set; }
        [DataMember(Name = "RPercentArmorPenetrationModPerLevel")]
        public double? rPercentArmorPenetrationModPerLevel { get; set; }
        [DataMember(Name = "RPercentAttackSpeedModPerLevel")]
        public double? rPercentAttackSpeedModPerLevel { get; set; }
        [DataMember(Name = "RPercentCooldownMod")]
        public double? rPercentCooldownMod { get; set; }
        [DataMember(Name = "RPercentCooldownModPerLevel")]
        public double? rPercentCooldownModPerLevel { get; set; }
        [DataMember(Name = "RPercentMagicPenetrationMod")]
        public double? rPercentMagicPenetrationMod { get; set; }
        [DataMember(Name = "RPercentMagicPenetrationModPerLevel")]
        public double? rPercentMagicPenetrationModPerLevel { get; set; }
        [DataMember(Name = "RPercentMovementSpeedModPerLevel")]
        public double? rPercentMovementSpeedModPerLevel { get; set; }
        [DataMember(Name = "RPercentTimeDeadMod")]
        public double? rPercentTimeDeadMod { get; set; }
        [DataMember(Name = "RPercentTimeDeadModPerLevel")]
        public double? rPercentTimeDeadModPerLevel { get; set; }
    }
}
