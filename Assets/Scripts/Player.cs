using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;

    public float trailSpeed = 1;
    private int trailPoints;

    public Sprite[] sprite_set1;
    public Sprite[] sprite_set2;
    private Sprite[] sprites;
    private bool isProud;
    public Toggle prideToggle;

    private int spriteIndex;
    private int flapAnimationRun = 0;
    public int maxFlapAnimations = 1;

    private Vector3 direction;
    private Vector3 rotationVector;
    private float rotatingStrength = 5.0f;
    private float tiltSmoothing = 5.0f;

    public float gravity = -9.8f;
    public float strength = 5.0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable() 
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        direction = Vector3.zero;
        rotationVector = Vector3.zero;
        trailRenderer.Clear();
        isProud = prideToggle.isOn;

        if (isProud)
        {
            sprites = sprite_set2;
        }
        else
        {
            sprites = sprite_set1;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0)) 
        { 
            direction = Vector3.up * strength;
            flapAnimationRun = 0;
        }

        if (Input.touchCount > 0) 
        { 
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) 
            {
                direction = Vector3.up * strength;
            }
        }

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        rotationVector.z = direction.y * rotatingStrength;
        Quaternion rotation = Quaternion.Euler(rotationVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * tiltSmoothing);

        trailPoints = trailRenderer.positionCount;
        for (int i = 0; i < trailPoints; i++)
        {
            Vector3 TrailPosition = trailRenderer.GetPosition(i);
            TrailPosition += Vector3.left * trailSpeed * Time.deltaTime;
            trailRenderer.SetPosition(i, TrailPosition);
        }

    }

    private void AnimateSprite() 
    {
        if (flapAnimationRun < maxFlapAnimations)
        {
            spriteIndex++;

            if (spriteIndex == sprites.Length)
            {
                flapAnimationRun++;
            }

            if (spriteIndex >= sprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = sprites[spriteIndex];

        }
        else if (flapAnimationRun >= maxFlapAnimations && spriteIndex != 0)
        {
            spriteIndex++;
            if (spriteIndex >= sprites.Length)
            {
                spriteIndex = 0;
            }
            spriteRenderer.sprite = sprites[spriteIndex];
        }
        else
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            FindObjectOfType<GameManager>().GameOver();
        }
        else if (other.gameObject.tag == "Scoring") 
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }

}
