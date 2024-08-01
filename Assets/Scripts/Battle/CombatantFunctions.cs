using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatantFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private GameObject cloneObject;
    private GameObject highlightedObject;
    private GameObject previousHighlightObject;
    private FightManager fightManager;
    [SerializeField] GameObject highestObject;

    void Awake()
    {
        fightManager = GameObject.Find("FIGHTMANAGER").GetComponent<FightManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.TryGetComponent<BattleCharacter>(out var battleCharacter))
        {
            if (battleCharacter.IsEnemy() && !fightManager.IsPerformingAbility)
            {
                battleCharacter.TargetCombatant();

                if (fightManager.IsAbilitySelected)
                {
                    AttackEnemyTarget(false);
                }
            }
        }
        else
        {
            Debug.LogError("BattleCharacter component not found!");
        }
    }

    public void AttackAllyTarget(bool isAoe)
    {
        if (fightManager.ActiveCombatant != null && fightManager.EnemiesTarget != null)
        {
            _ = MoveCombatantAsync(false, isAoe);
        }
        else
        {
            Debug.LogError("ActiveCombatant or EnemiesTarget is null!");
        }
    }

    public void AttackEnemyTarget(bool isAoe)
    {
        if (fightManager.ActiveCombatant != null)
        {
            if (!isAoe && fightManager.EnemyTarget != null &&
                fightManager.EnemyTarget.TryGetComponent<BattleCharacter>(out var targetCharacter))
            {
                targetCharacter.StopBlinkingCombatant();

            }

            _ = MoveCombatantAsync(true, isAoe);
        }
        else
        {
            Debug.LogError("ActiveCombatant or EnemyTarget is null!");
        }
    }

    public void CalculateAbilityDamage(GameObject targetCombatant)
    {
        if (fightManager.ActiveAbility != null)
        {
            if (targetCombatant.TryGetComponent<BattleCharacter>(out var targetCharacter))
            {
                var rawDamage = UnityEngine.Random.Range(fightManager.ActiveAbility.AbilityLowerDamage, fightManager.ActiveAbility.AbilityHigherDamage);
                int damage = (int)Math.Round(rawDamage);
                targetCharacter.ReceiveDamage(damage);

                targetCombatant.transform.Find("CharacterRow/Damage/Value").GetComponent<TextMeshProUGUI>().text = damage.ToString();

                if (fightManager.ActiveCombatant != null && fightManager.ActiveCombatant.TryGetComponent<BattleCharacter>(out var activeChar))
                {
                    fightManager.CreateCombatMessage($"<color=#63ECFF>{activeChar.GetCombatantName()}</color> uses <color=yellow>{fightManager.ActiveAbility.Name}</color> and hits enemies for <color=yellow>{damage} damage</color>.");
                }
            }
        }
        else
        {
            Debug.LogError("ActiveAbility is null!");
        }
    }

    public async UniTask MoveCombatantAsync(bool isPlayerGroup, bool isAoe)
    {
        if (fightManager.ActiveCombatant != null)
        {
            fightManager.IsAbilitySelected = false;
            fightManager.IsPerformingAbility = true;
            fightManager.TargetService.ClearAllHighlights();

            if (isPlayerGroup)
            {
                if (!isAoe && fightManager.EnemyTarget != null)
                {
                    await CountTimer(fightManager.ActiveCombatant, fightManager.EnemyTarget, isPlayerGroup, fightManager.PlayerGroupTargets);
                    CalculateAbilityDamage(fightManager.EnemyTarget);

                    await UniTask.WaitForSeconds(1);

                    if (fightManager.EnemyTarget.TryGetComponent<BattleCharacter>(out var targetCharacter))
                    {
                        if (targetCharacter.IsCombatantDead())
                        {
                            fightManager.EnemyTarget.GetComponent<Animator>().Play(Constants.Death);
                            await UniTask.WaitForSeconds(1);
                        }
                    }
                }
                else if (isAoe && fightManager.PlayerGroupTargets.Count > 0)
                {
                    await CountTimer(fightManager.ActiveCombatant, fightManager.EnemyTarget, isPlayerGroup, fightManager.PlayerGroupTargets);

                    foreach (var target in fightManager.PlayerGroupTargets)
                    {
                        CalculateAbilityDamage(target);
                    }

                    await UniTask.WaitForSeconds(1);

                    foreach (var target in fightManager.PlayerGroupTargets)
                    {
                        if (target.TryGetComponent<BattleCharacter>(out var targetCharacter))
                        {
                            if (targetCharacter.IsCombatantDead())
                            {
                                target.GetComponent<Animator>().Play(Constants.Death);
                                await UniTask.WaitForSeconds(1);
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("EnemyTarget is null!");
                }
            }
            else
            {
                if (fightManager.EnemiesTarget != null)
                {
                    await CountTimer(fightManager.ActiveCombatant, fightManager.EnemiesTarget, isPlayerGroup, fightManager.EnemyGroupTargets);
                    CalculateAbilityDamage(fightManager.EnemiesTarget);

                    await UniTask.WaitForSeconds(1);

                    if (fightManager.EnemiesTarget.TryGetComponent<BattleCharacter>(out var targetCharacter))
                    {
                        if (targetCharacter.IsCombatantDead())
                        {
                            fightManager.EnemiesTarget.GetComponent<Animator>().Play(Constants.Death);
                            await UniTask.WaitForSeconds(1);
                        }
                    }
                }
                else
                {
                    Debug.LogError("EnemiesTarget is null!");
                }
            }

            bool allEnemiesDead = true;

            foreach (var combatant in fightManager.Combatants)
            {
                combatant.TryGetComponent<BattleCharacter>(out var combatantCharacter);
                {
                    if (combatantCharacter.IsEnemy() && !combatantCharacter.IsCombatantDead())
                    {
                        allEnemiesDead = false;
                    }
                }
            }

            bool allAlliesDead = true;

            foreach (var combatant in fightManager.Combatants)
            {
                combatant.TryGetComponent<BattleCharacter>(out var combatantCharacter);
                {
                    if (!combatantCharacter.IsEnemy() && !combatantCharacter.IsCombatantDead())
                    {
                        allAlliesDead = false;
                    }
                }
            }

            if (allEnemiesDead)
            {
                _ = fightManager.Victory();
            }
            else if (allAlliesDead)
            {
                fightManager.Defeat();
            }
            else
            {
                fightManager.StartTimebars();
            }

            fightManager.IsPerformingAbility = false;
            await UniTask.CompletedTask;
        }
        else
        {
            Debug.LogError("ActiveCombatant or EnemyTarget attributes are null!");
        }
    }

    private async UniTask CountTimer(GameObject activeCombatant, GameObject targetCombatant, bool isPlayerGroup, List<GameObject> targetCombatants)
    {
        float movingTime = 0.2f;
        float elapsed = 0f;

        float originalStarterX = activeCombatant.transform.position.x;
        float originalStarterY = activeCombatant.transform.position.y;

        float starterX = activeCombatant.transform.position.x;
        float starterY = activeCombatant.transform.position.y;

        float targetX = 0;
        float targetY = 0;

        if (targetCombatants.Count > 0)
        {
            foreach (var combatant in targetCombatants)
            {
                if (combatant.TryGetComponent<BattleCharacter>(out var combatantChar))
                {
                    if (combatantChar.GetCombatantPosition() == 2)
                    {
                        targetX = combatant.transform.position.x;

                        if (isPlayerGroup)
                        {
                            targetY = combatant.transform.position.y * 0.7f;
                        }
                        else
                        {
                            targetY = combatant.transform.position.y / 0.7f;
                        }
                    }
                }
            }
        }
        else
        {
            targetX = targetCombatant.transform.position.x;

            if (isPlayerGroup)
            {
                targetY = targetCombatant.transform.position.y * 0.7f;
            }
            else
            {
                targetY = targetCombatant.transform.position.y / 0.7f;
            }
        }

        Vector2 vector;

        while (elapsed < movingTime)
        {
            elapsed += Time.deltaTime;
            float newX = Mathf.Lerp(starterX, targetX, elapsed / movingTime);
            float newY = Mathf.Lerp(starterY, targetY, elapsed / movingTime);
            vector = new(newX, newY);
            activeCombatant.transform.position = vector;
            await UniTask.Yield();
        }

        elapsed = 0f;

        if (targetCombatants.Count > 0)
        {
            foreach (var combatant in targetCombatants)
            {
                combatant.GetComponent<Animator>().Play(Constants.Damage);
            }
        }
        else
        {
            targetCombatant.GetComponent<Animator>().Play(Constants.Damage);
        }

        while (elapsed < movingTime)
        {
            elapsed += Time.deltaTime;
            float newX = Mathf.Lerp(starterX, originalStarterX, elapsed / movingTime);
            float newY = Mathf.Lerp(starterY, originalStarterY, elapsed / movingTime);
            vector = new(newX, newY);
            activeCombatant.transform.position = vector;
            await UniTask.Yield();
        }

        if (activeCombatant.TryGetComponent<BattleCharacter>(out var character))
        {
            character.ClearCombatantTimebar();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.TryGetComponent<BattleCharacter>(out var battleCharacter) && !battleCharacter.IsEnemy() &&
            !fightManager.IsBattleRunning)
        {
            gameObject.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.NeonBlueOriginalHalf;
        }
        else if (gameObject.TryGetComponent<BattleCharacter>(out var enemyCharacter) && enemyCharacter.IsEnemy() &&
                 fightManager.IsBattleRunning)
        {
            gameObject.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.NeonRedHalf;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.TryGetComponent<BattleCharacter>(out var battleCharacter) && !battleCharacter.IsEnemy() &&
            !fightManager.IsBattleRunning)
        {
            gameObject.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.invisibleCol;
        }
        else if (gameObject.TryGetComponent<BattleCharacter>(out var enemyCharacter) && enemyCharacter.IsEnemy() &&
                 fightManager.IsBattleRunning)
        {
            gameObject.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.invisibleCol;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameObject.TryGetComponent<BattleCharacter>(out var battleCharacter) && !battleCharacter.IsEnemy())
        {
            cloneObject = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent);
            cloneObject.transform.SetParent(highestObject.transform);
            cloneObject.AddComponent<CanvasGroup>();
            cloneObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

            foreach (var indicatorObject in fightManager.DragAndDropIndicators)
            {
                indicatorObject.SetActive(true);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject.TryGetComponent<BattleCharacter>(out var battleCharacter) && !battleCharacter.IsEnemy() &&
            !fightManager.IsBattleRunning)
        {
            cloneObject.transform.position = eventData.position;
            CheckHighlightObject(eventData);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(cloneObject);

        if (highlightedObject != null)
        {
            RectTransform highlightRectTransform;

            if (highlightedObject.name == "TeamSetupHighlightImage")
            {
                highlightRectTransform = highlightedObject.transform.parent.GetComponent<RectTransform>();
            }
            else
            {
                highlightRectTransform = highlightedObject.GetComponent<RectTransform>();
            }

            RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();

            objectRectTransform.SetParent(highlightRectTransform);
            objectRectTransform.SetSiblingIndex(1);
            objectRectTransform.localPosition = Vector3.zero;

            DeactivateHighlightObject(highlightedObject);
        }

        foreach (var indicatorObject in fightManager.DragAndDropIndicators)
        {
            indicatorObject.SetActive(false);
        }
    }

    private void CheckHighlightObject(PointerEventData eventData)
    {
        highlightedObject = null;

        foreach (GameObject obj in fightManager.MarkObjects)
        {
            if (obj.name == Constants.InventoryContent)
            {
                continue; // Skip the iteration if originalParentName is "INVENTORYMANAGER" and obj is "InventoryContent"
            }

            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            if (rectTransform != null && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, null))
            {
                highlightedObject = obj;
                obj.transform.GetComponent<Image>().color = UIColors.quarterGreenCol;
                break;
            }
        }

        // Remove highlight effect from the previous highlightObject (if any)
        if (highlightedObject != previousHighlightObject)
        {
            if (previousHighlightObject != null)
            {
                DeactivateHighlightObject(previousHighlightObject);
            }
            previousHighlightObject = highlightedObject;
        }
    }

    private void DeactivateHighlightObject(GameObject obj)
    {
        if (obj.name != "TeamSetupHighlightImage")
        {
            obj.GetComponent<Image>().color = UIColors.invisibleCol;
        }
        else
        {
            obj.GetComponent<Image>().color = UIColors.quarterFadedCol;
        }
    }
}
