using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    protected Node m_node;

    public Inverter(Node node)
    {
        m_node = node;
    }
    public override NodeState Evaluate()
    {
            switch (m_node.Evaluate())
            {
                case NodeState.RUNNING:
                    m_nodeState = NodeState.RUNNING;
                    break;
                case NodeState.SUCCESS:
                    m_nodeState = NodeState.FAILURE;
                    break;
                case NodeState.FAILURE:
                    m_nodeState = NodeState.SUCCESS;
                    break;
                default:
                    break;

            }
            return m_nodeState;
    }
}
