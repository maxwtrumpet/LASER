using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamManager : MonoBehaviour
{
    public float goal_thickness = 2.0f;
    bool up = true;
    public float grow_time;
    float remaining_time;
    public int damage;
    [SerializeField] Texture[] frames;
    int cur_index = 0;
    float angle;
    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        transform.localScale = new Vector3(0.0f, goal_thickness, 1.0f);
        grow_time = 0.2f;
        remaining_time = grow_time;
        angle = Mathf.Atan(transform.position.y / transform.position.x);
    }

    IEnumerator Animate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.0167f);
            cur_index = (cur_index + 13) % 64;
            mat.SetTexture("_MainTex", frames[cur_index]);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    void Update()
    {
        if (transform.localScale.x < 20.0f && up)
        {
            remaining_time -= Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Min(20.0f,(grow_time - remaining_time) / grow_time * 20.0f), goal_thickness, 1.0f);
            transform.position = new Vector3(Mathf.Cos(angle) * (transform.localScale.x / 2.0f + 2.25f), Mathf.Sin(angle) * (transform.localScale.x / 2.0f + 2.25f), -0.01f);
            if (transform.localScale.x != 0.0f) mat.SetTextureScale("_MainTex", new Vector2(transform.localScale.x / 20.0f, 1.0f));
        }
        else if (up)
        {
            up = false;
        }
        else
        {
            remaining_time += Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Max(0.0f, (grow_time - remaining_time) / grow_time * 20.0f), goal_thickness, 1.0f);
            transform.position = new Vector3(Mathf.Cos(angle) * (22.25f - transform.localScale.x / 2.0f), Mathf.Sin(angle) * (22.25f - transform.localScale.x / 2.0f), -0.01f);
            if (transform.localScale.x != 0.0f) mat.SetTextureScale("_MainTex", new Vector2(transform.localScale.x / 20.0f, 1.0f));
            if (remaining_time >= grow_time)
            {
                GameObject.FindGameObjectWithTag("enemy").GetComponent<EnemyManager>().ResetBonus();
                Destroy(gameObject);
            }
        }
    }

}
