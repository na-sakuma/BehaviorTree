using UnityEngine;

/// <summary>
/// 子が1つでもSuccessを返したらそこでSuccessを返す。全ての子がFailureならFailureを返す。
/// ブランチノード
/// </summary>
public class SelectorNode : BranchNode {

    /// <summary>
    /// ノード実行中のステータスを返す
    /// </summary>
    /// <returns>status</returns>
    public override NodeStatus OnRunning() {
        base.OnRunning();
        status = EvaluateChild();
        return status;
    }

    /// <summary>
    /// 子のステータスを評価する
    /// </summary>
    /// <returns>status</returns>
    protected override NodeStatus EvaluateChild() {
        NodeStatus result = NodeStatus.WAITING;
        int failureCounter = 0;
        foreach (BaseNode child in _childNodeList) {
            // すでに実行結果が出ているものはスキップ
            if (child.status == NodeStatus.SUCCESS || child.status == NodeStatus.FAILURE) {
                if (child.status == NodeStatus.FAILURE) {
                    failureCounter++;
                    _childIndex++;
                }
                continue;
            }
            child.OnRunning();
            if (child.status != NodeStatus.SUCCESS) {
                _childIndex++;
                if (child.status == NodeStatus.FAILURE) {
                    failureCounter++;
                    child.OnFinish();
                }
                continue;
            }
            // 1つでもSuccessなら、他をWatingにさせて終了
            child.OnFinish();
            result = child.status;
            for (int i = 0; i < _childNodeList.Count; i++) {
                if (i == _childIndex) {
                    continue;
                }
                _childNodeList[i].status = NodeStatus.WAITING;
            }
            return result;
        }
        // 全ての子がfailureならfailureを返す
        if (failureCounter >= _childNodeList.Count) {
            result = NodeStatus.FAILURE;
        } else {
            result = NodeStatus.RUNNING;
        }
        return result;
    }
}
