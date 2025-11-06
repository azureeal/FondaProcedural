using UnityEngine;
using System;
using System.Collections.Generic;
using VTools.RandomService;
using Unity.Mathematics;

public class BSPNode
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private RectInt m_Rect;
    private BSPNode m_Parent;
    private BSPNode m_Firstchild;
    private BSPNode m_Secondchild;
    private Vector2Int _roomMinSize = new(5, 5);
    private Vector2Int _roomMaxSize = new(12, 8);
    public RandomService RandomService;

    public BSPNode Parent { get => m_Parent; set => m_Parent = value; }
    public BSPNode Firstchild { get => m_Firstchild; set => m_Firstchild = value; }
    public BSPNode Secondchild { get => m_Secondchild; set => m_Secondchild = value; }

    public RectInt Rect { get => m_Rect; set => m_Rect = value; }

    public BSPNode(RectInt rect, RandomService service, List<BSPNode> nodeList) 
    { 
        m_Rect = rect; 
        RandomService = service;
        SplitNode(nodeList);
        nodeList.Add(m_Firstchild);
        nodeList.Add(m_Secondchild);
    }
    public void SplitNode(List<BSPNode> nodeList, int depth = 0)
    {
        // Limit the depth of the splits
        int maxDepth = 10; // Adjust as necessary
        if (depth >= maxDepth) return;

        bool horizontalSplit = RandomService.Chance(0.5f);

        if (horizontalSplit)
        {
            int splitYCoordinate = RandomService.Range(Rect.yMin + 1, Rect.yMax);
            if (splitYCoordinate == Rect.yMin || splitYCoordinate == Rect.yMax) return; // Prevent invalid split

            RectInt top = new RectInt(Rect.xMin, Rect.yMin, splitYCoordinate - Rect.yMin, Rect.width);
            RectInt bottom = new RectInt(Rect.xMin, splitYCoordinate, Rect.yMax - splitYCoordinate, Rect.width);

            if (bottom.height < _roomMinSize.y || top.height < _roomMinSize.y) return;
            if (bottom.width < _roomMinSize.x || top.width < _roomMinSize.x) return;

            BSPNode child1 = new BSPNode(top, RandomService, nodeList);
            Firstchild = child1;
            child1.Parent = this;

            BSPNode child2 = new BSPNode(bottom, RandomService, nodeList);
            Secondchild = child2;
            child2.Parent = this;
        }
        // Vertical split
        else
        {
            int splitXCoordinate = RandomService.Range(Rect.xMin + 1, Rect.xMax);
            if (splitXCoordinate == Rect.xMin || splitXCoordinate == Rect.xMax) return; // Prevent invalid split

            RectInt left = new RectInt(Rect.xMin, Rect.yMin, Rect.height, splitXCoordinate - Rect.xMin);
            RectInt right = new RectInt(splitXCoordinate, Rect.yMin, Rect.height, Rect.xMax - splitXCoordinate);

            // Check if the split would create too small rooms
            if (left.width < _roomMinSize.x || right.width < _roomMinSize.x) return;
            if (left.height < _roomMinSize.y || right.height < _roomMinSize.y) return;

            // Create the child nodes
            BSPNode child1 = new BSPNode(left, RandomService, nodeList);
            Firstchild = child1;
            child1.Parent = this;

            BSPNode child2 = new BSPNode(right, RandomService, nodeList);
            Secondchild = child2;
            child2.Parent = this;

        }
    }


}
