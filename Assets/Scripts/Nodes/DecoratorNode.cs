using System;
using UnityEngine;

/// <summary>
/// 条件を満たしているなら子のstatusを、そうでないならFailureを返す
/// 子を1つしか持つことが出来ない
/// ブランチノード
/// </summary>
public class DecoratorNode : BranchNode {
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
        base.OnRunning();
        if (_childNodeList.Count > 1) {
            Debug.LogError("DecoratorNode can only have one");
            return NodeStatus.FAILURE;
        }
        if (_conditionFunc == null) {
            Debug.LogError("_conditionFunc is null : " + name);
            return NodeStatus.FAILURE;
        }
        status = EvaluateChild();
        if (status == NodeStatus.RUNNING) {
            OnRunning();
        }
        return status;
    }

    /// <summary>
    /// 子のステータスを評価する
    /// </summary>
    /// <returns>status</returns>
    protected override NodeStatus EvaluateChild() {
        NodeStatus result = NodeStatus.WAITING;
        status = _conditionFunc();
        // 判定が通らなかったら強制的にFailureを返す
        if (status == NodeStatus.FAILURE) {
            result = NodeStatus.FAILURE;
            return result;
        // 子のstatusを返す
        } else if (status == NodeStatus.SUCCESS) {
            result = _childNodeList[0].OnRunning();
        }
        if (result == NodeStatus.SUCCESS || result == NodeStatus.FAILURE) {
            _childNodeList[0].OnFinish();
        }
        return result;
    }
}
