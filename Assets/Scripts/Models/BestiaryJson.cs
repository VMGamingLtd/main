namespace Assets.Scripts.Models
{
    /// <summary>
    /// LOCATION refference:
    /// VolcanicCave = 9
    /// IceCave = 10
    /// HiveNest = 11
    /// CyberHideout = 12
    /// AlienBase = 13
    /// WormTunnels = 14
    /// Shipwreck = 15
    /// MysticTemple = 16
    /// 
    /// RACE reference:
    /// Humanoid = 0
    /// Beast = 1
    /// Insect = 2
    /// Reptilian = 3
    /// Mechanical = 4
    /// Ethereal = 5
    /// Mutant = 6
    /// Amorphous = 7
    /// 
    /// CLASS reference:
    /// Warrior = 0
    /// Rogue = 1
    /// Ranger = 2
    /// Mage = 3
    /// Warlock = 4
    /// 
    /// BATTLE FORMATION reference:
    /// Front line = 0
    /// Back line = 1
    /// </summary>
    public class BestiaryJson
    {
        public static string json = @"
{
  ""header"": ""Available creatures"",
  ""creatures"": [
    {
      ""index"": 0,
      ""location"": 9,
      ""tier"": 1,
      ""name"": ""FireBeetle"",
      ""race"": 2,
      ""classType"": 0,
      ""battleFormation"": 0,
      ""physicalProtection"": 4,
      ""fireProtection"": 4,
      ""coldProtection"": 0,
      ""poisonProtection"": 0,
      ""energyProtection"": 0,
      ""psiProtection"": 0,
      ""shieldPoints"": 0,
      ""armor"": 8,
      ""hitPoints"": 16,
      ""attackSpeed"": 1.2,
      ""meleePhysicalDamage"": 12,
      ""meleeFireDamage"": 8,
      ""meleeColdDamage"": 0,
      ""meleePoisonDamage"": 0,
      ""meleeEnergyDamage"": 0,
      ""rangedPhysicalDamage"": 0,
      ""rangedFireDamage"": 0,
      ""rangedColdDamage"": 0,  
      ""rangedPoisonDamage"": 0,
      ""rangedEnergyDamage"": 0,
      ""psiDamage"": 0,
      ""hitChance"": 75,
      ""criticalChance"": 5,
      ""criticalDamage"": 50,
      ""dodge"": 5,
      ""resistance"": 8,
      ""counterChance"": 1,
      ""penetration"": 4,
      ""strength"": 2,
      ""perception"": 0,
      ""intelligence"": 0,
      ""agility"": 1,
      ""charisma"": 0,
      ""willpower"": 0,
      ""abilities"": [
        {
          ""index"": 9,
          ""name"": ""TorchBite""
        }
      ]
    },
    {
      ""index"": 1,
      ""location"": 9,
      ""tier"": 1,
      ""name"": ""Cinderling"",
      ""race"": 2,
      ""classType"": 2,
      ""battleFormation"": 1,
      ""physicalProtection"": 2,
      ""fireProtection"": 2,
      ""coldProtection"": 0,
      ""poisonProtection"": 0,
      ""energyProtection"": 0,
      ""psiProtection"": 0,
      ""shieldPoints"": 0,
      ""armor"": 4,
      ""hitPoints"": 12,
      ""attackSpeed"": 1.2,
      ""meleePhysicalDamage"": 8,
      ""meleeFireDamage"": 10,
      ""meleeColdDamage"": 0,
      ""meleePoisonDamage"": 0,
      ""meleeEnergyDamage"": 0,
      ""rangedPhysicalDamage"": 6,
      ""rangedFireDamage"": 12,
      ""rangedColdDamage"": 0,  
      ""rangedPoisonDamage"": 0,
      ""rangedEnergyDamage"": 0,
      ""psiDamage"": 0,
      ""hitChance"": 80,
      ""criticalChance"": 5,
      ""criticalDamage"": 50,
      ""dodge"": 8,
      ""resistance"": 6,
      ""counterChance"": 2,
      ""penetration"": 6,
      ""strength"": 1,
      ""perception"": 0,
      ""intelligence"": 0,
      ""agility"": 4,
      ""charisma"": 0,
      ""willpower"": 0,
      ""abilities"": [
        {
          ""index"": 10,
          ""name"": ""FireSpray""
        }
      ]
    },
        {
      ""index"": 2,
      ""location"": 9,
      ""tier"": 2,
      ""name"": ""MagmaSlime"",
      ""race"": 7,
      ""classType"": 0,
      ""battleFormation"": 0,
      ""physicalProtection"": 6,
      ""fireProtection"": 6,
      ""coldProtection"": 0,
      ""poisonProtection"": 4,
      ""energyProtection"": 0,
      ""psiProtection"": 4,
      ""shieldPoints"": 0,
      ""armor"": 22,
      ""hitPoints"": 24,
      ""attackSpeed"": 1.4,
      ""meleePhysicalDamage"": 8,
      ""meleeFireDamage"": 20,
      ""meleeColdDamage"": 0,
      ""meleePoisonDamage"": 0,
      ""meleeEnergyDamage"": 0,
      ""rangedPhysicalDamage"": 0,
      ""rangedFireDamage"": 0,
      ""rangedColdDamage"": 0,  
      ""rangedPoisonDamage"": 0,
      ""rangedEnergyDamage"": 0,
      ""psiDamage"": 0,
      ""hitChance"": 90,
      ""criticalChance"": 5,
      ""criticalDamage"": 50,
      ""dodge"": 8,
      ""resistance"": 12,
      ""counterChance"": 2,
      ""penetration"": 6,
      ""strength"": 4,
      ""perception"": 0,
      ""intelligence"": 0,
      ""agility"": 4,
      ""charisma"": 0,
      ""willpower"": 0,
      ""abilities"": [
        {
          ""index"": 11,
          ""name"": ""MagmaTouch""
        },
        {
          ""index"": 12,
          ""name"": ""Reinforce""
        }
      ]
    },
    {
      ""index"": 3,
      ""location"": 9,
      ""tier"": 2,
      ""name"": ""EmberSerpent"",
      ""race"": 3,
      ""classType"": 2,
      ""battleFormation"": 1,
      ""physicalProtection"": 3,
      ""fireProtection"": 6,
      ""coldProtection"": 0,
      ""poisonProtection"": 0,
      ""energyProtection"": 0,
      ""psiProtection"": 0,
      ""shieldPoints"": 0,
      ""armor"": 12,
      ""hitPoints"": 24,
      ""attackSpeed"": 2,
      ""meleePhysicalDamage"": 0,
      ""meleeFireDamage"": 0,
      ""meleeColdDamage"": 0,
      ""meleePoisonDamage"": 0,
      ""meleeEnergyDamage"": 0,
      ""rangedPhysicalDamage"": 6,
      ""rangedFireDamage"": 18,
      ""rangedColdDamage"": 0,  
      ""rangedPoisonDamage"": 0,
      ""rangedEnergyDamage"": 0,
      ""psiDamage"": 0,
      ""hitChance"": 90,
      ""criticalChance"": 10,
      ""criticalDamage"": 80,
      ""dodge"": 12,
      ""resistance"": 8,
      ""counterChance"": 4,
      ""penetration"": 8,
      ""strength"": 2,
      ""perception"": 0,
      ""intelligence"": 0,
      ""agility"": 6,
      ""charisma"": 0,
      ""willpower"": 0,
      ""abilities"": [
        {
          ""index"": 13,
          ""name"": ""Fireball""
        },
        {
          ""index"": 14,
          ""name"": ""AttackSpeedBuff""
        }
      ]
    }
  ]
}
";
    }
}
