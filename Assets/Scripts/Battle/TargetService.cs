using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A local service of FightManager.cs that handles targeting and highlighting details.
/// </summary>
public class TargetService
{
    private readonly FightManager _fightManager;
    private readonly List<int> frontLineIDs = new() { 1, 2, 3 };
    private readonly List<int> backLineIDs = new() { 4, 5, 7 };

    public TargetService(FightManager fightManager)
    {
        _fightManager = fightManager != null ? fightManager : throw new ArgumentNullException(nameof(fightManager));
    }

    public void HighlightPossibleTargets(GameObject startingObject, string abilityName)
    {
        if (startingObject.TryGetComponent<BattleCharacter>(out var battleCharacter))
        {
            if (!battleCharacter.IsEnemy())
            {
                var ability = battleCharacter.GetCombatantAbility(abilityName);

                if (ability != null)
                {
                    if (ability.IsFrontlineAoe && ability.IsBacklineAoe)
                    {
                        foreach (var combatant in _fightManager.Combatants)
                        {
                            if (combatant.TryGetComponent<BattleCharacter>(out var enemyCharacter) &&
                                enemyCharacter.IsEnemy())
                            {
                                combatant.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.NeonRedHalf;
                            }
                        }
                    }
                    else if (ability.IsFrontlineAoe)
                    {
                        foreach (var combatant in _fightManager.Combatants)
                        {
                            if (combatant.TryGetComponent<BattleCharacter>(out var enemyCharacter) &&
                                enemyCharacter.IsEnemy() && frontLineIDs.Contains(enemyCharacter.GetCombatantPosition()))
                            {
                                combatant.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.NeonRedHalf;
                            }
                        }
                    }
                    else if (ability.IsBacklineAoe)
                    {
                        foreach (var combatant in _fightManager.Combatants)
                        {
                            if (combatant.TryGetComponent<BattleCharacter>(out var enemyCharacter) &&
                                enemyCharacter.IsEnemy() && backLineIDs.Contains(enemyCharacter.GetCombatantPosition()))
                            {
                                combatant.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.NeonRedHalf;
                            }
                        }
                    }
                    else
                    {
                        foreach (var combatant in _fightManager.Combatants)
                        {
                            if (combatant.TryGetComponent<BattleCharacter>(out var enemyCharacter) &&
                                enemyCharacter.IsEnemy())
                            {
                                combatant.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.NeonRedHalf;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("Ability was not found");
                }
            }
        }
        else
        {
            Debug.LogError("BattleCharacter component not found!");
        }
    }

    public void BlinkAvailableTargets(string objectName)
    {
        try
        {
            if (_fightManager.ActiveCombatant != null && _fightManager.ActiveCombatant.TryGetComponent<BattleCharacter>(out var charComponent))
            {
                var ability = charComponent.GetCombatantAbility(objectName);

                if (ability != null && ability.Name == objectName)
                {
                    if (ability.IsEnemyAbility && !ability.IsFrontlineAoe && !ability.IsBacklineAoe && _fightManager.EnemyTarget == null)
                    {
                        ClearAllHighlights();
                        _fightManager.ClearCombatAbilities();
                        _fightManager.ActionPanel.SetActive(false);
                        _fightManager.IsAbilitySelected = true;
                        _fightManager.ActiveAbility = ability;
                        _fightManager.PlayerGroupTargets.Clear();

                        foreach (var combatant in _fightManager.Combatants)
                        {
                            if (combatant.TryGetComponent<BattleCharacter>(out var battleComponent) && battleComponent != null &&
                                battleComponent.IsEnemy())
                            {
                                battleComponent.BlinkCombatant();
                            }
                        }
                    }
                    else
                    {
                        ClearAllHighlights();
                        _fightManager.ClearCombatAbilities();
                        _fightManager.ActionPanel.SetActive(false);
                        _fightManager.IsAbilitySelected = true;
                        _fightManager.ActiveAbility = ability;
                        _fightManager.PlayerGroupTargets.Clear();

                        if (_fightManager.ActiveCombatant.TryGetComponent<CombatantFunctions>(out var combatantFunction))
                        {
                            if (ability.IsFrontlineAoe && ability.IsBacklineAoe)
                            {
                                foreach (var combatant in _fightManager.Combatants)
                                {
                                    if (combatant.TryGetComponent<BattleCharacter>(out var battleComponent) && battleComponent != null &&
                                        battleComponent.IsEnemy())
                                    {
                                        _fightManager.PlayerGroupTargets.Add(combatant);
                                    }
                                }

                                combatantFunction.AttackEnemyTarget(true);
                            }
                            else if (ability.IsFrontlineAoe)
                            {
                                foreach (var combatant in _fightManager.Combatants)
                                {
                                    if (combatant.TryGetComponent<BattleCharacter>(out var battleComponent) && battleComponent != null &&
                                        battleComponent.IsEnemy() && battleComponent.GetBattleFormation() == Enumerations.BattleFormation.Front)
                                    {
                                        _fightManager.PlayerGroupTargets.Add(combatant);
                                    }
                                }

                                combatantFunction.AttackEnemyTarget(true);
                            }
                            else if (ability.IsBacklineAoe)
                            {
                                foreach (var combatant in _fightManager.Combatants)
                                {
                                    if (combatant.TryGetComponent<BattleCharacter>(out var battleComponent) && battleComponent != null &&
                                        battleComponent.IsEnemy() && battleComponent.GetBattleFormation() == Enumerations.BattleFormation.Back)
                                    {
                                        _fightManager.PlayerGroupTargets.Add(combatant);
                                    }
                                }

                                combatantFunction.AttackEnemyTarget(true);
                            }
                            else
                            {
                                combatantFunction.AttackEnemyTarget(false);
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("FightManager has no ActiveCombatant assigned or BattleCharacter component not found");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void ClearAllHighlights()
    {
        foreach (var combatant in _fightManager.Combatants)
        {
            if (combatant.TryGetComponent<BattleCharacter>(out var enemyCharacter) &&
                enemyCharacter.IsEnemy() && frontLineIDs.Contains(enemyCharacter.GetCombatantPosition()))
            {
                combatant.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.invisibleCol;
            }
        }
    }
}
