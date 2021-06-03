using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
public class ChaseNode : Node
{
    public UnitBrain m_Unit;
   public ChaseNode(UnitBrain Gridpos)
   {
        m_Unit = Gridpos;
   }
   public override NodeState Evaluate() //Calculates target-self position/relation
   {
            if (Vector3.Distance(m_Unit.gameObject.transform.position, m_Unit.m_Target.gameObject.transform.position) < 1.5)
            {
               // nadat de unit in range is om aan te vallen. Gaat hij over naar SEEK
               m_Unit.SetSeek();
               return NodeState.SUCCESS;
            }
            else
            {
              
              if (!m_Unit.m_Target.m_MyGridLocation.ReturnGridLocation().Equals(m_Unit.m_Brain.Followpath[m_Unit.m_Brain.Followpath.Count - 1]))
              {
                bool a = m_Unit.FollowPath(m_Unit.m_Target.m_MyGridLocation);
                if (a)
                {
                  return NodeState.RUNNING;
                }
                else
                {
                    return NodeState.SUCCESS;
                }
              }
                
               return NodeState.RUNNING;
            }
   }
}
