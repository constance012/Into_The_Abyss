using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SurfaceSensor surfaceSensor;
    private HealthPoint healthPoint;

    [Header("Movement")]
    [SerializeField] private float acceleration = 0.29f;
    [SerializeField] private float maxXSpeed = 3f;
    [Range(0, 1) , SerializeField] private float groundDecay = 0.8f;
    public float xInput {get; private set;}

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float maxFallSpeed = 70f;

    [Header("Digging")]
    [SerializeField] private Tilemap ground;
    [SerializeField] private Transform digPoint;
    [SerializeField] private float digSpeed = 1f;
    [SerializeField] private float timeFromLastDig = 0f;

    [SerializeField]private List<TypeTile> listTiles;
    private Dictionary<TileBase, TypeTile> tileData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        surfaceSensor = GetComponent<SurfaceSensor>();
        healthPoint = GetComponent<HealthPoint>();

        //Get all the tiles from the list and add them to a dictionary for easy access
        tileData = new Dictionary<TileBase, TypeTile>();

        foreach(TypeTile tile in listTiles)
        {
            foreach(TileBase tileBase in tile.Tiles)
            {
                tileData.Add(tileBase, tile);
            }
        }
    }

    private void Update()
    {
        CheckInput();
        DigTile();

        timeFromLastDig += Time.deltaTime;
    }

    private void FixedUpdate(){
        ApplyPhysics();
        HandleMovement();
    }

    private void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    private void DigTile()
    {
        if(Input.GetMouseButtonDown(0) && surfaceSensor.grounded && timeFromLastDig > 1/digSpeed)
        {
            Vector3Int gridPosition = ground.WorldToCell(digPoint.position);

            TileBase currentTile = ground.GetTile(gridPosition);

            if(currentTile != null && tileData[currentTile].Destructible)
            {
                ground.SetTile(gridPosition, null);
            }

            timeFromLastDig = 0f;
        }
    }

    private void HandleMovement()
    {
        if(Mathf.Abs(xInput) > 0)
        {
            //increment velocity by our acceleration, then clamp within max
            float increment = xInput * acceleration;
            float newSpeed = Math.Clamp(rb.linearVelocityX + increment, -maxXSpeed, maxXSpeed);
            rb.linearVelocity = new Vector2(newSpeed, rb.linearVelocityY);
        }

        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && surfaceSensor.grounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void ApplyPhysics()
    {
        //Apply Fiction
        if(surfaceSensor.grounded && Mathf.Abs(xInput) < 0.1f)
        {
            rb.linearVelocity *= groundDecay;
        }

        //Apply Gravity
        if(!surfaceSensor.grounded)
        {
            rb.linearVelocity = new Vector2(0f, gravity * Time.deltaTime);
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxFallSpeed);
        }
    }
}
