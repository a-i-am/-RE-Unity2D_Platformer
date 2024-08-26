using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KDTree
{
    private class Node
    {
        public Vector2 point;
        public Node left;
        public Node right;

        public Node(Vector2 point)
        {
            this.point = point;
        }
    }

    private Node root;

    public void Insert(Vector2 point)
    {
        root = Insert(root, point, 0);
    }

    private Node Insert(Node node, Vector2 point, int depth)
    {
        if (node == null)
        {
            return new Node(point);
        }

        int axis = depth % 2;

        if (axis == 0)
        {
            if (point.x < node.point.x)
                node.left = Insert(node.left, point, depth + 1);
            else
                node.right = Insert(node.right, point, depth + 1);
        }
        else
        {
            if (point.y < node.point.y)
                node.left = Insert(node.left, point, depth + 1);
            else
                node.right = Insert(node.right, point, depth + 1);
        }

        return node;
    }

    public Vector2? NearestNeighbor(Vector2 target)
    {
        return NearestNeighbor(root, target, 0, null)?.point;
    }

    private Node NearestNeighbor(Node node, Vector2 target, int depth, Node best)
    {
        if (node == null) return best;

        if (best == null || Vector2.Distance(node.point, target) < Vector2.Distance(best.point, target))
        {
            best = node;
        }

        int axis = depth % 2;
        Node nearNode = (axis == 0 && target.x < node.point.x) || (axis == 1 && target.y < node.point.y) ? node.left : node.right;
        Node farNode = nearNode == node.left ? node.right : node.left;

        best = NearestNeighbor(nearNode, target, depth + 1, best);

        if ((axis == 0 && Mathf.Abs(target.x - node.point.x) < Vector2.Distance(best.point, target)) ||
            (axis == 1 && Mathf.Abs(target.y - node.point.y) < Vector2.Distance(best.point, target)))
        {
            best = NearestNeighbor(farNode, target, depth + 1, best);
        }

        return best;
    }
}

