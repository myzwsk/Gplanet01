using UnityEngine;

public class bearanimation : MonoBehaviour
{
    private Animator anim;
    public BossHp BossHp;
    bool change;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("level 2", false);
        anim.SetBool("level 3", false);
        anim.SetBool("level 4", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(BossHp.state == BossHp.State.gear2)
            {
            anim.SetBool("level 2", true);

        }
       
        if(BossHp.state == BossHp.State.gear3)
        {
            anim.SetBool("level 3", true);
            anim.SetBool("level 2", false);



        }

        if (BossHp.state == BossHp.State.gear4)
        {
            anim.SetBool("level 4", true);
            anim.SetBool("level 3", false);
        }

    }


    void LateUpdate()
    {
        // Z座標を強制的に0に固定し続ける
        Vector3 pos = transform.position;
        pos.z = 181f;
        transform.position = pos;
    }
}
