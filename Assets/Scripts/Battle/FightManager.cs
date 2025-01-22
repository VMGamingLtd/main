using Cysharp.Threading.Tasks;
using ItemManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enumerations;

[Serializable]
public class BestiaryDataJson
{
    public int index;
    [JsonConverter(typeof(StringEnumConverter))]
    public EventIconType location;
    public int tier;
    public string name;
    [JsonConverter(typeof(StringEnumConverter))]
    public Race race;
    [JsonConverter(typeof(StringEnumConverter))]
    public ClassType classType;
    [JsonConverter(typeof(StringEnumConverter))]
    public BattleFormation battleFormation;
    public int physicalProtection;
    public int fireProtection;
    public int coldProtection;
    public int poisonProtection;
    public int energyProtection;
    public int psiProtection;
    public int shieldPoints;
    public int armor;
    public int hitPoints;
    public float attackSpeed;
    public int meleePhysicalDamage;
    public int meleeFireDamage;
    public int meleeColdDamage;
    public int meleePoisonDamage;
    public int meleeEnergyDamage;
    public int rangedPhysicalDamage;
    public int rangedFireDamage;
    public int rangedColdDamage;
    public int rangedPoisonDamage;
    public int rangedEnergyDamage;
    public int psiDamage;
    public int hitChance;
    public int criticalChance;
    public int criticalDamage;
    public int dodge;
    public int resistance;
    public int counterChance;
    public int penetration;
    public int strength;
    public int perception;
    public int intelligence;
    public int agility;
    public int charisma;
    public int willpower;
    public List<AbilityReference> abilities;
}

[Serializable]
public class AbilityReference
{
    public int index;
    public string name;
}


public class FightManager : MonoBehaviour
{
    [Serializable]
    private class BestiaryDataJsonArray
    {
        public List<BestiaryDataJson> creatures;
    }

    [SerializeField] Transform PlayerFrontLine;
    [SerializeField] Transform PlayerBackLine;
    [SerializeField] Transform EnemyFrontLine;
    [SerializeField] Transform EnemyBackLine;
    [SerializeField] Transform Formations;
    [SerializeField] Transform ActionAbilitiesList;
    [SerializeField] Transform LootField;
    [SerializeField] Transform CombatLogList;
    [SerializeField] GameObject TeamSetupPanel;
    [SerializeField] GameObject CombatantTemplate;
    [SerializeField] GameObject CombatAbilityTemplate;
    [SerializeField] GameObject StatusEffectTemplate;
    [SerializeField] GameObject FlyingMessageTemplate;
    [SerializeField] GameObject StartBattleButton;
    [SerializeField] GameObject Scoreboard;
    [SerializeField] GameObject AnimatedTopHud;
    [SerializeField] List<GameObject> DungeonLevels;
    [SerializeField] TextMeshProUGUI AvailableCombatantsCount;
    [SerializeField] TextMeshProUGUI EventLevel;
    [SerializeField] TextMeshProUGUI DungeonUIname;

    private readonly IList<GameObject> combatants = new List<GameObject>();
    private readonly IList<string> formationStyles = new List<string> { "FF", "FB", "BB" };
    private string DungeonName;

    private int TotalDungeonLevels;
    private int CurrentDungeonLevel;
    private List<BestiaryDataJson> bestiaryDataList;
    private TranslationManager translationManager;

    public IReadOnlyList<GameObject> Combatants => combatants.ToList().AsReadOnly();
    public GameObject ActionPanel;
    public GameObject[] MarkObjects;
    public GameObject[] DragAndDropIndicators;
    public TargetService TargetService;
    public LootService LootService;

    [HideInInspector] public GameObject EnemyTarget = null;
    [HideInInspector] public List<GameObject> PlayerGroupTargets = new();
    [HideInInspector] public GameObject EnemiesTarget = null;
    [HideInInspector] public List<GameObject> EnemyGroupTargets = new();
    [HideInInspector] public GameObject ActiveCombatant = null;
    [HideInInspector] public CombatAbility ActiveAbility = null;
    [HideInInspector] public EventSize EventSize;
    [HideInInspector] public EventIconType DungeonType;
    [HideInInspector] public bool IsBattleRunning;
    [HideInInspector] public bool IsAbilitySelected;
    [HideInInspector] public bool IsPerformingAbility;
    [HideInInspector] public bool IsCounterAttacking;
    [HideInInspector] public bool IsVictory;

    void Awake()
    {
        try
        {
            translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
            ItemCreator itemCreator = GameObject.Find("ItemCreatorList").GetComponent<ItemCreator>();
            TargetService = new TargetService(this);

            string jsonText = Assets.Scripts.Models.BestiaryJson.json;
            BestiaryDataJsonArray itemDataArray = JsonUtility.FromJson<BestiaryDataJsonArray>(jsonText);
            if (itemDataArray != null)
            {
                bestiaryDataList = itemDataArray.creatures;
            }

            if (itemCreator != null)
            {
                LootService = new LootService(this, itemCreator, LootField);
            }
            else
            {
                Debug.LogError("ItemCreator not found!");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public async UniTask CreateFlyingMessage(GameObject targetCombatant, StatusEffect statusEffect = null)
    {
        GameObject flyingMessage = Instantiate(FlyingMessageTemplate, targetCombatant.transform);

        if (statusEffect != null)
        {
            flyingMessage.transform.Find("Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignCombatSpriteToSlot(statusEffect.name);
            flyingMessage.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Resisted");
        }
        else
        {
            flyingMessage.transform.Find("Image").gameObject.SetActive(false);
            flyingMessage.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("CounterAttack");
        }

        flyingMessage.transform.localPosition = new Vector3(0, 150, 0);
        await MoveFlyingMessage(flyingMessage, 2, 20);
        Destroy(flyingMessage);
    }

    private async UniTask MoveFlyingMessage(GameObject flyingMessage, float duration, float distance)
    {
        Vector3 startPosition = flyingMessage.transform.localPosition;
        Vector3 endPosition = flyingMessage.transform.localPosition + new Vector3(0f, distance, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            flyingMessage.transform.localPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    public void CreateCombatMessage(string message)
    {
        GameObject gameObject = new();
        gameObject.transform.SetParent(CombatLogList, false);
        gameObject.AddComponent<RectTransform>();
        var textMesh = gameObject.AddComponent<TextMeshProUGUI>();
        textMesh.fontSize = 18;
        textMesh.text = message;
    }

    public void StartAbility(GameObject startingAbility)
    {
        TargetService.HighlightPossibleTargets(ActiveCombatant, startingAbility.name);
    }

    public void ClearCombatAbilities()
    {
        if (ActionAbilitiesList.childCount > 0)
        {
            foreach (Transform child in ActionAbilitiesList)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void FillCombatAbilities(IList<CombatAbility> combatAbilities)
    {
        foreach (var ability in combatAbilities)
        {
            GameObject newAbility = Instantiate(CombatAbilityTemplate, ActionAbilitiesList);

            newAbility.name = ability.Name;
            newAbility.transform.Find("Icon/Image").GetComponent<Image>().sprite = AssetBundleManager.AssignCombatSpriteToSlot(ability.Weapon);
            newAbility.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(ability.Name);
            //newAbility.transform.Find("AttackType").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(ability.Type);

            if (ability.Cooldown > 0)
            {
                newAbility.transform.Find("Icon/Cooldown/Value").GetComponent<TextMeshProUGUI>().text = ability.Cooldown.ToString();
            }
            else
            {
                newAbility.transform.Find("Icon/Cooldown").gameObject.SetActive(false);
            }

            if (ability.AbilityLowerDamage > 0)
            {
                newAbility.transform.Find("Damage/DamageBckg/Damage/Value").GetComponent<TextMeshProUGUI>().text = $"{ability.AbilityLowerDamage}-{ability.AbilityHigherDamage}";
            }
            else
            {
                newAbility.transform.Find("Damage").gameObject.SetActive(false);
            }

            if (ability.IsFrontlineAoe && ability.IsBacklineAoe)
            {
                newAbility.transform.Find("AOE/Value").GetComponent<TextMeshProUGUI>().text = "FB";
            }
            else if (ability.IsFrontlineAoe)
            {
                newAbility.transform.Find("AOE/Value").GetComponent<TextMeshProUGUI>().text = "F";
            }
            else if (ability.IsBacklineAoe)
            {
                newAbility.transform.Find("AOE/Value").GetComponent<TextMeshProUGUI>().text = "B";
            }
            else
            {
                newAbility.transform.Find("AOE").gameObject.SetActive(false);
            }

            if (ability.PositiveStatusEffects.Count > 0)
            {
                for (int i = 0; i < ability.PositiveStatusEffects.Count; i++)
                {
                    if (i == 0)
                    {
                        var statusEffect = ability.PositiveStatusEffects[i];

                        if (statusEffect != null)
                        {
                            if (!statusEffect.isFrontLineAoe && !statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Positive/AOEeffect").gameObject.SetActive(false);
                            }
                            else if (statusEffect.isFrontLineAoe && statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Positive/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "FB";
                            }
                            else if (statusEffect.isFrontLineAoe)
                            {
                                newAbility.transform.Find("Stats/Positive/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "F";
                            }
                            else if (statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Positive/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "B";
                            }

                            newAbility.transform.Find("Stats/Positive/StatusEffect/Value").GetComponent<TextMeshProUGUI>().text = statusEffect.chance.ToString();
                            newAbility.transform.Find("Stats/Positive/StatusEffect/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignCombatSpriteToSlot(statusEffect.name);
                        }
                    }
                    if (i == 1)
                    {
                        var statusEffect = ability.PositiveStatusEffects[i];

                        if (statusEffect != null)
                        {
                            if (!statusEffect.isFrontLineAoe && !statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Positive2/AOEeffect").gameObject.SetActive(false);
                            }
                            else if (statusEffect.isFrontLineAoe && statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Positive2/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "FB";
                            }
                            else if (statusEffect.isFrontLineAoe)
                            {
                                newAbility.transform.Find("Stats/Positive2/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "F";
                            }
                            else if (statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Positive2/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "B";
                            }

                            newAbility.transform.Find("Stats/Positive2/StatusEffect/Value").GetComponent<TextMeshProUGUI>().text = statusEffect.chance.ToString();
                            newAbility.transform.Find("Stats/Positive2/StatusEffect/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignCombatSpriteToSlot(statusEffect.name);
                        }
                    }
                    else
                    {
                        newAbility.transform.Find("Stats/Positive2").gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                newAbility.transform.Find("Stats/Positive").gameObject.SetActive(false);
                newAbility.transform.Find("Stats/Positive2").gameObject.SetActive(false);
            }

            if (ability.NegativeStatusEffects.Count > 0)
            {
                for (int i = 0; i < ability.NegativeStatusEffects.Count; i++)
                {
                    if (i == 0)
                    {
                        var statusEffect = ability.NegativeStatusEffects[i];

                        if (statusEffect != null)
                        {
                            if (!statusEffect.isFrontLineAoe && !statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Negative/AOEeffect").gameObject.SetActive(false);
                            }
                            else if (statusEffect.isFrontLineAoe && statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Negative/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "FB";
                            }
                            else if (statusEffect.isFrontLineAoe)
                            {
                                newAbility.transform.Find("Stats/Negative/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "F";
                            }
                            else if (statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Negative/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "B";
                            }

                            newAbility.transform.Find("Stats/Negative/StatusEffect/Value").GetComponent<TextMeshProUGUI>().text = statusEffect.chance.ToString();
                            newAbility.transform.Find("Stats/Negative/StatusEffect/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignCombatSpriteToSlot(statusEffect.name);
                        }
                    }
                    if (i == 1)
                    {
                        var statusEffect = ability.NegativeStatusEffects[i];

                        if (statusEffect != null)
                        {
                            if (!statusEffect.isFrontLineAoe && !statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Negative2/AOEeffect").gameObject.SetActive(false);
                            }
                            else if (statusEffect.isFrontLineAoe && statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Negative2/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "FB";
                            }
                            else if (statusEffect.isFrontLineAoe)
                            {
                                newAbility.transform.Find("Stats/Negative2/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "F";
                            }
                            else if (statusEffect.isBackLineAoe)
                            {
                                newAbility.transform.Find("Stats/Negative2/AOEeffect/Value").GetComponent<TextMeshProUGUI>().text = "B";
                            }

                            newAbility.transform.Find("Stats/Negative2/StatusEffect/Value").GetComponent<TextMeshProUGUI>().text = statusEffect.chance.ToString();
                            newAbility.transform.Find("Stats/Negative2/StatusEffect/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignCombatSpriteToSlot(statusEffect.name);
                        }
                    }
                    else
                    {
                        newAbility.transform.Find("Stats/Negative2").gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                newAbility.transform.Find("Stats/Negative").gameObject.SetActive(false);
                newAbility.transform.Find("Stats/Negative2").gameObject.SetActive(false);
            }

            if (ability.PositiveStatusEffects.Count == 0 && ability.NegativeStatusEffects.Count == 0)
            {
                newAbility.transform.Find("Stats").gameObject.SetActive(false);
            }

            if (ability.IsAbilityOnCooldown())
            {
                newAbility.transform.GetComponent<Button>().interactable = false;
                newAbility.transform.Find("CooldownBlock/Value").GetComponent<TextMeshProUGUI>().text = ability.RemainingCooldown.ToString();
                newAbility.transform.Find("CooldownBlock/Timebar").GetComponent<Image>().fillAmount = (float)(ability.Cooldown - ability.RemainingCooldown) / ability.Cooldown;
            }
            else
            {
                newAbility.transform.GetComponent<Button>().interactable = true;
                newAbility.transform.Find("CooldownBlock").gameObject.SetActive(false);
            }
        }
    }

    public GameObject GetStatusEffectTemplate()
    {
        return StatusEffectTemplate;
    }
    public string GetDungeonName()
    {
        return DungeonName;
    }

    public int GetCurrentDungeonLevel()
    {
        return CurrentDungeonLevel;
    }

    public int ModifyDungeonLevel(int value)
    {
        return CurrentDungeonLevel = value;
    }

    public void StartPreparationPhase(GameObject startingObject)
    {
        try
        {
            IsVictory = false;
            IsPerformingAbility = false;
            IsAbilitySelected = false;
            TeamSetupPanel.SetActive(true);
            ActionPanel.SetActive(false);
            IsBattleRunning = false;
            AvailableCombatantsCount.text = Player.PassiveCombatants.ToString();
            EventLevel.text = startingObject.transform.Find("Info/Level").GetComponent<TextMeshProUGUI>().text;
            DungeonName = startingObject.transform.Find("DungeonName").GetChild(0).name;
            DungeonUIname.text = translationManager.Translate(DungeonName);
            CurrentDungeonLevel = 1;
            Player.InCombat = true;

            if (EventSize == EventSize.Small)
            {
                TotalDungeonLevels = UnityEngine.Random.Range(1, 2);
            }
            else if (EventSize == EventSize.Medium)
            {
                TotalDungeonLevels = UnityEngine.Random.Range(2, 4);
            }
            else if (EventSize == EventSize.Large)
            {
                TotalDungeonLevels = UnityEngine.Random.Range(4, 6);
            }

            for (int i = 0; i < TotalDungeonLevels; i++)
            {
                DungeonLevels[i].SetActive(true);
            }

            DisplayDungeonLevel();

            SpawnEnemyGroup(EnemyGroupSize.One);
            SpawnPlayerFirstTime();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void StartBattle()
    {
        try
        {
            IsBattleRunning = true;
            TeamSetupPanel.SetActive(false);
            ActionPanel.SetActive(true);
            StartBattleButton.SetActive(false);

            StartTimebars();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void StopTimebars()
    {
        if (Combatants.Count > 0)
        {
            foreach (var combatant in Combatants)
            {
                if (combatant.TryGetComponent<BattleCharacter>(out var battleCharacter))
                {
                    battleCharacter.StopCurrentTask();
                }
            }
        }
    }

    /// <summary>
    /// We need to check if the attacker is facing only stealthed enemies or not. Because if there are only
    /// invisible enemies, they can't be targeted.
    /// </summary>
    /// <returns></returns>
    public bool EnemyGroupInvisible()
    {
        if (Combatants.Count > 0)
        {
            foreach (var combatant in Combatants)
            {
                if (combatant.TryGetComponent(out BattleCharacter battleCharacter) &&
                    battleCharacter.IsEnemy())
                {
                    if (!battleCharacter.IsInvisible())
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public bool PlayerGroupInvisible()
    {
        if (Combatants.Count > 0)
        {
            foreach (var combatant in Combatants)
            {
                if (combatant.TryGetComponent(out BattleCharacter battleCharacter) &&
                    !battleCharacter.IsEnemy())
                {
                    if (!battleCharacter.IsInvisible())
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public void ClearLoot()
    {
        LootService.ClearLoot();
    }

    public async UniTask Victory()
    {
        IsVictory = true;
        IsPerformingAbility = false;
        IsAbilitySelected = false;
        Scoreboard.SetActive(true);
        Scoreboard.GetComponent<Animator>().Play("Victory");
        await UniTask.WaitForSeconds(1);
        LootService.CreateRandomLoot();
        Player.InCombat = false;
        RemoveAllCombatants();
    }

    public void Defeat()
    {
        IsVictory = false;
        IsPerformingAbility = false;
        IsAbilitySelected = false;
        Scoreboard.SetActive(true);
        Scoreboard.GetComponent<Animator>().Play("Defeat");
        Player.InCombat = false;
        RemoveAllCombatants();
    }

    public int CheckActiveCombatants(bool isEnemyGroup)
    {
        int activeCombatants = 0;

        foreach (var combatant in combatants)
        {
            if (isEnemyGroup)
            {
                if (combatant.TryGetComponent(out BattleCharacter battleCharacter) &&
                    battleCharacter.IsEnemy())
                {
                    activeCombatants++;
                }
            }
            else
            {
                if (combatant.TryGetComponent(out BattleCharacter battleCharacter) &&
                    !battleCharacter.IsEnemy())
                {
                    activeCombatants++;
                }
            }
        }

        return activeCombatants;
    }


    public void StartTimebars()
    {
        if (Combatants.Count > 0)
        {
            foreach (var combatant in Combatants)
            {
                if (combatant.TryGetComponent<BattleCharacter>(out var battleCharacter) &&
                    !battleCharacter.IsCombatantDead())
                {
                    _ = battleCharacter.StartTimebar();
                }
            }
        }
        else
        {
            Debug.LogError("Combatants count is 0!");
        }
    }

    private void DisplayDungeonLevel()
    {
        GameObject levelMarker = new();
        var rectTransform = levelMarker.AddComponent<RectTransform>();
        var imageComponent = levelMarker.AddComponent<Image>();
        imageComponent.sprite = AssetBundleManager.AssignMiscSpriteToSlot("CircleGlow");
        imageComponent.color = UIColors.NeonBlueOriginalFull;
        rectTransform.sizeDelta = new Vector2(24, 24);

        levelMarker.name = Constants.LevelMark;

        foreach (Transform child in AnimatedTopHud.transform)
        {
            if (child.name == CurrentDungeonLevel.ToString())
            {
                levelMarker.transform.SetParent(child, false);
            }
        }
    }

    private void RemoveCurrentDisplayedDungeonLevel()
    {
        AnimatedTopHud.transform.Cast<Transform>()
                                .Where(child => child.name == CurrentDungeonLevel.ToString())
                                .ToList()
                                .ForEach(child =>
                                {
                                    var nestedChild = child.Find(Constants.LevelMark);
                                    if (nestedChild != null)
                                    {
                                        Destroy(nestedChild.gameObject);
                                    }
                                });
    }

    private void AddCombatant(GameObject combatant)
    {
        combatants.Add(combatant);
    }

    private void RemoveCombatant(GameObject combatant)
    {
        combatants.Remove(combatant);
    }

    private void RemoveAllCombatants()
    {
        if (Combatants.Count > 0)
        {
            foreach (var combatant in Combatants)
            {
                Destroy(combatant);
            }
        }
    }

    /// <summary>
    /// Spawns a group of random enemy combatants based on the type of dungeon.
    /// Group size has to be from 1 to 6 max.
    /// Can spawn either front line or back line online or mixed combatants.
    /// </summary>
    /// <param name="groupSize"></param>
    private protected void SpawnEnemyGroup(EnemyGroupSize groupSize)
    {
        var creaturesList = bestiaryDataList.Where(type => type.location == DungeonType)
                                            .Where(diff => diff.tier == int.Parse(EventLevel.text.Substring(1, 1)))
                                            .ToList();

        var creaturesListTestFrontline = bestiaryDataList.Where(type => type.location == DungeonType)
                            .Where(diff => diff.tier == 2)
                            .Where(formation => formation.battleFormation == BattleFormation.Front)
                            .ToList();

        var creaturesListTestBackline = bestiaryDataList.Where(type => type.location == DungeonType)
                                    .Where(diff => diff.tier == 2)
                                    .Where(formation => formation.battleFormation == BattleFormation.Back)
                                    .ToList();

        var frontLineCreaturesList = bestiaryDataList.Where(type => type.location == DungeonType)
                                            .Where(diff => diff.tier == int.Parse(EventLevel.text.Substring(1, 1)))
                                            .Where(formation => formation.battleFormation == BattleFormation.Front)
                                            .ToList();

        var backLineCreaturesList = bestiaryDataList.Where(type => type.location == DungeonType)
                                    .Where(diff => diff.tier == int.Parse(EventLevel.text.Substring(1, 1)))
                                    .Where(formation => formation.battleFormation == BattleFormation.Back)
                                    .ToList();

        var randomStyleIndex = UnityEngine.Random.Range(0, formationStyles.Count);
        var randomStyle = formationStyles[randomStyleIndex];

        if (frontLineCreaturesList.Count > 0 && backLineCreaturesList.Count > 0 && creaturesList.Count > 0)
        {
            if (groupSize == EnemyGroupSize.One)
            {
                SpawnRandomEnemy(creaturesListTestBackline, "2", BattleFormation.Front);
            }
            else if (groupSize == EnemyGroupSize.Two)
            {
                if (frontLineCreaturesList.Count > 0 && backLineCreaturesList.Count > 0)
                {
                    var randomCombination = UnityEngine.Random.Range(0, 2);

                    if (randomStyle == "FB")
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                    }
                    else if (randomStyle == "FF")
                    {
                        if (randomCombination == 0)
                        {
                            SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                            SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                        }
                        else
                        {
                            SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                            SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                        }

                    }
                    else if (randomStyle == "BB")
                    {
                        if (randomCombination == 0)
                        {
                            SpawnRandomEnemy(backLineCreaturesList, "2", BattleFormation.Front);
                            SpawnRandomEnemy(backLineCreaturesList, "1", BattleFormation.Front);
                        }
                        else
                        {
                            SpawnRandomEnemy(backLineCreaturesList, "2", BattleFormation.Front);
                            SpawnRandomEnemy(backLineCreaturesList, "3", BattleFormation.Front);
                        }
                    }
                }
            }
            else if (groupSize == EnemyGroupSize.Three)
            {
                if (frontLineCreaturesList.Count > 0 && backLineCreaturesList.Count > 0)
                {
                    if (randomStyle == "FB")
                    {
                        var randomCombination = UnityEngine.Random.Range(0, 2);

                        if (randomCombination == 0)
                        {
                            SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                            SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                            SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                        }
                        else
                        {
                            SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                            SpawnRandomEnemy(backLineCreaturesList, "4", BattleFormation.Back);
                            SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                        }
                    }
                    else if (randomStyle == "FF")
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                    }
                    else if (randomStyle == "BB")
                    {
                        SpawnRandomEnemy(backLineCreaturesList, "3", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "1", BattleFormation.Front);
                    }
                }
            }
            else if (groupSize == EnemyGroupSize.Four)
            {
                if (randomStyle == "FB")
                {
                    var randomCombination = UnityEngine.Random.Range(0, 2);

                    if (randomCombination == 0)
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "4", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                    }
                    else
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "6", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                    }
                }
                else if (randomStyle == "FF")
                {
                    SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                    SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                    SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                    SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                }
                else if (randomStyle == "BB")
                {
                    SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                    SpawnRandomEnemy(backLineCreaturesList, "4", BattleFormation.Back);
                    SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                    SpawnRandomEnemy(backLineCreaturesList, "6", BattleFormation.Back);
                }
            }
            else if (groupSize == EnemyGroupSize.Five)
            {
                var randomCombination = UnityEngine.Random.Range(0, 2);

                if (randomStyle == "FB")
                {
                    if (randomCombination == 0)
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "4", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                    }
                    else
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "6", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "4", BattleFormation.Back);
                    }
                }
                else if (randomStyle == "FF")
                {
                    if (randomCombination == 0)
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "4", BattleFormation.Back);
                    }
                    else
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "6", BattleFormation.Back);
                    }
                }
                else if (randomStyle == "BB")
                {
                    if (randomCombination == 0)
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "4", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "6", BattleFormation.Back);
                    }
                    else
                    {
                        SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                        SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                        SpawnRandomEnemy(backLineCreaturesList, "4", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                        SpawnRandomEnemy(backLineCreaturesList, "6", BattleFormation.Back);
                    }
                }
            }
            else
            {
                SpawnRandomEnemy(frontLineCreaturesList, "3", BattleFormation.Front);
                SpawnRandomEnemy(frontLineCreaturesList, "2", BattleFormation.Front);
                SpawnRandomEnemy(frontLineCreaturesList, "1", BattleFormation.Front);
                SpawnRandomEnemy(backLineCreaturesList, "4", BattleFormation.Back);
                SpawnRandomEnemy(backLineCreaturesList, "5", BattleFormation.Back);
                SpawnRandomEnemy(backLineCreaturesList, "6", BattleFormation.Back);
            }
        }
    }

    /// <summary>
    /// Spawns a random creature from a list of creatures based on its formation and on a specific index [1-6] where
    /// 1-3 is front line and 4-6 is back line spots.
    /// </summary>
    /// <param name="creatureList"></param>
    /// <param name="index"></param>
    /// <param name="formation"></param>
    private protected void SpawnRandomEnemy(List<BestiaryDataJson> creatureList, string index, BattleFormation formation)
    {
        var randomIndex = UnityEngine.Random.Range(0, creatureList.Count);
        var randomCreature = creatureList[randomIndex];
        SpawnEnemy(index, formation, randomCreature);
    }

    private protected void SpawnEnemy(string slotID, BattleFormation battleFormation, BestiaryDataJson creatureData)
    {
        GameObject newItem;

        if (battleFormation == BattleFormation.Front)
        {
            newItem = Instantiate(CombatantTemplate, EnemyFrontLine.Find(slotID).transform);
        }
        else
        {
            newItem = Instantiate(CombatantTemplate, EnemyBackLine.Find(slotID).transform);
        }

        newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);
        newItem.transform.localScale = Vector3.one;

        if (newItem.TryGetComponent<BattleCharacter>(out var battleCharacter) &&
            int.TryParse(slotID, out int position) && int.TryParse(EventLevel.text.Substring(1, 1), out int level))
        {
            battleCharacter.CreateCombatant(battleFormation, Faction.Enemy, creatureData.race, position, creatureData.name, level, creatureData);
            AddCombatant(newItem);
        }
    }

    private protected void SpawnPlayerFirstTime()
    {
        GameObject newItem = Instantiate(CombatantTemplate, PlayerFrontLine.Find("2").transform);
        newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);
        newItem.transform.localScale = Vector3.one;

        if (newItem.TryGetComponent<BattleCharacter>(out var battleCharacter))
        {
            battleCharacter.CreateCombatant(BattleFormation.Front, Faction.Player, Race.Humanoid, 1, Player.Name, Player.Level);
            AddCombatant(newItem);
        }
    }
}
