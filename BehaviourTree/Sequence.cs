using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    protected List<Node> Childnodes = new List<Node>();

    public Sequence(List<Node> node)
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
                    return NodeState.RUNNING;
                case NodeState.FAILURE:
                    m_nodeState = NodeState.FAILURE;
                    return m_nodeState;
                default:
                    break;
            }
        }
        m_nodeState = NodeState.SUCCESS;
        return m_nodeState;
    }

}
