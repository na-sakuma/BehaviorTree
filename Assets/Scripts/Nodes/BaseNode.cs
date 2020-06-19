using UnityEngine;

/// <summary>
/// 各ノードのベースとなるもの
/// </summary>
public class BaseNode {
    // ステータス
    private NodeStatus _status = NodeStatus.WAITING;
    public NodeStatus status {
        get { return _status; }
        set { _status = value; }
    }
    // 名前
    public string name { get; set; }


    /// <summary>
    /// ノード起動時処理
    /// </summary>
    public virtual void OnStart() {
        if (_status != NodeStatus.WAITING) {
            Debug.LogError("Status is not waiting : " + name);
            return;
        }
        _status = NodeStatus.RUNNING;
        Debug.Log("OnStart : " + name + ", status : " + _status);
    }

    /// <summary>
    /// ノード実行中のステータスを返す
    /// </summary>
    /// <returns>status</returns>
    public virtual NodeStatus OnRunning() {
        if (_status == NodeStatus.WAITING) {
            OnStart();
        }
        Debug.Log("OnRunning : " + name + ", status : " + _status);
        if (_status == NodeStatus.SUCCESS || _status == NodeStatus.FAILURE) {
            OnFinish();
        }
        return _status;
    }

    /// <summary>
    /// ノード実行完了時処理
    /// </summary>
    public virtual void OnFinish() {
        if (_status != NodeStatus.SUCCESS && _status != NodeStatus.FAILURE) {
            Debug.LogError("まだ終了していません : " + name);
            return;
        }
        Debug.Log("OnFinish : " + name + ", status : " + _status);
    }
}

/// <summary>
/// ノードのステータス
/// </summary>
public enum NodeStatus {
    // 待機中
    WAITING,
    // 成功
    SUCCESS,
    // 失敗
    FAILURE,
    // 実行中
    RUNNING,
}