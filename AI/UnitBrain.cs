using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Steering;
public enum Awareness{Alert, Not_Alert }

public class UnitBrain : MonoBehaviour
{
    #region Variables
    public UnitStats m_stats; //This units Stats

    public UnitBrain m_Target; //Gets assigned by the BehaviourTree once it has found a target.

    public int m_CurrentPath; //The current indexnumber of m_path its following.
    public bool m_RepeatPath; //Repeats the current path
    public GridTile[] m_Path; //A list of objectives, The unit follows from 0 to List.Count.
    public GridTile m_MyGridLocation; //The current location of this unit.

    public Brain m_Brain; //Reference to brain script.
    public HoofdGebouwEnemy m_MainBuildingEnemy;//Enemy Main Building
    public LayerMask m_Mask; //Layermask for enemy units
    
    public Sequence m_Topnode; //Top node of the behaviour Tree
    #endregion
    #region UnitStart & BehaviourTree
    public void BehaviourTree()
    {
        VisionNode visionNode = new VisionNode(10 + (m_stats.m_SightRange), gameObject.transform.position,m_Mask,this);
        ChaseNode chaseNode = new ChaseNode(this);
        AttackNode attackNode = new AttackNode(this);
        TargetIsActive TargetNode = new TargetIsActive(this); 
        FollowNode followNode = new FollowNode(this);
        AtEndPathNode EndpathNode = new AtEndPathNode(this);
        AttackBaseNode attackBaseNode = new AttackBaseNode(this,m_MainBuildingEnemy);
        ResumePathNode ResumeNode = new ResumePathNode(this);
        Sequence FollowSequence = new Sequence(new List<Node> {TargetNode,ResumeNode,followNode ,EndpathNode,attackBaseNode});
        Sequence FollowCheck = new Sequence(new List<Node> { EndpathNode, FollowSequence });
        Sequence AttackSequence = new Sequence(new List<Node> { TargetNode, attackNode });
        Selector AttackSelector = new Selector(new List<Node> { AttackSequence, FollowSequence});
        Sequence ChaseSequence = new Sequence(new List<Node> { TargetNode, chaseNode,AttackSelector });
        Selector ChaseSelector = new Selector(new List<Node> {ChaseSequence,FollowSequence });
        Sequence VisionSequence  = new Sequence(new List<Node>{ visionNode,ChaseSelector});
        Selector VisionSelector = new Selector(new List<Node> { VisionSequence, FollowSequence });
        Selector FollowSelector = new Selector(new List<Node> { FollowCheck,VisionSelector});
        m_Topnode = new Sequence( new List<Node> { FollowSelector});
        m_Topnode.Evaluate(); 
    }
    private void Update()
    {
        m_Topnode.Evaluate(); //Evaluates the Topnode and runs the BehaviourTree
    }
    public void UnitScriptStart(int2 MyLoc,  UnitStats stats, LayerMask mask)
    {
        //Assigning Value's
        m_MyGridLocation = new GridTile();
        m_MyGridLocation.x = MyLoc.x;
        m_MyGridLocation.y = MyLoc.y;
        m_CurrentPath = 0;
        m_stats = stats;
        m_Mask = mask;
        m_Brain = GetComponent<Brain>();
        SteeringSettings Settings = GetComponent<Steering.Steering>().steeringSettings;
        Settings.maxDesiredVelocity = Settings.maxDesiredVelocity * ((100 + m_stats.m_MoveSpeed) / 100);
        Settings.maxSpeed = Settings.maxSpeed * ((100 + m_stats.m_MoveSpeed / 100));
        Settings.maxSteeringForce = Settings.maxSteeringForce * ((100 + m_stats.m_MoveSpeed) / 100);

        //Assigning Objectives and Path
        PathStart(m_stats.m_Behaviour, m_stats.m_Type);
        //starting pathing
        FollowPath(m_Path[m_CurrentPath]);
        //create behaviourTree
        BehaviourTree();
     
    }
    #endregion
    #region UnitFunctions
    public void PathStart(BehaviourType behaviour, TeamType Type)
    {
        switch (behaviour)
        {
            case BehaviourType.Aggresive:
                m_Path = new GridTile[2];
                foreach (Objectives item in WaypointManager.Instance.m_Objectives)
                {
                   if (Type == TeamType.Red)
                    {

                      switch (item.waypoint)
                      {
                        case WaypointType.attackBuffA:
                            m_Path[0] = item.Gridtile;
                            break;
                        case WaypointType.EnemyBase:
                            m_MainBuildingEnemy = item.Gridtile.LiveGrid.GetComponentInChildren<HoofdGebouwEnemy>();
                            m_Path[1] = item.Gridtile;
                            break;
                      }
                    }
                    else
                    {
                        switch (item.waypoint)
                        {
                            case WaypointType.DefendBuffA:
                                m_Path[0] = item.Gridtile;
                                break;
                            case WaypointType.Playerbase:
                                m_MainBuildingEnemy = item.Gridtile.LiveGrid.GetComponentInChildren<HoofdGebouwEnemy>();
                                m_Path[1] = item.Gridtile;
                                break;
                        }

                    }
                }
                break;
            case BehaviourType.Defensive:
                m_Path = new GridTile[2];
                m_RepeatPath = true;
                foreach (Objectives item in WaypointManager.Instance.m_Objectives)
                {
                    if (Type == TeamType.Red)
                    {
                        switch (item.waypoint)
                        {
                            case WaypointType.DefendPathA:
                                m_Path[0] = item.Gridtile;
                                break;
                            case WaypointType.Playerbase:
                                m_Path[1] = item.Gridtile;
                                break;
                        }
                    }
                    else
                    {
                        switch (item.waypoint)
                        {
                            case WaypointType.DefendPathB:
                                m_Path[0] = item.Gridtile;
                                break;
                            case WaypointType.EnemyBase:
                                m_Path[1] = item.Gridtile;
                                break;
                        }

                    }
                }
                break;  
            case BehaviourType.GuardPathA:
                m_Path = new GridTile[2];
                m_RepeatPath = true;
                foreach (Objectives item in WaypointManager.Instance.m_Objectives)
                {
                    if (Type == TeamType.Red)
                    {
                        switch (item.waypoint)
                        {
                            case WaypointType.DefendPathA:
                                m_Path[0] = item.Gridtile;
                                break;
                            case WaypointType.DefendPathAB:
                                m_Path[1] = item.Gridtile;
                                break;
                        }
                    }
                    else
                    {
                        switch (item.waypoint)
                        {
                            case WaypointType.DefendPathB:
                                m_Path[0] = item.Gridtile;
                                break;
                            case WaypointType.DefendPathBA:
                                m_Path[1] = item.Gridtile;
                                break;
                        }

                    }
                }
                break;
            case BehaviourType.GuardPathB:
                m_Path = new GridTile[2];
                m_RepeatPath = true;
                foreach (Objectives item in WaypointManager.Instance.m_Objectives)
                {
                    if (Type == TeamType.Red)
                    {
                        switch (item.waypoint)
                        {
                            case WaypointType.DefendPathB:
                                m_Path[0] = item.Gridtile;
                                break;
                            case WaypointType.DefendPathBA:
                                m_Path[1] = item.Gridtile;
                                break;
                        }
                    }
                    else
                    {
                        switch (item.waypoint)
                        {
                            case WaypointType.DefendPathA:
                                m_Path[0] = item.Gridtile;
                                break;
                            case WaypointType.DefendPathAB:
                                m_Path[1] = item.Gridtile;
                                break;
                        }

                    }
                }
                break;
        }
    }
    public void UpdatePosition(int2 Newpos)
    {
        m_MyGridLocation.x = Newpos.x;
        m_MyGridLocation.y = Newpos.y;
    }
    public bool NextPath()
    {
        m_CurrentPath++;
        if (m_Path.Length -1>= m_CurrentPath )
        {
            FollowPath(m_Path[m_CurrentPath]);
            return false;
        }
        else
        {
            return true;
        }
    }
    public void ResumePath()
    {
        FollowPath(m_Path[m_CurrentPath]);
    }
    public void SetSleep()
    {
        Brain brain = GetComponent<Brain>();
        brain.requestedBehaviour = Brain.BehaviourEnum.Sleep;
        brain.BehaviourChange();
    }
    public bool FollowPath(GridTile grid)
    {
        List<int2> Path = Pathfinding.Instance.FindPath(new int2(m_MyGridLocation.x, m_MyGridLocation.y), new int2(grid.x, grid.y), Map.Instance.GridMap);
        if (Path.Count > 0)
        {
            if (Path.Count > 2)
            {
                Path.RemoveAt(0);
            }
            Brain m_brain = GetComponent<Brain>();
            m_brain.Followpath = Path;
            m_brain.requestedBehaviour = Brain.BehaviourEnum.FollowPath;
            m_brain.BehaviourChange();
            return true;
        }
        else
        {
            return false;
        }
   
    }
    public void SetTarget(UnitBrain ATarget)
    {
        m_Target = ATarget;
    }
    public void SetSeek()
    {
        Brain m_brain = GetComponent<Brain>();
        m_brain.seekTarget = m_Target.gameObject;
        m_brain.requestedBehaviour = Brain.BehaviourEnum.Seek;
        m_brain.BehaviourChange();
    }
    public bool TakeDamage(int damage)
    {
        m_stats.m_Defense -= damage;
        if (m_stats.m_Defense <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    public void Die()
    {
        m_stats.Death();
    }

    #endregion
}
