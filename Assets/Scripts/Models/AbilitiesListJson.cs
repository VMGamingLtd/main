namespace Assets.Scripts.Models
{
    /// <summary>
    /// Effects type reference:
    /// Damage = 0
    /// Stun = 1
    /// Sleep = 2
    /// Buff = 3
    /// Debuff = 4
    /// 
    /// Prefab rotation reference:
    /// None = 0,
    /// ToEnemy = 1,
    /// Up = 2,
    /// Down = 3,
    /// Left = 4,
    /// Right = 5
    /// 
    /// Prefab start reference:
    /// MovePhase = 0,
    /// DamagePhase = 1,
    /// ReturnPhase = 2,
    /// 
    /// Stat affection type reference:
    /// None = 0
    /// Attack = 1,
    /// AttackSpeed = 2,
    /// Protection = 3,
    /// Health = 4,
    /// Armor = 5,
    /// Shield = 6,
    /// HitChance = 7,
    /// Dodge = 8
    /// </summary>
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
      ""isMovingAbility"": true,
      ""isFrontLineAoe"": true,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0.6,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.4,
      ""cooldown"": 0,
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 1,
      ""abilityName"": ""Stab"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": ""Blade"",
      ""isMovingAbility"": true,
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
          ""type"": 0,
          ""statAffection"": 0,
          ""chance"": 50,
          ""portionValue"": 50,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 2,
      ""abilityName"": ""AimedShot"",
      ""abilityType"": ""Ranged"",
      ""abilityWeapon"": ""Gun"",
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 1.6,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.8,
      ""cooldown"": 2,   
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 3,
      ""abilityName"": ""BurstShotPistol"",
      ""abilityType"": ""Ranged"",
      ""abilityWeapon"": ""Gun"",
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": true,
      ""isBackLineAoe"": true,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 1.2,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.1,
      ""cooldown"": 1,   
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 4,
      ""abilityName"": ""Choke"",
      ""abilityType"": ""Psi"",
      ""abilityWeapon"": ""Psi"",
      ""isMovingAbility"": false,
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
          ""type"": 1,
          ""statAffection"": 0,
          ""chance"": 50,
          ""portionValue"": 0,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 5,
      ""abilityName"": ""Terrify"",
      ""abilityType"": ""Psi"",
      ""abilityWeapon"": ""Psi"",
      ""isMovingAbility"": false,
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
          ""chance"": 75,
          ""type"": 1,
          ""statAffection"": 0,
          ""portionValue"": 0,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 6,
      ""abilityName"": ""MindControl"",
      ""abilityType"": ""Psi"",
      ""abilityWeapon"": ""Psi"",
      ""isMovingAbility"": false,
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
          ""type"": 1,
          ""statAffection"": 0,
          ""chance"": 75,
          ""portionValue"": 0,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 7,
      ""abilityName"": ""Swing"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": ""Blunt"",
      ""isMovingAbility"": true,
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
          ""type"": 1,
          ""statAffection"": 0,
          ""chance"": 25,
          ""portionValue"": 0,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 8,
      ""abilityName"": ""Smash"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": ""Blunt"",
      ""isMovingAbility"": true,
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
          ""type"": 1,
          ""statAffection"": 0,
          ""chance"": 75,
          ""portionValue"": 0,
          ""duration"": 1,
          ""isFrontLineAoe"": true,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 9,
      ""abilityName"": ""TorchBite"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0.8,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.4,
      ""cooldown"": 0,   
      ""negativeEffectsList"": [
        {
          ""name"": ""Ignite"",
          ""type"": 0,
          ""statAffection"": 0,
          ""chance"": 25,
          ""portionValue"": 25,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 10,
      ""abilityName"": ""FireSpray"",
      ""abilityType"": ""Ranged"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 1.2,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.8,
      ""cooldown"": 0,   
      ""negativeEffectsList"": [
        {
          ""name"": ""Ignite"",
          ""type"": 0,
          ""statAffection"": 0,
          ""chance"": 40,
          ""portionValue"": 35,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""FireSpray"",
          ""prefabRotation"": 1,
          ""prefabStart"": 0,
          ""prefabMovement"": 0
        }
      ]
    },
    {
      ""index"": 11,
      ""abilityName"": ""MagmaTouch"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 1,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.4,
      ""cooldown"": 0,   
      ""negativeEffectsList"": [
        {
          ""name"": ""AttackDebuff"",
          ""type"": 4,
          ""statAffection"": 1,
          ""chance"": 35,
          ""portionValue"": 25,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 12,
      ""abilityName"": ""ArmorBuff"",
      ""abilityType"": ""Buff"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 0,
      ""cooldown"": 3,   
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [
        {
          ""name"": ""ArmorBuff"",
          ""type"": 3,
          ""statAffection"": 5,
          ""chance"": 100,
          ""portionValue"": 30,
          ""duration"": 2,
          ""isFrontLineAoe"": true,
          ""isBackLineAoe"": false
        }
      ],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""ArmorBuff"",
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0
        }
      ]
    },
    {
      ""index"": 13,
      ""abilityName"": ""Fireball"",
      ""abilityType"": ""Ranged"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 1.2,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.8,
      ""cooldown"": 0,   
      ""negativeEffectsList"": [
        {
          ""name"": ""Ignite"",
          ""type"": 0,
          ""statAffection"": 0,
          ""chance"": 60,
          ""portionValue"": 30,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""Fireball"",
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 1
        },
        {
          ""prefabName"": ""FireballExplosion"",
          ""prefabRotation"": 0,
          ""prefabStart"": 1,
          ""prefabMovement"": 0
        }
      ]
    },
    {
      ""index"": 14,
      ""abilityName"": ""AttackSpeedBuff"",
      ""abilityType"": ""Buff"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 0,
      ""cooldown"": 3,   
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [
        {
          ""name"": ""AttackSpeedBuff"",
          ""type"": 3,
          ""statAffection"": 2,
          ""chance"": 100,
          ""portionValue"": 30,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""AttackSpeedBuff"",
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0
        }
      ]
    },
  ]
}
";
    }
}
