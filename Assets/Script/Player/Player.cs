using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Sprite 
    [SerializeField]
    private Sprite[] playerSprites;
    [SerializeField]
    private Sprite winSprite;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    #endregion

    #region Movement variables
    private Vector2 movementDirection;

    #endregion

    private void Start()
    {

        SetPlayerCenterCell();

    }

    private void Update()
    {
        if (GameManager.instance.IsGameStart())
        {
            if (movementDirection != Vector2.zero && !GameManager.instance.IsGameLose() && !GameManager.instance.IsGameWin())
            {
                Vector3Int cellPos = GridCellManager.instance.GetObjCell(transform.position);
                Vector3Int nextCellPos = cellPos + new Vector3Int((int)movementDirection.x, (int)movementDirection.y, 0);
                if (GridCellManager.instance.IsPlaceableArea(nextCellPos))
                {
                    this.transform.position = GridCellManager.instance.PositonToMove(nextCellPos);
                }
                SetRandomSprite();
                movementDirection = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Lightning")
        {
            GameManager.instance.Lose();    
        }
        if(collision.gameObject.tag == "Diamond")
        {
            spriteRenderer.sprite = winSprite;
            Destroy(collision.gameObject);
            GameManager.instance.Win();
        }
    }

    private void SetRandomSprite()
    {
        Sprite randomSprite = playerSprites[Random.Range(0, playerSprites.Length)];
        spriteRenderer.sprite = randomSprite;
    }

    #region Default Setup
    private void SetPlayerCenterCell()
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(transform.position);
        this.transform.position = GridCellManager.instance.PositonToMove(cellPos);
    }
    #endregion

    #region Input System
    private void OnLeft()
    {
        movementDirection = Vector2.left;
    }
    private void OnRight()
    {
        movementDirection = Vector2.right;
    }
    private void OnTop()
    {
        movementDirection = Vector2.up;
    }
    private void OnBot()
    {
        movementDirection = Vector2.down;
    }
    #endregion
}
