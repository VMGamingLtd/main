namespace Assets.Scripts.Models
{
    public class AbilitiesListJson
    {
        public static string json = @"
{
  ""header"": ""Available abilities"",
  ""abilities"": [
    {
      ""index"": 0,
      ""abilityName"": ""Slash"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": ""Blade"",
      ""isEnemyAbility"": true,
      ""isFrontLineAoe"": true,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0.6,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.4,
      ""cooldown"": 0,
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": []
    },
    {
      ""index"": 1,
      ""abilityName"": ""Stab"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": ""Blade"",
      ""isEnemyAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0.8,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.6,
      ""cooldown"": 1,   
      ""negativeEffectsList"": [
        {
          ""name"": ""Bleed"",
          ""value"": 50,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": []
    },
    {
      ""index"": 2,
      ""abilityName"": ""AimedShot"",
      ""abilityType"": ""Ranged"",
      ""abilityWeapon"": ""Gun"",
      ""isEnemyAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 1.6,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.8,
      ""cooldown"": 2,   
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": []
    },
    {
      ""index"": 3,
      ""abilityName"": ""BurstShotPistol"",
      ""abilityType"": ""Ranged"",
      ""abilityWeapon"": ""Gun"",
      ""isEnemyAbility"": true,
      ""isFrontLineAoe"": true,
      ""isBackLineAoe"": true,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 1.2,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.1,
      ""cooldown"": 1,   
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": []
    },
    {
      ""index"": 4,
      ""abilityName"": ""Choke"",
      ""abilityType"": ""Psi"",
      ""abilityWeapon"": ""Psi"",
      ""isEnemyAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 1.4,
      ""scaleMultiplication"": 1.6,
      ""cooldown"": 0,   
      ""negativeEffectsList"": [
        {
          ""name"": ""Stun"",
          ""value"": 50,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": []
    },
    {
      ""index"": 5,
      ""abilityName"": ""Terrify"",
      ""abilityType"": ""Psi"",
      ""abilityWeapon"": ""Psi"",
      ""isEnemyAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0.8,
      ""scaleMultiplication"": 1.5,
      ""cooldown"": 1,   
      ""negativeEffectsList"": [
        {
          ""name"": ""Stun"",
          ""value"": 75,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": []
    },
    {
      ""index"": 6,
      ""abilityName"": ""MindControl"",
      ""abilityType"": ""Psi"",
      ""abilityWeapon"": ""Psi"",
      ""isEnemyAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 1.8,
      ""scaleMultiplication"": 2,
      ""cooldown"": 3,   
      ""negativeEffectsList"": [
        {
          ""name"": ""Stun"",
          ""value"": 75,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": []
    },
    {
      ""index"": 7,
      ""abilityName"": ""Swing"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": ""Blunt"",
      ""isEnemyAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 1,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.5,
      ""cooldown"": 0,   
      ""negativeEffectsList"": [
        {
          ""name"": ""Stun"",
          ""value"": 25,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": []
    },
    {
      ""index"": 8,
      ""abilityName"": ""Smash"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": ""Blunt"",
      ""isEnemyAbility"": true,
      ""isFrontLineAoe"": true,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 1.5,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.8,
      ""cooldown"": 2,   
      ""negativeEffectsList"": [
        {
          ""name"": ""Stun"",
          ""value"": 75,
          ""duration"": 1,
          ""isFrontLineAoe"": true,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": []
    }
  ]
}
";
    }
}
