using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIsActive : Node
{
    private UnitBrain m_Script; 
    public TargetIsActive(UnitBrain script)
    {
        m_Script = script;
    }
    public override NodeState Evaluate()
    {
        if (m_Script.m_Target != null)
        {
          if (!m_Script.m_Target.gameObject.activeInHierarchy)
          {
                m_Script.m_Target = null;
                m_Script.ResumePath();
                return NodeState.FAILURE;
          }
        }
            return NodeState.SUCCESS;
    }
}
