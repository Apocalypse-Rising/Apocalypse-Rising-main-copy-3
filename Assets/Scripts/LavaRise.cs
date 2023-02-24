using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRise : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public float speed = 1f;
    private ComboCount comboGet;
    private int comboNum;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.Find("Player");
        comboGet = player.GetComponent<ComboCount>();
        comboNum = comboGet.comboNum;
    }

    // Update is called once per frame
    void Update()
    {
        comboNum = comboGet.comboNum;
        if (comboNum >= 5)
        {
            rb.velocity = new Vector2(0, speed * 0.5f);
        } else
        {
            rb.velocity = new Vector2(0, speed);
        }
        
    }
}
