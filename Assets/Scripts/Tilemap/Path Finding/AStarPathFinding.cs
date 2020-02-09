using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Tilemap
{
    public class AStarPathFinding
    {
        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 14;
        private TilemapManager manager;
        private Tile[,] tiles;
        private PathNode[,] allPathNodes;
        private List<PathNode> openList = new List<PathNode>();
        private List<PathNode> closedList = new List<PathNode>();
        private PathNode endNode;

        public AStarPathFinding(TilemapManager manager) {
            this.manager = manager;
        }

        public void ResetTilemap(Tile[,] tiles) {
            this.tiles = tiles;
            allPathNodes = new PathNode[tiles.GetLength(0), tiles.GetLength(1)];
            SavePathNodes(tiles);

        }
        private void SavePathNodes(Tile[,] tiles) {
            PathNode newNode;
            foreach (Tile tile in tiles) {
                newNode = new PathNode(manager.FromWorldToGrid(tile.transform.position), tile.IsWalkable());
                allPathNodes[newNode.x, newNode.y] = newNode;
            }
        }


        public List<Vector2Int> FindPath(Vector2Int startingPosition, Vector2Int targetPosition) {
            ResetSelf();

            endNode = allPathNodes[targetPosition.x, targetPosition.y];
            PathNode startingNode = allPathNodes[startingPosition.x, startingPosition.y];
            openList.Add(startingNode);
            startingNode.gCost = 0;
            startingNode.hCost = CalculateDistanceCost(startingNode, endNode);

            while (openList.Count > 0) {
                PathNode currNode = FindNodeWithLowestFCost();
                int tentativeGCost;

                if (currNode == endNode) return CalculatePath(endNode);

                openList.Remove(currNode);
                closedList.Add(currNode);

                foreach (PathNode neighbourNode in GetNeighbours(currNode)) {
                    if (closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.isWalkable && neighbourNode != endNode) {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    tentativeGCost = currNode.gCost + CalculateDistanceCost(currNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost) {
                        neighbourNode.cameFromNode = currNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                    }
                }
            }

            return null;
        }

        private List<PathNode> GetNeighbours(PathNode node) {
            List<PathNode> neighbours = new List<PathNode>();
            if (node.x - 1 >= 0) {
                //Left
                if (allPathNodes[node.x - 1, node.y] != null)
                    neighbours.Add(allPathNodes[node.x - 1, node.y]);
                //Left Down
                if (node.y - 1 >= 0 && allPathNodes[node.x - 1, node.y - 1] != null)
                    neighbours.Add(allPathNodes[node.x - 1, node.y - 1]);
                //Left Up
                if (node.y + 1 < allPathNodes.GetLength(1) && allPathNodes[node.x - 1, node.y + 1] != null)
                    neighbours.Add(allPathNodes[node.x - 1, node.y + 1]);
            }
            if (node.x + 1 < allPathNodes.GetLength(0)) {
                //Right
                if (allPathNodes[node.x + 1, node.y] != null)
                    neighbours.Add(allPathNodes[node.x + 1, node.y]);
                //Right Down
                if (node.y - 1 >= 0 && allPathNodes[node.x + 1, node.y - 1] != null)
                    neighbours.Add(allPathNodes[node.x + 1, node.y - 1]);
                //Right Up
                if (node.y + 1 < allPathNodes.GetLength(1) && allPathNodes[node.x + 1, node.y + 1] != null)
                    neighbours.Add(allPathNodes[node.x + 1, node.y + 1]);
            }
            //Down
            if (node.y - 1 >= 0 && allPathNodes[node.x, node.y - 1] != null)
                neighbours.Add(allPathNodes[node.x, node.y - 1]);
            //Up
            if (node.y + 1 < allPathNodes.GetLength(1) && allPathNodes[node.x, node.y + 1] != null)
                neighbours.Add(allPathNodes[node.x, node.y + 1]);

            return neighbours;
        }

        private List<Vector2Int> CalculatePath(PathNode endNode) {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currNode = endNode;
            while (currNode.cameFromNode != null) {
                path.Add(currNode.cameFromNode);
                currNode = currNode.cameFromNode;
            }
            path.Reverse();
            return ConvertToVectors(path);
        }
        private List<Vector2Int> ConvertToVectors(List<PathNode> path) {
            List<Vector2Int> returnList = new List<Vector2Int>();
            foreach (PathNode node in path) {
                returnList.Add(new Vector2Int(node.x, node.y));
            }
            return returnList;
        }

        private PathNode FindNodeWithLowestFCost() {
            PathNode lowestFCostNode = openList[0];
            foreach (PathNode node in openList) {
                if (node.FCost < lowestFCostNode.FCost) {
                    lowestFCostNode = node;
                }
            }
            return lowestFCostNode;
        }

        private int CalculateDistanceCost(PathNode nodeA, PathNode nodeB) {
            int xDistance = Mathf.Abs(nodeA.x - nodeB.x);
            int yDistance = Mathf.Abs(nodeA.y - nodeB.y);
            int remaining = Mathf.Abs(xDistance - yDistance);

            return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
        }

        private void ResetSelf() {
            openList.Clear();
            closedList.Clear();

            foreach (PathNode node in allPathNodes) {
                node.gCost = int.MaxValue;
                node.cameFromNode = null;
            }
        }
    }
}