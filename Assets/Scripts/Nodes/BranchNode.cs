using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブランチとなれるノード
/// </summary>
public class BranchNode : BaseNode {
    protected List<BaseNode> _childNodeList = new List<BaseNode>();
    protected int _childIndex = 0;

    /// <summary>
    /// ノード起動時処理
    /// </summary>
    public override void OnStart() {
        base.OnStart();
        _childIndex = 0;
        if (_childNodeList.Count == 0) {
            Debug.LogError("not child");
            return;
        }
    }

    /// <summary>
    /// 子のステータスを評価する
    /// </summary>
    /// <returns>status</returns>
    protected virtual NodeStatus EvaluateChild() {
        return NodeStatus.WAITING;
    }

    /// <summary>
    /// 子を追加する
    /// </summary>
    /// <param name="child">追加する子ノード</param>
    public virtual void AddChild(BaseNode child) {
        _childNodeList.Add(child);
    }

    /// <summary>
    /// 子ノードリストを取得
    /// </summary>
    /// <returns>_childNodeList</returns>
    public virtual List<BaseNode> GetChildList() {
        return _childNodeList;
    }
}
