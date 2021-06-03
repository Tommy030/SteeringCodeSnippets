using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    protected List<Node> Childnodes = new List<Node>();

    public Selector(List<Node> node)
    {
        Childnodes = node;
    }
    public override NodeState Evaluate()
    {
        foreach (Node item in Childnodes)
        {
            switch (item.Evaluate())
            {
                case NodeState.RUNNING:
                    m_nodeState = NodeState.RUNNING;
                    return m_nodeState;
                case NodeState.SUCCESS:
                    m_nodeState = NodeState.SUCCESS;
                    return m_nodeState;
                case NodeState.FAILURE:
                    break;
                default:
                    break;

            }
        }
        m_nodeState = NodeState.FAILURE;
        return m_nodeState;
    }   
}
