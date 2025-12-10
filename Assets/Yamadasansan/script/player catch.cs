
using Unity.VisualScripting;
using UnityEngine;

public class playercatch : MonoBehaviour
{
    // プレイヤーのTransform
    public Transform playerTransform;

    // プレイヤーのスクリプトコンポーネントを格納する変数
    // 🚨 ここを、実際のプレイヤーのスクリプト名に置き換えてください (例: private PlayerController playerScript;)
    private playerhandcopy playerHandScript;

    // ----------------------------------------------------
    // ユーザー要求のブール値 (外部参照可能、内部でのみ設定可能)
    // ----------------------------------------------------
    public bool isPushing { get; private set; }
    public bool isPulling { get; private set; }
    public bool isStationary { get; private set; }

    // 内部で使用する状態管理
    private enum PushPullState { Pushing, Pulling, Stationary }
    private PushPullState currentState = PushPullState.Stationary;

    // 内部変数
    private Transform objectTransform;
    private Vector3 currentObjectPosition;
    private Vector3 previousObjectPosition;
    private const float MovementThreshold = 0.005f;

    // 🔴 追加: 状態が安定したと見なすために必要な最短時間 (例: 0.05秒)
    private const float StateStabilityTime = 0.05f;

    // 🔴 追加: 現在の状態になった時間
    private float timeEnteredCurrentState;



    void Start()
    {
        objectTransform = transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned. Please assign the Player object in the Inspector.");
            enabled = false;
            return;
        }

        // プレイヤーのスクリプトコンポーネントを取得
        playerHandScript = playerTransform.GetComponent<playerhandcopy>();

        if (playerHandScript == null)
        {
            Debug.LogError("PlayerClimbController component not found on Player. Check the script name.");
            enabled = false;
            return;
        }

        currentObjectPosition = objectTransform.position;
        previousObjectPosition = objectTransform.position;

        // 初期状態としてStationaryを設定
        SetBooleanStates(PushPullState.Stationary);
    }

    void Update()
    {
        // ----------------------------------------------------
        // 1. プレイヤーがオブジェクトを掴んでいるか（isClimbing == trueか）をチェック
        // ----------------------------------------------------
        if (!playerHandScript.isGrabbing)
        {
            // 掴んでいない場合、状態を「静止」にして処理を終了
            SetBooleanStates(PushPullState.Stationary);

            // 掴んでいない間も位置を更新し、次回掴んだときに大きな移動量とならないようにする
            previousObjectPosition = objectTransform.position;
            return;
        }

        // ----------------------------------------------------
        // 2. 掴んでいる場合、押す/引くの判定ロジックを実行
        // ----------------------------------------------------

        currentObjectPosition = objectTransform.position;

        // 🔴 追加: 次の状態を保持する変数を必ず初期化する
        PushPullState nextState = PushPullState.Stationary;

        Vector3 movement = currentObjectPosition - previousObjectPosition;
        Debug.Log($"[{gameObject.name} Detector]: Movement Magnitude: {movement.magnitude}");
        // オブジェクトが静止しているか判定
        if (movement.magnitude < MovementThreshold)
        {
            SetBooleanStates(PushPullState.Stationary);
            Debug.Log($"[{gameObject.name} Detector]: State is Stationary (Too slow).");
        }
        else
        {
            Debug.Log($"[{gameObject.name} Detector]: Moving! Analyzing X/Z axis.");
            // X軸とZ軸の移動量を比較し、主軸を決定
            float absMovementX = Mathf.Abs(movement.x);
            float absMovementZ = Mathf.Abs(movement.z);

            if (absMovementX > absMovementZ)
            {
                nextState = HandleXAxisMovement(movement.x);

            }
            else
            {
                nextState = HandleZAxisMovement(movement.z);
            }
        }

        // 次のフレームのために現在の位置を保存
        previousObjectPosition = currentObjectPosition;

        // 最後にブール値を現在のcurrentStateに基づいて設定
        SetBooleanStates(currentState);

        // 🔴 SetBooleanStatesに計算された次の状態を渡し、遅延判定をさせる
        SetBooleanStates(nextState);
    }

    // --- 状態に基づいてブール値を設定するヘルパー関数 ---
    private void SetBooleanStates(PushPullState state)
    {
        if (state != currentState)
        {
            // 🔴 新しい状態への切り替えを遅延させる判定
            // 以前の状態になってから、十分な時間が経過しているかチェック
            if (Time.time < timeEnteredCurrentState + StateStabilityTime)
            {
                // まだ安定時間内にない場合は、状態の切り替えをスキップ（以前の状態を維持）
                return;
            }

            // 安定時間が経過した場合、新しい状態に切り替える
            currentState = state;
            timeEnteredCurrentState = Time.time; // 状態が変更された時間を更新
        }

        isPushing = (currentState == PushPullState.Pushing);
        isPulling = (currentState == PushPullState.Pulling);
        isStationary = (currentState == PushPullState.Stationary);

        // Debug.Log($"State: {currentState}, Push: {isPushing}, Pull: {isPulling}, Stationary: {isStationary}");
    }

    // --- X軸・Z軸判定ロジックは変更なし ---

    private PushPullState HandleXAxisMovement(float moveX)
    {
        // ... (省略: 判定ロジックは前述の通り)
        if (moveX < 0) // 左 (-)
        {
            if (objectTransform.position.x < playerTransform.position.x)
            { return PushPullState.Pushing; }
            else
            { return  PushPullState.Pulling; }
        }
        else // 右 (+)
        {
            if (objectTransform.position.x > playerTransform.position.x)
            { return  PushPullState.Pushing; }
            else
            { return PushPullState.Pulling; }
        }
    }

    private PushPullState HandleZAxisMovement(float moveZ)
    {
        // ... (省略: 判定ロジックは前述の通り)
        if (moveZ < 0) // 奥 (-)
        {
            if (objectTransform.position.z < playerTransform.position.z)
            { return  PushPullState.Pushing; }
            else
            { return  PushPullState.Pulling; }
        }
        else // 手前 (+)
        {
            if (objectTransform.position.z > playerTransform.position.z)
            { return PushPullState.Pushing; }
            else
            { return PushPullState.Pulling; }
        }
    }
}





