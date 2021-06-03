using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionNode : Node
{
    private float m_Range;
    private Vector3 m_SpherePos;
    private LayerMask m_Mask;
    private UnitBrain m_Unit;
    public VisionNode(float m_range , Vector3 Currentpos, LayerMask mask, UnitBrain script)
    {
        m_Range = m_range;
        m_SpherePos = Currentpos;
        m_Mask = mask;
        m_Unit = script;
    }
    public override NodeState Evaluate()
    {
        if (m_Unit.m_Target != null)
        {
          if (m_Unit.m_Target.gameObject.activeInHierarchy)
          {
            return NodeState.SUCCESS;
          }
        }
        Collider[] col = Physics.OverlapSphere(m_SpherePos, m_Range * 0.8f, m_Mask);
        if (col.Length > 0)
        {
            float bestdist = 99999.0f;
            Collider Bestcollider = null;
            foreach (Collider item in col)
            {
                float distance = Vector3.Distance(m_Unit.transform.position, item.transform.position);
                if (distance < bestdist)
                {
                    bestdist = distance;
                    Bestcollider = item;
                }
            }
          if (m_Unit.m_Target == null)
          {
              m_Unit.m_Brain.Followpath.RemoveAt(0);
              m_Unit.SetTarget(Bestcollider.gameObject.GetComponent<UnitBrain>());
          }
                      return NodeState.SUCCESS;      
        }
        return NodeState.FAILURE;
    }
}
