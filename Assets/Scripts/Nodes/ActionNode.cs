using System;
using UnityEngine;

/// <summary>
/// 実際の行動をするノード
/// リーフノード
/// </summary>
public class ActionNode : BaseNode {
    // 実行する処理
    private Func<NodeStatus> _runningFunc = null;

    /// <summary>
    /// 実行する処理の設定
    /// </summary>
    /// <param name="func">実行する処理</param>
    public void SetRunningFunc(Func<NodeStatus> func) {
        _runningFunc = func;
    }

    /// <summary>
    /// ノード実行中のステータスを返す
    /// </summary>
    /// <returns>status</returns>
    public override NodeStatus OnRunning() {
        if (_runningFunc == null) {
            Debug.LogError("_runningFunc is null : " + name);
            return NodeStatus.FAILURE;
        }
        base.OnRunning();
        status = _runningFunc();
        return status;
    }
}
