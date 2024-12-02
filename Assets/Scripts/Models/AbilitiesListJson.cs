namespace Assets.Scripts.Models
{
    /// <summary>
    /// Effects type reference:
    /// Damage = 0
    /// Stun = 1
    /// Sleep = 2
    /// Buff = 3
    /// Debuff = 4
    /// Passive = 5
    /// 
    /// Prefab rotation reference:
    /// None = 0,
    /// ToEnemy = 1,
    /// Up = 2,
    /// Down = 3,
    /// Left = 4,
    /// Right = 5
    /// 
    /// Prefab spawn reference:
    /// User = 0,
    /// Target = 1,
    /// BattlefieldCenter = 2
    /// 
    /// Prefab start reference:
    /// MovePhase = 0,
    /// DamagePhase = 1,
    /// ReturnPhase = 2,
    /// 
    /// Prefab movement reference:
    /// None = 0,
    /// ToEnemy = 1
    /// 
    /// Prefab casting time reference:
    /// represents miliseconds delay for UniTask 
    /// in Start phase
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
    /// Dodge = 8,
    /// CounterChance = 9,
    /// CriticalChance = 10,
    /// CriticalDamage = 11
    /// 
    /// Ability trigger reference:
    /// None 0
    /// None = 0,
    /// Health25 = 1,
    /// Health50 = 2,
    /// Health75 = 3
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
      ""abilityTrigger"": 0,
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
      ""abilityTrigger"": 0,
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
      ""abilityTrigger"": 0,
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
      ""abilityTrigger"": 0,
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
      ""abilityTrigger"": 0,
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
      ""cooldown"": 2,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [
        {
          ""name"": ""Stun"",
          ""chance"": 75,
          ""type"": 1,
          ""statAffection"": 0,
          ""portionValue"": 0,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""Terrify"",
          ""prefabSpawn"": 1,
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 800
        }
      ]
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
      ""abilityTrigger"": 0,
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
      ""abilityTrigger"": 0,
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
      ""abilityTrigger"": 0,
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
      ""abilityTrigger"": 0,
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
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""FireHit1"",
          ""prefabSpawn"": 1,
          ""prefabRotation"": 0,
          ""prefabStart"": 1,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
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
      ""abilityTrigger"": 0,
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
          ""prefabSpawn"": 0,
          ""prefabRotation"": 1,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 0
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
      ""abilityTrigger"": 0,
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
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [
        {
          ""name"": ""ArmorBuff"",
          ""type"": 3,
          ""statAffection"": 5,
          ""chance"": 100,
          ""portionValue"": 30,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""ArmorBuff"",
          ""prefabSpawn"": 0,
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 0
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
      ""abilityTrigger"": 0,
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
          ""prefabSpawn"": 0,
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 1,
          ""castingTime"": 0
        },
        {
          ""prefabName"": ""FireballExplosion"",
          ""prefabSpawn"": 1,
          ""prefabRotation"": 0,
          ""prefabStart"": 1,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 14,
      ""abilityName"": ""AttackSpeed"",
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
      ""abilityTrigger"": 0,
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
          ""prefabSpawn"": 0,
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 15,
      ""abilityName"": ""FireSmash"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 1.4,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.8,
      ""cooldown"": 0,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [
        {
          ""name"": ""HitChanceDebuff"",
          ""type"": 4,
          ""statAffection"": 7,
          ""chance"": 100,
          ""portionValue"": 35,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""FireSmash"",
          ""prefabSpawn"": 1,
          ""prefabRotation"": 0,
          ""prefabStart"": 1,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 16,
      ""abilityName"": ""CounterArmorBuff"",
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
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [
        {
          ""name"": ""ArmorBuff"",
          ""type"": 3,
          ""statAffection"": 5,
          ""chance"": 100,
          ""portionValue"": 50,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        },
        {
          ""name"": ""CounterChanceBuff"",
          ""type"": 3,
          ""statAffection"": 9,
          ""chance"": 100,
          ""portionValue"": 30,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""ArmorBuff"",
          ""prefabSpawn"": 0,
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 17,
      ""abilityName"": ""MeteorShower"",
      ""abilityType"": ""Psi"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": true,
      ""isBackLineAoe"": true,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 1.0,
      ""scaleMultiplication"": 1.5,
      ""cooldown"": 3,
      ""abilityTrigger"": 0,
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
          ""prefabName"": ""MeteorShower"",
          ""prefabSpawn"": 2,
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 18,
      ""abilityName"": ""ArcticRend"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 1,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.6,
      ""cooldown"": 0,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [
        {
          ""name"": ""AttackSpeedDebuff"",
          ""type"": 4,
          ""statAffection"": 2,
          ""chance"": 35,
          ""portionValue"": 30,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""IceHit1"",
          ""prefabSpawn"": 1,
          ""prefabRotation"": 0,
          ""prefabStart"": 1,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 19,
      ""abilityName"": ""FrostBeam"",
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
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [
        {
          ""name"": ""AttackSpeedDebuff"",
          ""type"": 4,
          ""statAffection"": 2,
          ""chance"": 35,
          ""portionValue"": 30,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""FrostBeam"",
          ""prefabSpawn"": 0,
          ""prefabRotation"": 1,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 800
        }
      ]
    },
    {
      ""index"": 20,
      ""abilityName"": ""LifeRegen"",
      ""abilityType"": ""Passive"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 0,
      ""cooldown"": 0,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [
        {
          ""name"": ""HitPointsBuff"",
          ""type"": 5,
          ""statAffection"": 4,
          ""chance"": 100,
          ""portionValue"": 10,
          ""duration"": 0,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""LifeRegen"",
          ""prefabSpawn"": 0,
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 21,
      ""abilityName"": ""ChillingAura"",
      ""abilityType"": ""PassiveReflect"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 0,
      ""cooldown"": 0,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [
        {
          ""name"": ""AttackSpeedDebuff"",
          ""type"": 4,
          ""statAffection"": 2,
          ""chance"": 60,
          ""portionValue"": 30,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [
        {
          ""name"": ""ChillingAura"",
          ""type"": 3,
          ""statAffection"": 0,
          ""chance"": 100,
          ""portionValue"": 0,
          ""duration"": 0,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 22,
      ""abilityName"": ""Enrage"",
      ""abilityType"": ""Passive"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 0,
      ""cooldown"": 3,
      ""abilityTrigger"": 2,
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [
        {
          ""name"": ""AttackBuff"",
          ""type"": 3,
          ""statAffection"": 1,
          ""chance"": 100,
          ""portionValue"": 25,
          ""duration"": 3,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""DamageBuff"",
          ""prefabSpawn"": 0,
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 23,
      ""abilityName"": ""IceSpike"",
      ""abilityType"": ""Ranged"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 1.4,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.6,
      ""cooldown"": 0,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [
        {
          ""name"": ""AttackSpeedDebuff"",
          ""type"": 4,
          ""statAffection"": 2,
          ""chance"": 40,
          ""portionValue"": 30,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""IceSpike"",
          ""prefabSpawn"": 0,
          ""prefabRotation"": 1,
          ""prefabStart"": 0,
          ""prefabMovement"": 1,
          ""castingTime"": 0
        },
        {
          ""prefabName"": ""IceExplosion"",
          ""prefabSpawn"": 1,
          ""prefabRotation"": 0,
          ""prefabStart"": 1,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 24,
      ""abilityName"": ""Stealth"",
      ""abilityType"": ""Passive"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 0,
      ""cooldown"": 0,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [],
      ""positiveEffectsList"": [
        {
          ""name"": ""Invisible"",
          ""type"": 3,
          ""statAffection"": 0,
          ""chance"": 100,
          ""portionValue"": 0,
          ""duration"": 0,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""abilityPrefabsList"": []
    },
    {
      ""index"": 25,
      ""abilityName"": ""Inferno"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": true,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 1.2,
      ""scaleMultiplication"": 1.3,
      ""cooldown"": 3,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [
        {
          ""name"": ""Ignite"",
          ""type"": 0,
          ""statAffection"": 0,
          ""chance"": 75,
          ""portionValue"": 30,
          ""duration"": 2,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        },
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
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""Inferno"",
          ""prefabSpawn"": 1,
          ""prefabRotation"": 0,
          ""prefabStart"": 0,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 26,
      ""abilityName"": ""WhisperOfDeath"",
      ""abilityType"": ""Psi"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": false,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 0,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 1.4,
      ""scaleMultiplication"": 1.2,
      ""cooldown"": 0,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [
        {
          ""name"": ""Stun"",
          ""chance"": 25,
          ""type"": 1,
          ""statAffection"": 0,
          ""portionValue"": 0,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        },
        {
          ""name"": ""HitChanceDebuff"",
          ""type"": 4,
          ""statAffection"": 7,
          ""chance"": 50,
          ""portionValue"": 25,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""WhisperOfDeathHit"",
          ""prefabSpawn"": 1,
          ""prefabRotation"": 0,
          ""prefabStart"": 1,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    },
    {
      ""index"": 27,
      ""abilityName"": ""Frostbite"",
      ""abilityType"": ""Melee"",
      ""abilityWeapon"": null,
      ""isMovingAbility"": true,
      ""isFrontLineAoe"": false,
      ""isBackLineAoe"": false,
      ""meleeDamageScale"": 1,
      ""rangedDamageScale"": 0,
      ""psiDamageScale"": 0,
      ""scaleMultiplication"": 1.2,
      ""cooldown"": 0,
      ""abilityTrigger"": 0,
      ""negativeEffectsList"": [
        {
          ""name"": ""CriticalChanceDebuff"",
          ""type"": 4,
          ""statAffection"": 10,
          ""chance"": 40,
          ""portionValue"": 5,
          ""duration"": 1,
          ""isFrontLineAoe"": false,
          ""isBackLineAoe"": false
        }
      ],
      ""positiveEffectsList"": [],
      ""abilityPrefabsList"": [
        {
          ""prefabName"": ""IceHit1"",
          ""prefabSpawn"": 1,
          ""prefabRotation"": 0,
          ""prefabStart"": 1,
          ""prefabMovement"": 0,
          ""castingTime"": 0
        }
      ]
    }
  ]
}
";
    }
}
