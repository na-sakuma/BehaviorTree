using UnityEngine;

public class ExampleTree : MonoBehaviour {
    [SerializeField]
    private int probabilitySkillA = 0;
    private BehaviorTreeController _behaviorTreeController;
    private int _myHp = 150;
    private int _enemyHp = 120;

    void Start() {
        _behaviorTreeController = new BehaviorTreeController();

        // rootとなるSequencer
        SequencerNode rootNode = new SequencerNode();
        rootNode.name = "rootノード";

        // 出発
        ActionNode departure = new ActionNode();
        departure.name = "出発する";
        departure.SetRunningFunc(() => {
            Debug.LogError("出発");
            return NodeStatus.SUCCESS;
        });

        // HP確認のDecorator
        DecoratorNode confirmationHp = new DecoratorNode();
        confirmationHp.name = "HP確認するのDecorator";
        confirmationHp.SetConditionFunc(() => {
            return _myHp >= 100 ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        });

        // 敵に寄る
        ActionNode enemyApproach = new ActionNode();
        enemyApproach.name = "敵に寄るアクションノード";
        enemyApproach.SetRunningFunc(() => {
            Debug.LogError("敵に寄る");
            return NodeStatus.SUCCESS;
        });

        // HP確認のDecoratorの子供登録
        confirmationHp.AddChild(enemyApproach);

        // 友達2人を呼ぶParallelNode
        ParallelNode callFriendAB = new ParallelNode();
        callFriendAB.name = "友達2人を呼ぶParallelNode";

        // 友達A
        ActionNode friendA = new ActionNode();
        friendA.name = "友達Aを呼ぶ";
        friendA.SetRunningFunc(() => {
            Debug.LogError("友達A");
            return NodeStatus.SUCCESS;
        });

        // 友達B
        ActionNode friendB = new ActionNode();
        friendB.name = "友達Bを呼ぶ";
        friendB.SetRunningFunc(() => {
            Debug.LogError("友達B");
            return NodeStatus.SUCCESS;
        });

        // 友達2人を呼ぶParallelNodeの子供登録
        callFriendAB.AddChild(friendA);
        callFriendAB.AddChild(friendB);

        // スキルを繰り返し行うRepeaterNode
        RepeaterNode skillRepeater = new RepeaterNode();
        skillRepeater.name = "スキルを繰り返し行うRepeaterNode";

        // スキルを選択するSelector
        SelectorNode selectSkill = new SelectorNode();
        selectSkill.name = "スキルを選択するSelector";

        // スキルAの発動を確認するDecorator
        DecoratorNode triggerSkillA = new DecoratorNode();
        triggerSkillA.name = "スキルAの発動を確認するDecorator";
        triggerSkillA.SetConditionFunc(() => {
            int probability = Mathf.Clamp(probabilitySkillA, 0, 100);
            int random = Random.Range(0, 100);
            return probability > random ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        });

        // スキルA
        ActionNode skillA = new ActionNode();
        skillA.name = "skillA";
        skillA.SetRunningFunc(() => {
            Debug.LogError("skillA");
            _enemyHp -= 50;
            return NodeStatus.SUCCESS;
        });

        // スキルAの発動を確認するDecoratorの子供登録
        triggerSkillA.AddChild(skillA);


        // スキルB
        ActionNode skillB = new ActionNode();
        skillB.name = "skillB";
        skillB.SetRunningFunc(() => {
            Debug.LogError("skillB");
            _enemyHp -= 60;
            return NodeStatus.SUCCESS;
        });

        // スキルを選択するSelectorの子供登録
        selectSkill.AddChild(triggerSkillA);
        selectSkill.AddChild(skillB);

        // スキルを繰り返し行うRepeaterNodeの子供登録
        skillRepeater._repeatNum = 2;
        skillRepeater.AddChild(selectSkill);

        // 敵の生存を確認するSelector
        SelectorNode enemySurvial = new SelectorNode();
        enemySurvial.name = "敵の生存を確認するSelector";

        // 敵が死んでいるか確認するDecorator
        DecoratorNode enemyDied = new DecoratorNode();
        enemyDied.name = "敵が死んでいるか確認するDecorator";
        enemyDied.SetConditionFunc(() => {
            return _enemyHp <= 0 ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        });

        // 敵が死んでいる
        ActionNode died = new ActionNode();
        died.name = "敵が死んでいる";
        died.SetRunningFunc(() => {
            Debug.LogError("End1");
            Debug.LogError("EnemyHp : " + _enemyHp);
            return NodeStatus.SUCCESS;
        });

        // 敵が死んでいるか確認するDecoratorの子供登録
        enemyDied.AddChild(died);

        // 敵が生きているか確認するDecorator
        DecoratorNode enemyAlive = new DecoratorNode();
        enemyAlive.name = "敵が生きているか確認するDecorator";
        enemyAlive.SetConditionFunc(() => {
            return _enemyHp > 0 ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        });

        // 敵が生きている
        ActionNode alive = new ActionNode();
        alive.name = "敵が生きている";
        alive.SetRunningFunc(() => {
            Debug.LogError("End2");
            Debug.LogError("EnemyHp : " + _enemyHp);
            return NodeStatus.SUCCESS;
        });

        // 敵が生きているか確認するDecoratorの子供登録
        enemyAlive.AddChild(alive);

        // 敵の生存を確認するSelectorの子供登録
        enemySurvial.AddChild(enemyDied);
        enemySurvial.AddChild(enemyAlive);


        // rootノードの子供登録
        rootNode.AddChild(departure);
        rootNode.AddChild(confirmationHp);
        rootNode.AddChild(callFriendAB);
        rootNode.AddChild(skillRepeater);
        rootNode.AddChild(enemySurvial);

        // ツリー実行
        _behaviorTreeController.Initialize(rootNode);
        _behaviorTreeController.OnStart();
    }

    void Update() {
        _behaviorTreeController.OnRunning();
    }
}
