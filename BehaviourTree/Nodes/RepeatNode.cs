using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : Node
{
    private UnitBrain m_Script;
   public RepeatNode(UnitBrain Script)
    {
        m_Script = Script;
    }
    public override NodeState Evaluate()
    {
        Debug.Log("a");
        m_Script.FollowPath(m_Script.m_Path[m_Script.m_CurrentPath]);
        m_Script.BehaviourTree();
        return NodeState.SUCCESS;
    }
}
