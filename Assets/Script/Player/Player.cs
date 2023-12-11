using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Quaternion[] playerRotations;
    #region Sprite 
    [SerializeField]
    private Sprite[] playerSprites;
    [SerializeField]
    private Sprite winSprite;
    [SerializeField]
    private Sprite loseSprite;
    [SerializeField] 
    private Sprite[] lightningSprites;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    #endregion

    #region Movement variables
    private Vector2 movementDirection;

    #endregion

    private void Start()
    {

        SetPlayerCenterCell();
        this.GetComponent<SpriteRenderer>().enabled = false;

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
            collision.GetComponent<SpriteRenderer>().color = Color.white;
            StartCoroutine(LoseAnim());
            GameManager.instance.Lose();    
        }
        if(collision.gameObject.tag == "Diamond")
        {
            spriteRenderer.sprite = winSprite;
            Destroy(collision.gameObject);
            StartCoroutine(WinAnim());
            GameManager.instance.Win();
        }
    }

    private IEnumerator WinAnim()
    {
        Transform body = this.transform.Find("Body");
        int count = 0;

        while (true)
        {
            body.transform.rotation = playerRotations[count];
            count++;
            if (count == playerRotations.Length)
            {
                count = 0;
            }
            yield return new WaitForSeconds(.3f);
        }
    }

    private IEnumerator LoseAnim()
    {
        Transform body = this.transform.Find("Body");
        spriteRenderer.sprite = loseSprite;
        SpriteRenderer effectSprite = GetComponent<SpriteRenderer>();
        effectSprite.enabled = true;
        int count = 0;

        while (true)
        {
            effectSprite.sprite = lightningSprites[count];
            body.transform.rotation = playerRotations[count];
            count++;
            if (count == lightningSprites.Length)
            {
                count = 0;
            }
            yield return new WaitForSeconds(.3f);
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
        transform.position = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).playerSpawnPos;
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(transform.position);
        this.transform.position = GridCellManager.instance.PositonToMove(cellPos);
    }
    #endregion

    #region Input System
    private void OnLeft()
    {
        if(!GameManager.instance.IsGameLose() && !GameManager.instance.IsGameWin() && GameManager.instance.IsGameStart())
        {
            movementDirection = Vector2.left;
        }
    }
    private void OnRight()
    {
        if (!GameManager.instance.IsGameLose() && !GameManager.instance.IsGameWin() && GameManager.instance.IsGameStart())
        {
            movementDirection = Vector2.right;
        }
    }
    private void OnTop()
    {
        if (!GameManager.instance.IsGameLose() && !GameManager.instance.IsGameWin() && GameManager.instance.IsGameStart())
        {
            movementDirection = Vector2.up;
        }
    }
    private void OnBot()
    {
        if (!GameManager.instance.IsGameLose() && !GameManager.instance.IsGameWin() && GameManager.instance.IsGameStart())
        {
            movementDirection = Vector2.down;
        }
    }
    #endregion
}
