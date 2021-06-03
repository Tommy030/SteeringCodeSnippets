using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumePathNode : Node
{
    private UnitBrain m_Script;
    public ResumePathNode(UnitBrain script)
    {
        m_Script = script;
    }
    public override NodeState Evaluate()
    {
        if (m_Script.m_Brain.requestedBehaviour != Steering.Brain.BehaviourEnum.FollowPath)
        {
           m_Script.FollowPath(m_Script.m_Path[m_Script.m_CurrentPath]);
        }
        return NodeState.SUCCESS;
    }
}
