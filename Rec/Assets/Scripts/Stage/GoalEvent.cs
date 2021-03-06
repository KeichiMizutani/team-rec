using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class GoalEvent: IStageEvent
{
    /// <summary>
    /// 一体分の敵情報を格納するための構造体
    /// </summary>
    [System.Serializable]
    public struct EnemyParameter
    {
        public GameObject obj;
        /// <summary>
        /// 敵の場所を指定するためのもの
        /// 空オブジェクトでもよい
        /// </summary>
        public Transform Marker;
        public EnemyType type;
        public EnemyMove move;
    }

    /// <summary>
    /// 召喚する敵オブジェクトに追加し、Destroyされるタイミングを通知する。
    /// </summary>
    private class DestroyObserver : MonoBehaviour
    {
        public UnityEvent OnDestroyed = new UnityEvent();

        private void OnDestroy()
        {
            OnDestroyed.Invoke();
        }
    }
    
    private EnemyParameter[] enemies;
    private Vector3[] positions;
    private int numAliveEnemies;

    /// <summary>
    /// このイベントで召喚される敵をすべて倒せばステージクリアとなる
    /// </summary>
    /// <param name="enemies">敵情報</param>
    /// <param name="positions">敵の座標情報　座標のみ敵情報と別に取得する</param>
    /// <param name="time">発動時刻</param>
    public GoalEvent(IEnumerable<EnemyParameter> enemies, float time)
    {
        this.Time = time;
        this.enemies = enemies.ToArray();
        this.positions = enemies.Select(x => x.Marker.position).ToArray();
        numAliveEnemies = 0;
    }

    public void Call()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];
            var position = positions[i];

            // 敵オブジェクト生成
            var enemyInstance = GameObject.Instantiate(enemy.obj, position, Quaternion.identity);
            var enemyClass = enemyInstance.GetComponent<Enemy>();
            enemyClass.enemyType = enemy.type;
            enemyClass.enemyMove = enemy.move;

            // オブジェクトの破壊を検知するコンポーネントを敵オブジェクトに付与
            enemyInstance.AddComponent<DestroyObserver>();
            enemyInstance.GetComponent<DestroyObserver>().OnDestroyed.AddListener(CountEnemyDeath);
        }
        numAliveEnemies = enemies.Length;
    }

    /// <summary>
    /// 召喚した敵が倒されたときに呼び出されるイベント
    /// </summary>
    private void CountEnemyDeath()
    {
        --numAliveEnemies;
        if (numAliveEnemies == 0)
        {
            StageClear();
        }
    }

    /// <summary>
    /// ステージクリア時に呼び出す
    /// </summary>
    private void StageClear()
    {
        // 実装未定
        Debug.Log("Stage Clear");
    }

    public float Time { get; set; }
}
