using UnityEngine;

public class BehaviorTreeController {
    // 現在走っているノード
    private BaseNode _rootNode = null;
    // 最終結果のステータス
    private NodeStatus _resultStatus = NodeStatus.WAITING;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="node">rootNode</param>
    public void Initialize(BaseNode node) {
        _rootNode = node;
    }

    /// <summary>
    /// BehaviorTreeの起動
    /// </summary>
    public void OnStart() {
        Debug.Log("BehaviorTree start");
        if (_resultStatus != NodeStatus.WAITING) {
            Debug.LogError("status is waiting");
            return;
        }
        _resultStatus = NodeStatus.RUNNING;
        // ノード起動処理
        _rootNode.OnStart();
    }

    /// <summary>
    /// BehaviorTreeの更新処理
    /// </summary>
    public void OnRunning() {
        // BehaviorTreeの実行が完了している
        if (_resultStatus == NodeStatus.SUCCESS || _resultStatus == NodeStatus.FAILURE) {
            return;
        }
        if (_resultStatus == NodeStatus.WAITING) {
            Debug.LogError("status is waiting");
            return;
        }
        // ノード繰り返し起動処理
        _resultStatus = _rootNode.OnRunning();
        if (_resultStatus == NodeStatus.SUCCESS || _resultStatus == NodeStatus.FAILURE) {
            _rootNode.OnFinish();
            OnFinish();
            Debug.Log("BehaviorTree result : " + _rootNode.status);
        }
    }

    /// <summary>
    /// BehaviorTreeの完了処理
    /// </summary>
    public void OnFinish() {
        if (_resultStatus != NodeStatus.SUCCESS && _resultStatus != NodeStatus.FAILURE) {
            Debug.LogError("unexpected results are coming back. status is : " + _resultStatus);
            return;
        }
        Debug.Log("BehaviorTree finish");
        return;
    }
}
