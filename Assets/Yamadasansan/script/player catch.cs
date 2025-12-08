
using UnityEngine;

public class playercatch : MonoBehaviour
{
    // プレイヤーのTransform
    public Transform playerTransform;

    // プレイヤーのスクリプトコンポーネントを格納する変数
    // 🚨 ここを、実際のプレイヤーのスクリプト名に置き換えてください (例: private PlayerController playerScript;)
    private Player playerScript;

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
        playerScript = playerTransform.GetComponent<Player>();

        if (playerScript == null)
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
        if (!playerScript.isClimbing)
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
        Vector3 movement = currentObjectPosition - previousObjectPosition;

        // オブジェクトが静止しているか判定
        if (movement.magnitude < MovementThreshold)
        {
            SetBooleanStates(PushPullState.Stationary);
        }
        else
        {
            // X軸とZ軸の移動量を比較し、主軸を決定
            float absMovementX = Mathf.Abs(movement.x);
            float absMovementZ = Mathf.Abs(movement.z);

            if (absMovementX > absMovementZ)
            {
                HandleXAxisMovement(movement.x);
            }
            else
            {
                HandleZAxisMovement(movement.z);
            }
        }

        // 次のフレームのために現在の位置を保存
        previousObjectPosition = currentObjectPosition;

        // 最後にブール値を現在のcurrentStateに基づいて設定
        SetBooleanStates(currentState);
    }

    // --- 状態に基づいてブール値を設定するヘルパー関数 ---
    private void SetBooleanStates(PushPullState state)
    {
        currentState = state;

        isPushing = (currentState == PushPullState.Pushing);
        isPulling = (currentState == PushPullState.Pulling);
        isStationary = (currentState == PushPullState.Stationary);

        // Debug.Log($"State: {currentState}, Push: {isPushing}, Pull: {isPulling}, Stationary: {isStationary}");
    }

    // --- X軸・Z軸判定ロジックは変更なし ---

    private void HandleXAxisMovement(float moveX)
    {
        // ... (省略: 判定ロジックは前述の通り)
        if (moveX < 0) // 左 (-)
        {
            if (objectTransform.position.x < playerTransform.position.x)
            { currentState = PushPullState.Pushing; }
            else
            { currentState = PushPullState.Pulling; }
        }
        else // 右 (+)
        {
            if (objectTransform.position.x > playerTransform.position.x)
            { currentState = PushPullState.Pushing; }
            else
            { currentState = PushPullState.Pulling; }
        }
    }

    private void HandleZAxisMovement(float moveZ)
    {
        // ... (省略: 判定ロジックは前述の通り)
        if (moveZ < 0) // 奥 (-)
        {
            if (objectTransform.position.z < playerTransform.position.z)
            { currentState = PushPullState.Pushing; }
            else
            { currentState = PushPullState.Pulling; }
        }
        else // 手前 (+)
        {
            if (objectTransform.position.z > playerTransform.position.z)
            { currentState = PushPullState.Pushing; }
            else
            { currentState = PushPullState.Pulling; }
        }
    }
}





