public static class Constants
{
    // Animation state names
    public const string DamageAnim = "Damage";
    public const string CritAnim = "Crit";
    public const string DeathAnim = "Death";
    public const string DodgeAnim = "Dodge";
    public const string MissAnim = "Miss";

    // Dungeon names
    public const string VolcanicCave = "VolcanicCave";
    public const string IceCave = "IceCave";
    public const string HiveNest = "HiveNest";
    public const string CyberHideout = "CyberHideout";
    public const string AlienBase = "AlienBase";
    public const string WormTunnels = "WormTunnels";
    public const string Shipwreck = "Shipwreck";
    public const string MysticTunnels = "MysticTunnels";

    // Asset bungle names
    public const string ResourceIcons = "resourceicons";
    public const string EquipmentIcons = "equipmenticons";
    public const string BuildingIcons = "buildingicons";
    public const string SkillsIcons = "skillsicons";
    public const string CombatIcons = "combaticons";
    public const string MiscIcons = "miscicons";
    public const string CharacterIcons = "charactericons";
    public const string FlagIcons = "flagicons";
    public const string AbilityPrefabs = "abilityprefabs";

    // Equipment slot buttons
    public const string EmptyButton = "EmptyButton";
    public const string OxygenButton = "OxygenButton";
    public const string EnergyButton = "EnergyButton";
    public const string HelmetButton = "HelmetButton";
    public const string SuitButton = "SuitButton";
    public const string ToolButton = "ToolButton";
    public const string LeftHandButton = "LeftHandButton";
    public const string BackpackButton = "BackpackButton";
    public const string RightHandButton = "RightHandButton";
    public const string DrillButton = "DrillButton";
    public const string WaterButton = "WaterButton";
    public const string HungerButton = "HungerButton";

    // Equipment slot icons
    public const string EmptyOxygenSlot = "EmptyOxygenSlot";
    public const string EmptyEnergySlot = "EmptyEnergySlot";
    public const string EmptyHelmetSlot = "EmptyHelmetSlot";
    public const string EmptySuitSlot = "EmptySuitSlot";
    public const string EmptyToolSlot = "EmptyToolSlot";
    public const string EmptyLeftHandSlot = "EmptyLeftHandSlot";
    public const string EmptyRightHandSlot = "EmptyRightHandSlot";
    public const string EmptyBackpackSlot = "EmptyBackpackSlot";
    public const string EmptyWaterSlot = "EmptyWaterSlot";
    public const string EmptyFoodSlot = "EmptyFoodSlot";

    // building types
    public const string Buildings = "BUILDINGS";
    public const string Laboratory = "LABORATORY";
    public const string Powerplant = "POWERPLANT";

    // building tags
    public const string EnergyTag = "Energy";
    public const string ResearchTag = "Research";
    public const string ConsumeTag = "Consume";
    public const string NoConsumeTag = "NoConsume";

    // Ability type names
    public const string Buff = "Buff";
    public const string Debuff = "Debuff";
    public const string Damage = "Damage";
    public const string Passive = "Passive";
    public const string PassiveReflect = "PassiveReflect";

    // Item type names
    public const string Suit = "SUIT";
    public const string Helmet = "HELMET";
    public const string Fabricator = "FABRICATOR";
    public const string Plants = "PLANTS";
    public const string Energy = "ENERGY";
    public const string Liquid = "LIQUID";
    public const string Oxygen = "OXYGEN";
    public const string Meat = "MEAT";
    public const string MeleeWeapon = "MELEEWEAPON";
    public const string RangedWeapon = "RANGEDWEAPON";
    public const string Shield = "SHIELD";
    public const string Offhand = "OFFHAND";

    // Item grade names
    public const string Level = "Level";
    public const string Basic = "BASIC";
    public const string Processed = "PROCESSED";
    public const string Enhanced = "ENHANCED";
    public const string Assembled = "ASSEMBLED";

    // Player needs and attributes
    public const string PlayerOxygen = "PlayerOxygen";
    public const string PlayerEnergy = "PlayerEnergy";
    public const string PlayerWater = "PlayerWater";
    public const string PlayerHunger = "PlayerHunger";

    // Player stats
    public const string MeleeDamage = "MeleeDamage";
    public const string MeleePhysicalDamage = "MeleePhysicalDamage";
    public const string MeleeFireDamage = "MeleeFireDamage";
    public const string MeleeColdDamage = "MeleeColdDamage";
    public const string MeleePoisonDamage = "MeleePoisonDamage";
    public const string MeleeEnergyDamage = "MeleeEnergyDamage";
    public const string RangedDamage = "RangedDamage";
    public const string RangedPhysicalDamage = "RangedPhysicalDamage";
    public const string RangedFireDamage = "RangedFireDamage";
    public const string RangedColdDamage = "RangedFireDamage";
    public const string RangedPoisonDamage = "RangedPoisonDamage";
    public const string RangedEnergyDamage = "RangedEnergyDamage";
    public const string PsiDamage = "PsiDamage";
    public const string PhysicalProtection = "PhysicalProtection";
    public const string FireProtection = "FireProtection";
    public const string ColdProtection = "ColdProtection";
    public const string PoisonProtection = "PoisonProtection";
    public const string EnergyProtection = "EnergyProtection";
    public const string PsiProtection = "PsiProtection";
    public const string ShieldPoints = "ShieldPoints";
    public const string Armor = "Armor";
    public const string HitPoints = "HitPoints";
    public const string EnergyCapacity = "EnergyCapacity";
    public const string InventorySlots = "InventorySlots";
    public const string Strength = "Strength";
    public const string Perception = "Perception";
    public const string Intelligence = "Intelligence";
    public const string Agility = "Agility";
    public const string Charisma = "Charisma";
    public const string Willpower = "Willpower";
    public const string VisibilityRadius = "VisibilityRadius";
    public const string ExplorationRadius = "ExplorationRadius";
    public const string PickupRadius = "PickupRadius";
    public const string ProductionSpeed = "ProductionSpeed";
    public const string MaterialCost = "MaterialCost";
    public const string OutcomeRate = "OutcomeRate";
    public const string HitChance = "HitChance";
    public const string CriticalChance = "CriticalChance";
    public const string CriticalDamage = "CriticalDamage";
    public const string Dodge = "Dodge";
    public const string Resistance = "Resistance";
    public const string CounterChance = "CounterChance";
    public const string Penetration = "Penetration";

    // UI Object addresses in Hierarchy
    public const string MessageObjects = "MessageCanvas/MESSAGEOBJECTS";
    public const string EventIcon = "EventIcon";

    // UI Object names in Hierarchy
    public const string CoroutineManager = "CoroutineManager";
    public const string RecipeCreatorList = "RecipeCreatorList";
    public const string GoalManager = "GOALMANAGER";
    public const string TranslationManager = "TranslationManager";
    public const string FightManager = "FIGHTMANAGER";
    public const string GlobalCalculator = "GlobalCalculator";
    public const string AudioManager = "AudioManager";
    public const string EquipmentManager = "EquipmentManager";
    public const string InventoryManager = "INVENTORYMANAGER";
    public const string InventoryPlaceholder = "InventoryPlaceholder";
    public const string CountInventory = "CountInventory";
    public const string InventoryContent = "InventoryContent";
    public const string HighlightImage = "HighlightImage";
    public const string NoEnergyObjects = "NoEnergyObjects";
    public const string ItemCreatorList = "ItemCreatorList";
    public const string LevelMark = "LevelMark";
    public const string UIManagerReference = "UIMANAGER";

    // Science projects
    public const string ResearchPoint = "ResearchPoint";
    public const string ScienceProjects = "ScienceProjects";
    public const string SteamPower = "SteamPower";
    public const string BasicAlchemy = "BasicAlchemy";
    public const string Pharmaceuticals = "Pharmaceuticals";
    public const string Explosives = "Explosives";


    // Inventory item and building names and resources
    public const string FibrousLeaves = "FibrousLeaves";
    public const string Water = "Water";
    public const string Biofuel = "Biofuel";
    public const string DistilledWater = "DistilledWater";
    public const string Battery = "Battery";
    public const string BatteryCore = "BatteryCore";
    public const string Wood = "Wood";
    public const string IronOre = "IronOre";
    public const string Coal = "Coal";
    public const string IronBeam = "IronBeam";
    public const string BiofuelGenerator = "BiofuelGenerator";
    public const string SteamGenerator = "SteamGenerator";
    public const string Boiler = "Boiler";
    public const string Furnace = "Furnace";
    public const string SmallPowerGrid = "SmallPowerGrid";
    public const string IronSheet = "IronSheet";
    public const string BiomassLeaves = "BiomassLeaves";
    public const string BiomassWood = "BiomassWood";
    public const string LatexFoam = "LatexFoam";
    public const string ProteinBeans = "ProteinBeans";
    public const string ProteinPowder = "ProteinPowder";
    public const string BioOil = "BioOil";
    public const string IronTube = "IronTube";
    public const string WaterPump = "WaterPump";
    public const string FibrousPlantField = "FibrousPlantField";
    public const string ResearchDevice = "ResearchDevice";
    public const string FishMeat = "FishMeat";
    public const string AnimalMeat = "AnimalMeat";
    public const string AnimalSkin = "AnimalSkin";
    public const string OxygenTank = "OxygenTank";
    public const string Milk = "Milk";
    public const string SilicaSand = "SilicaSnad";
    public const string Limestone = "Limestone";
    public const string Clay = "Clay";
    public const string Stone = "Stone";
    public const string CopperOre = "CopperOre";
    public const string SilverOre = "SilverOre";
    public const string GoldOre = "GoldOre";
}
