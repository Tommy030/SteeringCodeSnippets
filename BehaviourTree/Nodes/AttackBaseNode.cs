using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBaseNode : Node
{
    private UnitBrain m_Unit;
    private HoofdGebouwEnemy m_EnemyBase;
    private float TimerMax;
    private float CurrentTimer;
    public AttackBaseNode(UnitBrain script,HoofdGebouwEnemy Enemy)
    {
        m_EnemyBase = Enemy;
        m_Unit = script;
        TimerMax = 2 - m_Unit.m_stats.m_AttackSpeed / 180;
    }
    public override NodeState Evaluate()
    {
        Debug.Log("attackbase");
        bool DeathTarget = false;
        CurrentTimer += Time.deltaTime;

        if (CurrentTimer >= TimerMax)
        {
            CurrentTimer = 0;
            DeathTarget = m_EnemyBase.TakeDamage(1 + m_Unit.m_stats.m_AttackDamage);
        }
        return DeathTarget ? NodeState.FAILURE : NodeState.RUNNING;
    }
}
