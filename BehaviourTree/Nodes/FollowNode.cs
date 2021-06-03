using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
public class FollowNode : Node
{
    private UnitBrain m_Unit;
    public FollowNode(UnitBrain unit)
    {
        m_Unit = unit;
    }
    public override NodeState Evaluate() //Calculates target-self position/relation
    {
        int2 m_Target = new int2(m_Unit.m_Path[m_Unit.m_CurrentPath].x, m_Unit.m_Path[m_Unit.m_CurrentPath].y);
        if (m_Unit.m_MyGridLocation.x >= m_Target.x - 1
         && m_Unit.m_MyGridLocation.x <= m_Target.x + 1
         && m_Unit.m_MyGridLocation.y >= m_Target.y - 1
         && m_Unit.m_MyGridLocation.y <= m_Target.y + 1)
        {
           
            if (m_Unit.m_Path.Length -1 !=  m_Unit.m_CurrentPath )
            {
                m_Unit.m_CurrentPath++;
                m_Unit.FollowPath(m_Unit.m_Path[m_Unit.m_CurrentPath]);
                return NodeState.FAILURE;
            }
            else
            {   
                if (m_Unit.m_RepeatPath)
                {
                    m_Unit.m_CurrentPath = 0;
                    m_Unit.FollowPath(m_Unit.m_Path[m_Unit.m_CurrentPath]);
                    return NodeState.FAILURE;
                }
                m_Unit.SetSleep();
                return NodeState.SUCCESS;
               
            }
        }
        return NodeState.RUNNING;
    }
}
