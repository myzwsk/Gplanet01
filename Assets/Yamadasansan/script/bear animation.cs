using UnityEngine;
using System.Collections; // コルーチンに必要
using UnityEngine.SceneManagement;

public class bearanimation : MonoBehaviour
{
    private Animator anim;
    public BossHp BossHp;
    public GoalArea GoalArea;
    bool change;

    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 1.0f;// フェード時間（秒）
    [SerializeField] private string nextSceneName = "NextLevelScene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("level 2", false);
        anim.SetBool("level 3", false);
        anim.SetBool("level 4", false);
        anim.SetBool("Destroy", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (BossHp.state == BossHp.State.gear2)
        {
            anim.SetBool("level 2", true);

        }

        if (BossHp.state == BossHp.State.gear3)
        {
            anim.SetBool("level 3", true);
            anim.SetBool("level 2", false);
        }

        if (BossHp.state == BossHp.State.gear4)
        {
            anim.SetBool("level 4", true);
            anim.SetBool("level 3", false);
        }

        if (BossHp.state == BossHp.State.normal)
        {
            anim.SetBool("level 2", false);
            anim.SetBool("level 3", false);
            anim.SetBool("level 4", false);
        }


        if (BossHp.Hp <= 0)
        {
            anim.SetBool("Destroy", true);
            StartCoroutine(FadeAndLoadScene());
        }


    }


    public IEnumerator FadeAndLoadScene()
    {
        float timer = 0f;

        // フェードアウト処理
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Alpha値を 0 から 1 に徐々に変化
            fadePanel.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null; // 1フレーム待つ
        }

        // 完全に白くなったらシーンをロード
        SceneManager.LoadScene(nextSceneName);
    }
}
