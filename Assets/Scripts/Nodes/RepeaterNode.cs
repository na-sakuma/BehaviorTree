using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 規定の回数を子ノードが返すまでRunningを返す。条件を満たしたらSuccessを返す。
/// ブランチノード
/// </summary>
public class RepeaterNode : BranchNode {
    public int _repeatNum = 0;
    private int _repeatCounter = 0;
    public override NodeStatus OnRunning() {
        base.OnRunning();

        if (_repeatNum == 0) {
            Debug.LogError("_repeatNum is 0");
            return NodeStatus.FAILURE;
        }
        status = EvaluateChild();
        if (status == NodeStatus.RUNNING) {
            OnRunning();
        }
        return status;
    }


    protected override NodeStatus EvaluateChild() {
         NodeStatus result = NodeStatus.WAITING;
        int finishCounter = 0;
        foreach (BaseNode child in _childNodeList) {
            // すでに実行結果が出ているものはスキップ
            if (child.status == NodeStatus.FAILURE || child.status == NodeStatus.SUCCESS) {
                finishCounter++;
                continue;
            }
            child.OnRunning();
            if (child.status == NodeStatus.FAILURE || child.status == NodeStatus.SUCCESS) {
                child.OnFinish();
                finishCounter++;
            }
        }
        // まだ子がRunning中ならRunningを返す
        if (finishCounter < _childNodeList.Count) {
            result = NodeStatus.RUNNING;
            return result;
        }
        _repeatCounter++;
        // 規定回数繰り返したならSuccessを返す
        if (_repeatCounter >= _repeatNum) {
            result = NodeStatus.SUCCESS;
            return result;
        }
        // 規定回数未満なら全ての子をWaitingに戻し、再び処理する
        result = NodeStatus.RUNNING;
        ChildWaiting(_childNodeList);
        return result;
    }

    /// <summary>
    /// 子ノードをWaitingにする
    /// </summary>
    /// <param name="childNodeList">子ノードリスト</param>
    private void ChildWaiting(List<BaseNode> childNodeList) {
        foreach (BaseNode child in childNodeList) {
            child.status = NodeStatus.WAITING;
            BranchNode branchNode = child as BranchNode;
            if (branchNode == null) {
                continue;
            }
            ChildWaiting(branchNode.GetChildList());
        }
    }
}
