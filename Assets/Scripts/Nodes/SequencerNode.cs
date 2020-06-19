using UnityEngine;

/// <summary>
/// 子が1つでもFailureを返したらそこでFailureを返す。全ての子がSuccessならSuccessを返す。
/// ブランチノード
/// </summary>
public class SequencerNode : BranchNode {

    /// <summary>
    /// ノード実行中のステータスを返す
    /// </summary>
    /// <returns>status</returns>
    public override NodeStatus OnRunning() {
        base.OnRunning();
        if (_childNodeList.Count <= _childIndex) {
            Debug.LogError("index is over");
            return NodeStatus.FAILURE;
        }
        NodeStatus childStatus = NodeStatus.WAITING;
        childStatus = _childNodeList[_childIndex].OnRunning();
        if (childStatus == NodeStatus.SUCCESS) {
            _childNodeList[_childIndex].OnFinish();
            _childIndex++;
        }
        status = EvaluateChild();
        return status;
    }

    /// <summary>
    /// 子のステータスを評価する
    /// </summary>
    /// <returns>status</returns>
    protected override NodeStatus EvaluateChild() {
        NodeStatus result = NodeStatus.WAITING;
        foreach (BaseNode child in _childNodeList) {
            // 1つでもFailureなら終了
            if (child.status == NodeStatus.FAILURE) {
                result = NodeStatus.FAILURE;
                break;
            } else if (child.status == NodeStatus.RUNNING || child.status == NodeStatus.WAITING) {
                result = NodeStatus.RUNNING;
                break;
            }
            result = NodeStatus.SUCCESS;
        }
        return result;
    }
}
