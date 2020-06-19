using System;
using UnityEngine;

/// <summary>
/// 条件付きノード
/// リーフノード
/// </summary>
public class ConditionNode : BaseNode {
    // 判定する処理
    private Func<NodeStatus> _conditionFunc = null;

    /// <summary>
    /// 判定する処理の設定
    /// </summary>
    /// <param name="func">判定する処理</param>
    public void SetConditionFunc(Func<NodeStatus> func) {
        _conditionFunc = func;
    }

    /// <summary>
    /// ノード実行中のステータスを返す
    /// </summary>
    /// <returns>status</returns>
    public override NodeStatus OnRunning() {
        if (_conditionFunc == null) {
            Debug.LogError("_conditionFunc is null : " + name);
            return NodeStatus.FAILURE;
        }
        base.OnRunning();
        status = _conditionFunc();
        return status;
    }
}
