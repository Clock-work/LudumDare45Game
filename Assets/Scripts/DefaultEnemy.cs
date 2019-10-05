﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : MonoBehaviour
{
    public static List<DefaultEnemy> enemies = new List<DefaultEnemy>();

    private Rigidbody2D m_rigidbody;
    private CircleCollider2D m_collider;
    private int m_health = 1;
    private int m_maxHealth = 1;
    private float m_rotationSpeed;
    private float m_startDegrees;

    //returns false if no free position was found
    public static bool findNewFreePosition(float width, float height, out Vector2 position)
    {
        var minPos = BoundsManager.getInternalMinPos();
        var maxPos = BoundsManager.getInternalMaxPos();
        float x = Random.Range(minPos.x + width, maxPos.x - width);
        float y = Random.Range(minPos.y + height, maxPos.y - height);



        position = new Vector2(x, y);
        return true;
    }

    //returns null if there is no space for another enemy | creates a new defaultenemy
    public static DefaultEnemy createRandomEnemy()
    {
        float widthHeight = Random.Range(3f, 5f);

        if(!findNewFreePosition(widthHeight, widthHeight, out Vector2 position))
        {
            return null;
        }

        float speed = Random.Range(2f, 20f);
        float targetX = Random.Range(-10f, 10f);
        float targetY = Random.Range(-10f, 10f);

        float rotationSpeed = Random.Range(-1.3f, 1.3f);
        if(rotationSpeed< 0.1f && rotationSpeed > -0.1f)
        {
            if(rotationSpeed>0)
            {
                rotationSpeed += 0.1f;
            }
            else
            {
                rotationSpeed -= 0.1f;
            }
        }
        float startDegrees = Random.Range(0.0f, 360.0f);

        return createNewEnemy(position.x, position.y, widthHeight, widthHeight, targetX, targetY, speed, rotationSpeed, startDegrees, "Prefabs/Enemies/DefaultEnemy");
    }

    public static DefaultEnemy createNewEnemy(float x, float y, float width, float height, float targetX, float targetY, float moveSpeed, float rotationSpeed, float startDegrees, string prefabName)
    {
        return createNewEnemy(new Vector2(x, y), new Vector2(width, height), new Vector2(targetX, targetY), moveSpeed, rotationSpeed, startDegrees, prefabName);
    }
    public static DefaultEnemy createNewEnemy(Vector2 position, Vector2 size, Vector2 direction, float moveSpeed, float rotationSpeed, float startDegrees, string prefabName)
    {
        var gameobject = Instantiate((GameObject)Resources.Load(prefabName), new Vector3(position.x, position.y, -1), Quaternion.identity);
        gameobject.transform.localScale = new Vector3(size.x, size.y, 1);
        var enemy = gameobject.GetComponent<DefaultEnemy>();
        enemy.m_rigidbody.velocity = moveSpeed * direction.normalized;
        enemy.m_rotationSpeed = rotationSpeed;
        enemy.transform.Rotate(new Vector3(0,0,1),startDegrees);
        enemy.onInit();
        return enemy;
    }

    //special values per subclass
    protected virtual void onInit()
    {
        m_maxHealth = 1;
    }

    //called after enemy is destroyed and removed
    protected virtual void onDeath()
    {

    }

    private void Awake()
    {
        if (!enemies.Contains(this))
        {
            enemies.Add(this);
        }
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        m_collider = this.GetComponent<CircleCollider2D>();
        m_rigidbody.isKinematic = false;
        m_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_health = m_maxHealth;
    }

    private void OnDestroy()
    {
        if(enemies.Contains(this))
        {
            enemies.Remove(this);
        }
        this.onDeath();
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_health<=0)
        {
            Destroy(this.gameObject);
        }
        this.transform.Rotate(new Vector3(0, 0, 1), m_rotationSpeed);
    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision: " + collision.transform.name);
    }




}
