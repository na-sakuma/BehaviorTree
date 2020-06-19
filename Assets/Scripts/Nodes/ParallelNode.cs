using UnityEngine;

/// <summary>
/// 子を同時に実行し、全ての子がSuccessならSuccessを返す、それ以外はFailureを返す
/// ブランチノード
/// </summary>
public class ParallelNode : BranchNode {

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
        int successCounter = 0;
        int runningCounter = 0;
        foreach (BaseNode child in _childNodeList) {
            // すでに実行結果が出ているものはスキップ
            if (child.status == NodeStatus.SUCCESS) {
                successCounter++;
                continue;
            }
             child.OnRunning();
            if (child.status != NodeStatus.FAILURE) {
                if (child.status == NodeStatus.RUNNING) {
                    runningCounter++;
                } else if (child.status == NodeStatus.SUCCESS) {
                    successCounter++;
                    child.OnFinish();
                }
                _childIndex++;
                continue;
            }
            // 子が1つでもFailureなら他をWaitingにしてFailureを返す
            result = NodeStatus.FAILURE;
            child.OnFinish();
            for (int i = 0; i < _childNodeList.Count; i++) {
                if (i == _childIndex) {
                    continue;
                }
                _childNodeList[i].status = NodeStatus.WAITING;
            }
            return result;

        }
        if (runningCounter > 0) {
            result = NodeStatus.RUNNING;
        // 全ての子がSuccessならSuccessを返す
        } else if (successCounter == _childNodeList.Count) {
            result = NodeStatus.SUCCESS;
        }
        return result;
    }
}
