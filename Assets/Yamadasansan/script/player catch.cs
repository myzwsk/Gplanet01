
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using System.Collections; // 🔴 この行を追加！

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

    private const int StateStabilityFrames = 0;

    // 🔴 追加: 現在の状態になった時間
    private float timeEnteredCurrentState;

    private const float PositionTolerance = 0.1f; // 10cmの遊び

    // 🔴 追加: ニュートラルゾーンの移動量閾値 (MovementThresholdより少し大きい値)
    private const float NeutralMoveTolerance = 0.008f;

    // 🔴 追加: 状態切り替えのディレイが現在進行中であるかを示すフラグ
    private bool isDelayingStateSwitch = false;

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
        ProcessStateChange(PushPullState.Stationary);
    }

    void Update()
    {
        // ----------------------------------------------------
        // 1. プレイヤーがオブジェクトを掴んでいるか（isClimbing == trueか）をチェック
        // ----------------------------------------------------
        if (!playerHandScript.isGrabbing)
        {
            // 掴んでいない場合、状態を「静止」にして処理を終了
            ProcessStateChange(PushPullState.Stationary);

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
            if (currentState == PushPullState.Pushing || currentState == PushPullState.Pulling)
            {
                // Pushing/Pulling 状態を維持
                nextState = currentState;

                // 🚨 デバッグログ: 動きが閾値を下回ったが状態維持
                // Debug.Log($"Movement below threshold ({MovementThreshold}), but state maintained: {currentState}");
            }
            else
            {
                // Stationary の場合はそのまま Stationary
                nextState = PushPullState.Stationary;
            }
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
        ProcessStateChange(currentState);

        // 🔴 SetBooleanStatesに計算された次の状態を渡し、遅延判定をさせる
        ProcessStateChange(nextState);

        UpdateInternalBooleans();
    }

    // --- 状態に基づいてブール値を設定するヘルパー関数 ---

    /*private void SetBooleanStates(PushPullState newState)
    {
        if (newState == currentState)
        {
            // 状態が変わっていなくても、ブール値は毎フレーム更新しておく
            isPushing = (currentState == PushPullState.Pushing);
            isPulling = (currentState == PushPullState.Pulling);
            isStationary = (currentState == PushPullState.Stationary);
            return;
        }

        bool shouldApplyDelay = true;

        // 🔴 [例外ルール] Stationaryから他の状態への切り替えは即時実行する (アニメーション開始時の遅延を防ぐ)
        if (currentState == PushPullState.Stationary)
        {
            shouldApplyDelay = false;
        }

        // 🔴 [例外ルール] Stationaryへの切り替えは即時実行する (アニメーション終了時の遅延を防ぐ)
        if (newState == PushPullState.Stationary)
        {
            shouldApplyDelay = false;
        }

        if (shouldApplyDelay)
        {
            // Pushing <-> Pulling 間の切り替えの場合：ディレイチェック
            if (Time.time < timeEnteredCurrentState + StateStabilityTime)
            {
                // ディレイ時間内のため、切り替えをスキップ（古い状態を維持し、戻る）
                isPushing = (currentState == PushPullState.Pushing);
                isPulling = (currentState == PushPullState.Pulling);
                isStationary = (currentState == PushPullState.Stationary);
                return;
            }
        }

        // 4. 状態を更新

        // ディレイチェックをパスしたか、または例外ルール（即時実行）が適用された場合
        currentState = newState;
        timeEnteredCurrentState = Time.time; // 状態が変更された時間を更新

        // 5. ブール値を設定
        isPushing = (currentState == PushPullState.Pushing);
        isPulling = (currentState == PushPullState.Pulling);
        isStationary = (currentState == PushPullState.Stationary);
    }*/

    // --- X軸・Z軸判定ロジックは変更なし ---

    private void ProcessStateChange(PushPullState newState)
    {
        // 状態が変わらない、または現在ディレイ中は、即座に終了（ロック）
        if (newState == currentState || isDelayingStateSwitch)
        {
            UpdateInternalBooleans();
            return;
        }

        // 🔴 Stationary からの開始、または Stationary への戻りは即時実行
        if (currentState == PushPullState.Stationary || newState == PushPullState.Stationary)
        {
            // 即時更新
            currentState = newState;
        }
        // 🔴 Pushing <-> Pulling 間の切り替えはディレイ実行
        else
        {
            StartCoroutine(DelayedStateSwitch(newState));
        }

        UpdateInternalBooleans();
    }

    // 内部のブール値（isPushing, isPullingなど）を更新するヘルパーメソッド
    private void UpdateInternalBooleans()
    {
        isPushing = (currentState == PushPullState.Pushing);
        isPulling = (currentState == PushPullState.Pulling);
        isStationary = (currentState == PushPullState.Stationary);
    }

    // 状態を更新するヘルパーメソッド
    private void UpdateCurrentState(PushPullState newState)
    {
        currentState = newState;
        // timeEnteredCurrentState の更新は、このロジックでは不要になりました
    }

    // コルーチン: ディレイをかけて状態を切り替える
    private IEnumerator DelayedStateSwitch(PushPullState newState)
    {
        isDelayingStateSwitch = true; // ロック開始

        // 🔴 修正: WaitForFixedUpdate を使って物理フレーム数で待機
        for (int i = 0; i < StateStabilityFrames; i++)
        {
            yield return new WaitForFixedUpdate(); // 物理フレームの更新を待つ
        }

        // ロックが解除される直前に、状態をチェックして更新
        if (newState != currentState)
        {
            UpdateCurrentState(newState);
        }

        isDelayingStateSwitch = false; // ロック解除
    }

    private PushPullState HandleXAxisMovement(float moveX)
    {
        float relativeX = objectTransform.position.x - playerTransform.position.x;

        // 🔴 【ニュートラルゾーン判定】 
        // 現在の状態が Push/Pull であり、かつ今回の移動量が小さすぎる場合
        // (極端なフリッカーを無視し、現在の状態を維持する)
        if (Mathf.Abs(moveX) < NeutralMoveTolerance && currentState != PushPullState.Stationary)
        {
            return currentState;
        }

        if (relativeX > PositionTolerance) // オブジェクトがプレイヤーより右側にある
        {
            if (moveX > 0)
            { return PushPullState.Pushing; }
            else
            { return PushPullState.Pulling; }
        }
        else if (relativeX < -PositionTolerance) // オブジェクトがプレイヤーより左側にある
        {
            if (moveX < 0)
            { return PushPullState.Pushing; }
            else
            { return PushPullState.Pulling; }
        }

        // プレイヤーとオブジェクトが近接している (Tolerance範囲内)
        if (moveX > 0)
        { return PushPullState.Pushing; }
        else if (moveX < 0)
        { return PushPullState.Pushing; }

        return PushPullState.Stationary;
    }

    private PushPullState HandleZAxisMovement(float moveZ)
    {
        float relativeZ = objectTransform.position.z - playerTransform.position.z;

        // 🔴 【ニュートラルゾーン判定】
        if (Mathf.Abs(moveZ) < NeutralMoveTolerance && currentState != PushPullState.Stationary)
        {
            return currentState;
        }

        if (relativeZ > PositionTolerance) // オブジェクトがプレイヤーより手前（Z+）にある
        {
            if (moveZ > 0)
            { return PushPullState.Pushing; }
            else
            { return PushPullState.Pulling; }
        }
        else if (relativeZ < -PositionTolerance) // オブジェクトがプレイヤーより奥（Z-）にある
        {
            if (moveZ < 0)
            { return PushPullState.Pushing; }
            else
            { return PushPullState.Pulling; }
        }

        // プレイヤーとオブジェクトが近接している (Tolerance範囲内)
        if (moveZ > 0)
        { return PushPullState.Pushing; }
        else if (moveZ < 0)
        { return PushPullState.Pushing; }

        return PushPullState.Stationary;
    }
    
}





