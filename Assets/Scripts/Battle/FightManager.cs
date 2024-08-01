using Cysharp.Threading.Tasks;
using ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enumerations;

public class FightManager : MonoBehaviour
{
    [SerializeField] Transform PlayerFrontLine;
    [SerializeField] Transform PlayerBackLine;
    [SerializeField] Transform EnemyFrontLine;
    [SerializeField] Transform EnemyBackLine;
    [SerializeField] Transform ActionAbilitiesList;
    [SerializeField] Transform LootField;
    [SerializeField] Transform CombatLogList;
    [SerializeField] GameObject TeamSetupPanel;
    [SerializeField] GameObject CombatantTemplate;
    [SerializeField] GameObject CombatAbilityTemplate;
    [SerializeField] GameObject StartBattleButton;
    [SerializeField] GameObject Scoreboard;
    [SerializeField] GameObject AnimatedTopHud;
    [SerializeField] List<GameObject> DungeonLevels;
    [SerializeField] TextMeshProUGUI AvailableCombatantsCount;
    [SerializeField] TextMeshProUGUI EventLevel;

    private readonly IList<GameObject> combatants = new List<GameObject>();
    private string DungeonName;

    private int TotalDungeonLevels;
    private int CurrentDungeonLevel;
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
    [HideInInspector] public bool IsBattleRunning;
    [HideInInspector] public bool IsAbilitySelected;
    [HideInInspector] public bool IsPerformingAbility;
    [HideInInspector] public bool IsVictory;

    void Awake()
    {
        try
        {
            translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
            ItemCreator itemCreator = GameObject.Find("ItemCreatorList").GetComponent<ItemCreator>();
            TargetService = new TargetService(this);

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

    public void CreateCombatMessage(string message)
    {
        GameObject gameObject = new();
        gameObject.AddComponent<RectTransform>();
        var textMesh = gameObject.AddComponent<TextMeshProUGUI>();
        textMesh.fontSize = 18;
        textMesh.text = message;
        Instantiate(gameObject, CombatLogList);
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

                            newAbility.transform.Find("Stats/Positive/StatusEffect/Value").GetComponent<TextMeshProUGUI>().text = statusEffect.value.ToString();
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

                            newAbility.transform.Find("Stats/Positive2/StatusEffect/Value").GetComponent<TextMeshProUGUI>().text = statusEffect.value.ToString();
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

                            newAbility.transform.Find("Stats/Negative/StatusEffect/Value").GetComponent<TextMeshProUGUI>().text = statusEffect.value.ToString();
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

                            newAbility.transform.Find("Stats/Negative2/StatusEffect/Value").GetComponent<TextMeshProUGUI>().text = statusEffect.value.ToString();
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
        }
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
            CurrentDungeonLevel = 1;

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

            SpawnEnemy("2", BattleFormation.Front, Race.Beast);
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
        RemoveAllCombatants();
    }

    public void Defeat()
    {
        IsVictory = false;
        IsPerformingAbility = false;
        IsAbilitySelected = false;
        Scoreboard.SetActive(true);
        Scoreboard.GetComponent<Animator>().Play("Defeat");
        RemoveAllCombatants();
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
                Instantiate(levelMarker, child);
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

    private protected void SpawnEnemy(string slotID, BattleFormation battleFormation, Race race)
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
            int.TryParse(slotID, out int position) && int.TryParse(EventLevel.text, out int level))
        {
            battleCharacter.CreateCombatant(battleFormation, Faction.Enemy, race, position, "FireBeetle", level);
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
