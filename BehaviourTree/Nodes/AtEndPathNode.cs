using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtEndPathNode : Node
{
    public UnitBrain m_Unit;

    public AtEndPathNode(UnitBrain script)
    {
        m_Unit = script;
    }
    public override NodeState Evaluate()
    {
        if (!m_Unit.m_RepeatPath)
        {
            if (m_Unit.m_Brain.requestedBehaviour == Steering.Brain.BehaviourEnum.Sleep)
            {
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
