using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Grid))]
public class Pathfinding : MonoBehaviour {
    private Grid grid;

    //Debug
    public Transform startPos;
    public Transform endPos;

    private void Awake() {
        grid = GetComponent<Grid>();
    }

    private void Update() {
        FindPath(startPos.position, endPos.position);
    }

    /* PSUDO CODE - Because I am going to forget how this works
     * 
     * OPEN - the set of tiles to be evaluated
     * CLOSED - the set of tiles already evaluated
     * add the start tile to OPEN
     * 
     * loop
     *  current = tile in OPEN with the lowest fcost
     *  remove current from OPEN
     *  add current to CLOSE
     * 
     * if current is the target node - path has been found
     *  return
     * 
     * foreach neighbour of the current node
     *  if neighbout is not traversable of neighbour is in CLOSED
     *      skip to the next neighbour
     *  
     *  if new path to neighbour is shorter OR neighbout is not in OPEN
     *      set fcost of neighbour
     *      set parent of neighbout to current
     *      if neighbour is not in OPEN
     *          add neighbour to OPEN
     */
    private void FindPath(Vector3 startPos, Vector3 endPos) {
        Tile startTile = grid.GetTileAtWorldPosition(startPos);
        Tile targetTile = grid.GetTileAtWorldPosition(endPos);

        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        openSet.Add(startTile);

        while (openSet.Count > 0) {
            Tile currentTile = openSet[0];

            for (int i = 1; i < openSet.Count; i++) {

                if (openSet[i].fCost < currentTile.fCost || (openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost)) {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == targetTile) {
                RetracePath(startTile, targetTile);
                return;
            }

            foreach (Tile neighbour in grid.GetNeighbours(currentTile)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetTile);
                    neighbour.parent = currentTile;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    private void RetracePath(Tile startTile, Tile endTile) {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile) {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }

        path.Reverse();
        grid.path = path;
    }

    public int GetDistance(Tile tileA, Tile tileB) {
        int distX = Mathf.Abs(tileA.GridX - tileB.GridX);
        int distY = Mathf.Abs(tileA.GridY - tileB.GridY);

        if (distX > distY) {
            return 14 * distY + 10 * (distX - distY);
        }

        return 14 * distX + 10* (distY - distX);    
    }
}
