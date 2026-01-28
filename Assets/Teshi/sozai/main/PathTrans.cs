using UnityEngine;

public class PathTrans : MonoBehaviour
{
    public float startDelay = 5f;
    public float cool = 3f;
    public float speed = 3f;
    public float rotateSpeed = 180f;

    public Vector3 Pos2 = new Vector3(0, 2, 0);
    public Vector3 Lota2 = new Vector3(0, 0, 90);
    public Vector3 Pos3 = new Vector3(-7, 0, 0);

    private float timer = 0f;

    private Vector3 defaultPos;
    private Quaternion defaultRot;
    private Quaternion targetRot;

    private enum State
    {
        WaitStart,
        MoveToPos2,
        RotateToLota2,
        MoveToPos3,
        MoveBackToPos2,
        RotateBack,
        MoveBackToDefault,
        CoolWait
    }

    private State state = State.WaitStart;

    void Start()
    {
        defaultPos = transform.localPosition;   // ★ ローカル座標で保存
        defaultRot = transform.localRotation;
        targetRot = Quaternion.Euler(Lota2);
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        switch (state)
        {
            case State.WaitStart:
                timer += Time.deltaTime;
                if (timer >= startDelay)
                {
                    timer = 0f;
                    state = State.MoveToPos2;
                }
                break;

            case State.MoveToPos2:
                MoveTo(defaultPos + Pos2, State.RotateToLota2);
                break;

            case State.RotateToLota2:
                RotateTo(targetRot, State.MoveToPos3);
                break;

            case State.MoveToPos3:
                MoveTo(defaultPos + Pos3, State.MoveBackToPos2);
                break;

            case State.MoveBackToPos2:
                MoveTo(defaultPos + Pos2, State.RotateBack);
                break;

            case State.RotateBack:
                RotateTo(defaultRot, State.MoveBackToDefault);
                break;

            case State.MoveBackToDefault:
                MoveTo(defaultPos, State.CoolWait);
                break;

            case State.CoolWait:
                timer += Time.deltaTime;
                if (timer >= cool)
                {
                    timer = 0f;
                    state = State.MoveToPos2;
                }
                break;
        }
    }

    // ローカル移動
    void MoveTo(Vector3 target, State next)
    {
        transform.localPosition =
            Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, target) < 0.01f)
        {
            state = next;
        }
    }

    // 回転処理（ローカル回転）
    void RotateTo(Quaternion target, State next)
    {
        transform.localRotation =
            Quaternion.RotateTowards(transform.localRotation, target, rotateSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.localRotation, target) < 0.5f)
        {
            state = next;
        }
    }
}
