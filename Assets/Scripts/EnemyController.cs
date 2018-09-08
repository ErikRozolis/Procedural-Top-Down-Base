using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    Character enemyScript;
    Transform player;
    bool alerted;
    GameController gc; 

    void Start()
    {
        alerted = false;
        player = GameObject.Find("PriestCharacter").transform;
        SpriteRenderer[] srList = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        enemyScript = GetComponent<Character>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        foreach (var sr in srList)
        {
            sr.sortingLayerName = "Player";
        }
    }

    void Update()
    {
        var scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x);

        if (player.position.x < transform.position.x) scale.x *= -1;

        transform.localScale = scale;
    }

    public void Alert()
    {
        alerted = true;
        enemyScript.Animator.SetBool("Walk", true);
        gc.AddAlertedEnemy(gameObject);
    }

    private void FixedUpdate()
    {
        if (alerted)
        {
            Vector2 dir = player.position - this.gameObject.transform.position;
            rb.velocity = dir.normalized * 10;
        }
    }
}
