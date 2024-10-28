using Cysharp.Threading.Tasks;
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
    private PrefabManager prefabManager;
    [SerializeField] GameObject highestObject;

    void Awake()
    {
        fightManager = GameObject.Find("FIGHTMANAGER").GetComponent<FightManager>();
        prefabManager = GameObject.Find("PREFABMANAGER").GetComponent<PrefabManager>();
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

    public void AttackAllyTarget(CombatAbility ability)
    {
        if (fightManager.ActiveCombatant != null && fightManager.EnemiesTarget != null)
        {
            bool isAoe = false;

            if (ability.IsBacklineAoe || ability.IsFrontlineAoe)
            {
                isAoe = true;
            }

            _ = MoveCombatantAsync(false, isAoe);
        }
        else
        {
            if (fightManager.ActiveCombatant.TryGetComponent<BattleCharacter>(out var character))
            {
                character.SetEnemyTarget(fightManager);

                bool isAoe = false;

                if (ability.IsBacklineAoe || ability.IsFrontlineAoe)
                {
                    isAoe = true;
                }

                _ = MoveCombatantAsync(false, isAoe);
            }
            else
            {
                Debug.LogError("ActiveCombatant has no BattleCharacter component!");
            }
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
            if (targetCombatant.TryGetComponent<BattleCharacter>(out var targetCharacter) &&
                fightManager.ActiveCombatant.TryGetComponent<BattleCharacter>(out var activeCharacter))
            {
                int damage = 0;

                if (fightManager.ActiveAbility.Type != Constants.Buff)
                {
                    // we check if the target has any reflection debuffs that should be applied before the damage is calculated
                    if (targetCharacter.GetReflectionStatusEffects().Count > 0)
                    {
                        foreach (var re in targetCharacter.GetReflectionStatusEffects())
                        {
                            var chance = Random.Range(0, 100);
                            if (chance <= re.chance)
                            {
                                var newStatusEffect = new StatusEffect(re.name, re.type, re.statAffection, re.chance, re.portionValue, re.damageValue,
                                    re.duration, re.isFrontLineAoe, re.isBackLineAoe);

                                activeCharacter.DisplayEffect(fightManager.ActiveCombatant, newStatusEffect, true, false);
                                activeCharacter.AddStatusEffect(newStatusEffect, false);
                            }
                        }
                    }

                    damage = targetCharacter.ApplyDamagedReductions(activeCharacter, fightManager.ActiveAbility);

                    var criticalChanceStrike = Random.Range(0, 100);
                    if (criticalChanceStrike < activeCharacter.GetCombatantIntStat(Constants.CriticalChance))
                    {
                        damage *= (activeCharacter.GetCombatantIntStat(Constants.CriticalDamage) / 100) + 1;
                        targetCharacter.GetComponent<Animator>().Play(Constants.CritAnim);
                    }
                    else
                    {
                        targetCharacter.GetComponent<Animator>().Play(Constants.DamageAnim);
                    }

                    targetCharacter.ReceiveDamage(damage);

                    // display damage in the UI and show a message in the combat log
                    targetCombatant.transform.Find("Damage/Value").GetComponent<TextMeshProUGUI>().text = damage.ToString();

                    if (fightManager.ActiveCombatant != null && fightManager.ActiveCombatant.TryGetComponent<BattleCharacter>(out var activeChar))
                    {
                        TranslationManager translationManager = GameObject.Find(Constants.TranslationManager).GetComponent<TranslationManager>();
                        var jsonCombatMessage = translationManager.Translate("CombatLogMessage");
                        var updatedMessage = jsonCombatMessage
                                .Replace("{activeCharName}", activeChar.GetCombatantName())
                                .Replace("{abilityName}", fightManager.ActiveAbility.Name)
                                .Replace("{damage}", damage.ToString());

                        fightManager.CreateCombatMessage(updatedMessage);
                    }
                }

                // if there are any dots in the ability
                if (fightManager.ActiveAbility.NegativeStatusEffects.Count > 0)
                {
                    foreach (var effect in fightManager.ActiveAbility.NegativeStatusEffects)
                    {
                        var chance = Random.Range(0, 100);
                        if (chance <= effect.chance)
                        {
                            // Negative status effect can be resisted and not applied if target has Resistance value
                            // but attacker can lower it with his Penetration value
                            var resistChance = Random.Range(0, 100);
                            var resistDiff = targetCharacter.GetCombatantIntStat(Constants.Resistance) -
                                activeCharacter.GetCombatantIntStat(Constants.Penetration);

                            if (resistDiff < 0)
                            {
                                resistDiff = 0;
                            }

                            if (resistChance < resistDiff)
                            {
                                _ = fightManager.CreateFlyingMessage(targetCombatant, effect);
                            }
                            else
                            {
                                var newStatusEffect = new StatusEffect(effect.name, effect.type, effect.statAffection, effect.chance, effect.portionValue, damage,
                                    effect.duration, effect.isFrontLineAoe, effect.isBackLineAoe);

                                targetCharacter.DisplayEffect(targetCombatant, newStatusEffect, true, false);
                                targetCharacter.AddStatusEffect(newStatusEffect, false);

                                // if the negative effect is a debuff, then apply the reductions to the target
                                if (effect.type == Enumerations.StatusEffectType.Debuff)
                                {
                                    targetCharacter.ApplyDebuffEffect(newStatusEffect);
                                }
                            }
                        }
                    }
                }
                else if (fightManager.ActiveAbility.PositiveStatusEffects.Count > 0)
                {
                    foreach (var effect in fightManager.ActiveAbility.PositiveStatusEffects)
                    {
                        var chance = Random.Range(0, 100);
                        if (chance <= effect.chance)
                        {
                            var newStatusEffect = new StatusEffect(effect.name, effect.type, effect.statAffection, effect.chance, effect.portionValue, effect.damageValue,
                                effect.duration, effect.isFrontLineAoe, effect.isBackLineAoe);

                            activeCharacter.DisplayEffect(fightManager.ActiveCombatant, newStatusEffect, false, false);
                            activeCharacter.AddStatusEffect(newStatusEffect, true);

                            // if the positive effect is a buff, then apply the additions to the caster
                            if (effect.type == Enumerations.StatusEffectType.Buff)
                            {
                                activeCharacter.ApplyBuffEffect(newStatusEffect);
                            }
                        }
                    }
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
                await PlayerAttacksEnemy(isAoe, isPlayerGroup);
            }
            else
            {
                await EnemyAttacksPlayer();
            }

            if (fightManager.IsCounterAttacking)
            {
                fightManager.IsCounterAttacking = false;
                return;
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

    private async UniTask PlayerAttacksEnemy(bool isAoe, bool isPlayerGroup)
    {
        bool counterAttackProc = false;

        if (!isAoe && fightManager.EnemyTarget != null)
        {
            await Attack(fightManager.ActiveCombatant, fightManager.EnemyTarget, isPlayerGroup, fightManager.PlayerGroupTargets, fightManager.ActiveAbility);

            // Chance for counter attack
            var counterChance = UnityEngine.Random.Range(0, 100);

            if (fightManager.EnemyTarget.TryGetComponent<BattleCharacter>(out var targetCharacter))
            {
                if (counterChance < targetCharacter.GetCombatantIntStat(Constants.CounterChance) &&
                    !targetCharacter.IsCombatantStunned())
                {
                    counterAttackProc = true;
                    fightManager.CreateFlyingMessage(fightManager.EnemyTarget, null).Forget();
                }

                await UniTask.WaitForSeconds(1);

                if (targetCharacter.IsCombatantDead())
                {
                    fightManager.EnemyTarget.GetComponent<Animator>().Play(Constants.DeathAnim);
                    await UniTask.WaitForSeconds(1);
                }
                else
                {
                    if (counterAttackProc)
                    {
                        // Counter attack will always use an auto attack (default)
                        fightManager.IsCounterAttacking = true;
                        fightManager.ActiveCombatant = fightManager.EnemyTarget;
                        targetCharacter.SetCombatantAbilityAndAttack(fightManager, 0);
                    }
                }
            }
        }
        else if (isAoe && fightManager.PlayerGroupTargets.Count > 0)
        {
            await Attack(fightManager.ActiveCombatant, fightManager.EnemyTarget, isPlayerGroup, fightManager.PlayerGroupTargets, fightManager.ActiveAbility);
            await UniTask.WaitForSeconds(1);

            foreach (var target in fightManager.PlayerGroupTargets)
            {
                if (target.TryGetComponent<BattleCharacter>(out var targetCharacter))
                {
                    if (targetCharacter.IsCombatantDead())
                    {
                        target.GetComponent<Animator>().Play(Constants.DeathAnim);
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

    private async UniTask EnemyAttacksPlayer()
    {
        if (fightManager.EnemiesTarget != null)
        {
            bool counterAttackProc = false;

            await Attack(fightManager.ActiveCombatant, fightManager.EnemiesTarget, false, fightManager.EnemyGroupTargets, fightManager.ActiveAbility);

            // Chance for counter attack
            var counterChance = UnityEngine.Random.Range(0, 100);

            if (fightManager.EnemiesTarget.TryGetComponent<BattleCharacter>(out var targetCharacter))
            {
                if (counterChance < targetCharacter.GetCombatantIntStat(Constants.CounterChance) &&
                    !targetCharacter.IsCombatantStunned())
                {
                    counterAttackProc = true;
                    fightManager.CreateFlyingMessage(fightManager.EnemiesTarget, null).Forget();
                }

                await UniTask.WaitForSeconds(1);

                if (targetCharacter.IsCombatantDead())
                {
                    fightManager.EnemiesTarget.GetComponent<Animator>().Play(Constants.DeathAnim);
                    await UniTask.WaitForSeconds(1);
                }
                else
                {
                    if (counterAttackProc)
                    {
                        // Counter attack will always use an auto attack (default)
                        fightManager.IsCounterAttacking = true;
                        targetCharacter.SetAttackerAsPlayerTarget(fightManager, fightManager.ActiveCombatant);
                        fightManager.ActiveCombatant = fightManager.EnemiesTarget;
                        targetCharacter.SetPlayerAbilityAndAttack(fightManager, 0);
                    }
                }
            }
            else
            {
                Debug.LogError("BattleCharacter not found on EnemiesTarget!");
            }
        }
        else
        {
            Debug.LogError("EnemiesTarget is null!");
        }
    }

    /// <summary>
    /// Attack always consists of 3 phases - Move, Damage and Return. If the ability is a moving ability, the move phase moves the attacker to the enemy.
    /// If the ability is not a moving ability but may have a prefab during 'move phase', then the prefab spawns instead of the whole moving.
    /// </summary>
    /// <param name="activeCombatant"></param>
    /// <param name="targetCombatant"></param>
    /// <param name="isPlayerGroup"></param>
    /// <param name="targetCombatants"></param>
    /// <param name="ability"></param>
    /// <returns></returns>
    private async UniTask Attack(GameObject activeCombatant, GameObject targetCombatant, bool isPlayerGroup, List<GameObject> targetCombatants, CombatAbility ability)
    {
        float movingTime = 0.3f;
        float elapsed = 0f;
        int castingTime = 800; // representing UniTask delay

        if (ability.IsMovingAbility)
        {
            float originalStarterX = activeCombatant.transform.position.x;
            float originalStarterY = activeCombatant.transform.position.y;

            float starterX = activeCombatant.transform.position.x;
            float starterY = activeCombatant.transform.position.y;

            (var targetX, var targetY) = GetTargetCoordinates(targetCombatant, targetCombatants, isPlayerGroup, false);

            Vector2 vector;

            // Move phase
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

            await CheckForAbilityPrefabs(ability, activeCombatant, targetCombatant, castingTime, targetCombatants, isPlayerGroup, elapsed, movingTime);

            // Damage phase
            if (targetCombatants.Count > 0)
            {
                foreach (var combatant in targetCombatants)
                {
                    CalculateChances(combatant);
                }
            }
            else
            {
                CalculateChances(targetCombatant);
            }

            // Return phase
            while (elapsed < movingTime)
            {
                elapsed += Time.deltaTime;
                float newX = Mathf.Lerp(starterX, originalStarterX, elapsed / movingTime);
                float newY = Mathf.Lerp(starterY, originalStarterY, elapsed / movingTime);
                vector = new(newX, newY);
                activeCombatant.transform.position = vector;
                await UniTask.Yield();
            }
        }
        // Not moving ability, meaning the combatant is staying at his place all the time
        else
        {
            // Move phase - in this case it's a phase where combatant should be moving but most probably it involves
            // spawning some initial prefab for spell or animation travelling to enemy
            await CheckForAbilityPrefabs(ability, activeCombatant, targetCombatant, castingTime, targetCombatants, isPlayerGroup, elapsed, movingTime);

            // Damage phase
            if (targetCombatants.Count > 0)
            {
                foreach (var combatant in targetCombatants)
                {
                    CalculateChances(combatant);
                }
            }
            else
            {
                CalculateChances(targetCombatant);
            }

            // Return phase
        }

        if (activeCombatant.TryGetComponent<BattleCharacter>(out var character))
        {
            character.ClearCombatantTimebar();
            character.ReduceAbilitiesCooldowns();
        }


        if (fightManager.ActiveAbility.Cooldown > 0)
        {
            fightManager.ActiveAbility.SetAbilityOnCooldown();
        }
    }

    private async UniTask CheckForAbilityPrefabs(CombatAbility ability, GameObject activeCombatant, GameObject targetCombatant, int castingTime,
        List<GameObject> targetCombatants, bool isPlayerGroup, float elapsed, float movingTime)
    {
        if (ability.AbilityPrefabs.Count > 0)
        {
            foreach (var abilityPrefab in ability.AbilityPrefabs)
            {
                if (abilityPrefab.prefabStart == Enumerations.PrefabStart.MovePhase)
                {
                    GameObject movePhasePrefab = prefabManager.GetAbilityPrefab(abilityPrefab.prefabName);
                    GameObject instantiatedPrefab;

                    if (abilityPrefab.prefabSpawn == Enumerations.PrefabSpawn.User)
                    {
                        instantiatedPrefab = Instantiate(movePhasePrefab, activeCombatant.transform);
                        instantiatedPrefab.AddComponent<DestroyWhenFinished>();
                    }
                    else if (abilityPrefab.prefabSpawn == Enumerations.PrefabSpawn.Target)
                    {
                        instantiatedPrefab = Instantiate(movePhasePrefab, targetCombatant.transform);
                        instantiatedPrefab.AddComponent<DestroyWhenFinished>();
                    }
                    else if (abilityPrefab.prefabSpawn == Enumerations.PrefabSpawn.BattlefieldCenter)
                    {
                        instantiatedPrefab = Instantiate(movePhasePrefab, highestObject.transform);
                        instantiatedPrefab.AddComponent<DestroyWhenFinished>();
                    }
                    else
                    {
                        instantiatedPrefab = Instantiate(movePhasePrefab, targetCombatant.transform);
                        instantiatedPrefab.AddComponent<DestroyWhenFinished>();
                    }

                    if (abilityPrefab.prefabRotation == Enumerations.PrefabRotation.ToEnemy)
                    {
                        RotatePrefabToEnemy(abilityPrefab, targetCombatant, activeCombatant, instantiatedPrefab);

                        // we are shortening the frost beam range if the combatant is placed in front line
                        if (instantiatedPrefab.name == "FrostBeam(Clone)")
                        {
                            if (activeCombatant.TryGetComponent(out BattleCharacter battleCharacter) &&
                                battleCharacter.GetBattleFormation() == Enumerations.BattleFormation.Front)
                            {
                                var velocityModule = instantiatedPrefab.transform.Find("Trail").GetComponent<ParticleSystem>().velocityOverLifetime;
                                velocityModule.enabled = true;
                                velocityModule.x = new ParticleSystem.MinMaxCurve(-5f, 5f);
                                velocityModule.y = new ParticleSystem.MinMaxCurve(0f, 25f);
                                velocityModule.z = new ParticleSystem.MinMaxCurve(0f, 0f);

                                var velocityModule2 = instantiatedPrefab.transform.Find("TrailCore").GetComponent<ParticleSystem>().velocityOverLifetime;
                                velocityModule2.enabled = true;
                                velocityModule2.x = 0f;
                                velocityModule2.y = 40f;
                                velocityModule2.z = 0f;
                            }
                        }

                        await UniTask.Delay(castingTime);
                    }

                    if (abilityPrefab.prefabMovement == Enumerations.PrefabMovement.ToEnemy)
                    {
                        float prefabX = instantiatedPrefab.transform.position.x;
                        float prefabY = instantiatedPrefab.transform.position.y;
                        instantiatedPrefab.transform.SetParent(highestObject.transform);
                        (var targetX, var targetY) = GetTargetCoordinates(targetCombatant, targetCombatants, isPlayerGroup, true);
                        Vector2 newVector;

                        while (elapsed < movingTime)
                        {
                            elapsed += Time.deltaTime;
                            float newX = Mathf.Lerp(prefabX, targetX, elapsed / movingTime);
                            float newY = Mathf.Lerp(prefabY, targetY, elapsed / movingTime);
                            newVector = new(newX, newY);
                            instantiatedPrefab.transform.position = newVector;
                            await UniTask.Yield();
                        }

                        Destroy(instantiatedPrefab);
                    }
                }

                if (abilityPrefab.prefabStart == Enumerations.PrefabStart.DamagePhase)
                {
                    GameObject damagePhasePrefab = prefabManager.GetAbilityPrefab(abilityPrefab.prefabName);
                    Transform prefabContainer = targetCombatant.transform.Find("PrefabContainer").transform;
                    GameObject instantiatedPrefab = Instantiate(damagePhasePrefab, prefabContainer);
                    instantiatedPrefab.AddComponent<DestroyWhenFinished>();
                }
            }
        }
    }

    private (float targetX, float targetY) GetTargetCoordinates(GameObject targetCombatant, List<GameObject> targetCombatants, bool isPlayerGroup, bool goUntilEnd)
    {
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

                        if (goUntilEnd)
                        {
                            targetY = combatant.transform.position.y;
                        }
                        else
                        {
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
        }
        else
        {
            targetX = targetCombatant.transform.position.x;

            if (goUntilEnd)
            {
                targetY = targetCombatant.transform.position.y;
            }
            else
            {
                if (isPlayerGroup)
                {
                    targetY = targetCombatant.transform.position.y * 0.7f;
                }
                else
                {
                    targetY = targetCombatant.transform.position.y / 0.7f;
                }
            }
        }

        return (targetX, targetY);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.TryGetComponent<BattleCharacter>(out var battleCharacter) && !battleCharacter.IsEnemy() &&
            !fightManager.IsBattleRunning)
        {
            gameObject.transform.Find("Glow").GetComponent<Image>().color = UIColors.NeonBlueOriginalHalf;
        }
        else if (gameObject.TryGetComponent<BattleCharacter>(out var enemyCharacter) && enemyCharacter.IsEnemy() &&
                 fightManager.IsBattleRunning)
        {
            gameObject.transform.Find("Glow").GetComponent<Image>().color = UIColors.NeonRedHalf;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.TryGetComponent<BattleCharacter>(out var battleCharacter) && !battleCharacter.IsEnemy() &&
            !fightManager.IsBattleRunning)
        {
            gameObject.transform.Find("Glow").GetComponent<Image>().color = UIColors.invisibleCol;
        }
        else if (gameObject.TryGetComponent<BattleCharacter>(out var enemyCharacter) && enemyCharacter.IsEnemy() &&
                 fightManager.IsBattleRunning)
        {
            gameObject.transform.Find("Glow").GetComponent<Image>().color = UIColors.invisibleCol;
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

    private void RotatePrefabToEnemy(AbilityPrefab abilityPrefab, GameObject targetCombatant, GameObject activeCombatant, GameObject instantiatedPrefab)
    {
        if (abilityPrefab.prefabRotation == Enumerations.PrefabRotation.ToEnemy)
        {
            // Assuming enemyObject is the GameObject representing the enemy
            Vector3 directionToEnemy = targetCombatant.transform.position - activeCombatant.transform.position;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;

            // Rotate the RectTransform on the Z-axis towards the enemy
            instantiatedPrefab.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angle * 2);
            instantiatedPrefab.transform.SetParent(highestObject.transform);
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

    private void CalculateChances(GameObject target)
    {
        if (target.TryGetComponent<BattleCharacter>(out var targetCharacter))
        {
            var hitChance = Random.Range(0, 100);

            if (hitChance < targetCharacter.GetCombatantIntStat(Constants.HitChance))
            {
                var dodgeChance = Random.Range(0, 100);

                if (dodgeChance < targetCharacter.GetCombatantIntStat(Constants.Dodge))
                {
                    target.GetComponent<Animator>().Play(Constants.DodgeAnim);
                }
                else
                {
                    CalculateAbilityDamage(target);
                }
            }
            else
            {
                target.GetComponent<Animator>().Play(Constants.MissAnim);
            }
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
