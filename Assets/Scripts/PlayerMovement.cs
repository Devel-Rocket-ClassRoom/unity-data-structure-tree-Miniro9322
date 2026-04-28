using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Stage stage;
    private Animator animator;
    private int currentTileId;
    public int forLength = 3;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;

        var findGo = GameObject.FindWithTag("Map");
        stage = findGo.GetComponent<Stage>();
    }

    private void Update()
    {
        var direction = Sides.None;

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Sides.Top;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Sides.Bottom;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Sides.Right;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Sides.Left;
        }

        if(direction != Sides.None)
        {
            var targetTile = stage.Map.tiles[currentTileId].adjacents[(int)direction];
            if(targetTile != null && targetTile.CanMove)
            {
                MoveTo(targetTile.Id);
            }
        }
    }

    public void MoveTo(int tileId)
    {
        currentTileId = tileId;
        transform.position = stage.GetTilePos(currentTileId);
        OpenFow();
    }

    private void OpenFow()
    {
        for (int i = -forLength; i <= forLength; i++)
        {
            for (int j = -forLength; j <= forLength; j++)
            {
                var tileId = currentTileId + i * stage.mapWidth + j;
                stage.Map.tiles[tileId].isVisited = true;
                foreach(var adjacent in stage.Map.tiles[tileId].adjacents)
                {
                    if(adjacent != null)
                    {
                        adjacent.UpdateFowAutoTileId();
                        stage.DecorateTile(adjacent.Id);

                    }
                }
            }
        }
    }
}
