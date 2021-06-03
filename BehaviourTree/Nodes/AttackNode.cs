using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackNode : Node
{
    private UnitBrain m_Unit;
    private float TimerMax;
    private float CurrentTimer;

    public AttackNode(UnitBrain my) //Sets relevant values
    {
        m_Unit = my;
        TimerMax = 10 - m_Unit.m_stats.m_AttackSpeed / 180;
    }
    public override NodeState Evaluate() //Quick check if there's a target, proceeds to attack until target or self is destroyed
    {
        bool DeathTarget = false;
        CurrentTimer += Time.deltaTime;
        if (Vector3.Distance(m_Unit.gameObject.transform.position, m_Unit.m_Target.gameObject.transform.position) < 1.5)
        {
            if ( CurrentTimer >= TimerMax)
            {   
            CurrentTimer = 0;
            DeathTarget = m_Unit.m_Target.TakeDamage(1 + m_Unit.m_stats.m_AttackDamage);
            }
        }
        return DeathTarget ? NodeState.FAILURE : NodeState.RUNNING;
    }
}
