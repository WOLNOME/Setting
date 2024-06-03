using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject particlePrefab;
    public GameObject wallPrefab;
    public GameObject backPrefab;
    public GameObject clearText;
    //配列の宣言
    int[,] map;//二次元配列
    int[,] back;
    GameObject[,] field;//ゲーム管理用の配列
    GameObject[,] field2;
    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == null)
                {
                    continue;
                }

                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }

        }
        return new Vector2Int(-1, -1);
    }

    //移動可不可の判断メソッド
    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        //移動不可ならfalse
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        //壁に当たった際も同様
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Wall")
        {
            return false;
        }


        //Boxタグを持っていたら再起処理
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }

        //移動処理
        //滑らかな移動処理
        Vector3 moveToPosition = IndexToPosition(new Vector3(moveTo.x, moveTo.y, 0));
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);

        //FromにあるplayerのデータをToのデータに更新する
        //field[moveFrom.y, moveFrom.x].transform.position = IndexToPosition(new Vector3(moveTo.x, moveTo.y, 0));
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];//フィールドのデータtoにfromを挿入
        field[moveFrom.y, moveFrom.x] = null;

        //パーティクル出現処理
        for (int i = 0; i < 5; i++)
        {
            field[moveFrom.y, moveFrom.x] = Instantiate(
                        particlePrefab,
                        IndexToPosition(new Vector3(moveFrom.x, moveFrom.y, 0)),
                        Quaternion.identity
                    );
        }

        return true;
    }

    Vector3 IndexToPosition(Vector3 index)
    {
        return new Vector3(
            index.x - map.GetLength(1) / 2 + 0.5f,
            -index.y + map.GetLength(0) / 2,
            index.z
            );
    }

    bool IsCleard()
    {
        //Vector2Int型の可変長配列
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //格納場所か否かを判断
                if (map[y, x] == 3)
                {
                    //格納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //要素数はgoals.countで取得
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                //一つでも箱がなかったら条件未達成
                return false;
            }
        }
        //条件未達成でなければ条件達成
        return true;

    }

    void Start()
    {
        Screen.SetResolution(1280, 720, false);


        //配列の実態の作成と初期化
        map = new int[,]
        {
            {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
            {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
            {4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4},
            {4,4,4,4,4,0,0,0,0,0,3,0,0,0,0,4,4,4,4,4},
            {4,4,4,4,4,0,0,0,0,0,2,0,0,0,0,4,4,4,4,4},
            {4,4,4,4,4,0,0,3,2,0,1,0,0,0,0,4,4,4,4,4},
            {4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4},
            {4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4},
            {4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4},
            {4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4},
            {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
            {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4}
        };
        back = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0},
            {0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0},
            {0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0},
            {0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0},
            {0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0},
            {0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0},
            {0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0},
            {0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        field = new GameObject
        [
            map.GetLength(0),
            map.GetLength(1)
        ];
        field2 = new GameObject
        [
            back.GetLength(0),
            back.GetLength(1)
        ];

        //追加。文字列の宣言と初期化
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        IndexToPosition(new Vector3(x, y, 0)),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        IndexToPosition(new Vector3(x, y, 0)),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(
                        goalPrefab,
                        IndexToPosition(new Vector3(x, y, 0.01f)),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(
                        wallPrefab,
                        IndexToPosition(new Vector3(x, y, 0)),
                        Quaternion.identity
                    );
                }
            }
        }

        for (int y = 0; y < back.GetLength(0); y++)
        {
            for (int x = 0; x < back.GetLength(1); x++)
            {
                if (back[y, x] == 5)
                {
                    field2[y, x] = Instantiate(
                        backPrefab,
                        IndexToPosition(new Vector3(x, y, 1.0f)),
                        Quaternion.identity
                    );
                }
            }
        }
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //1.メソッド化した処理を使用
            Vector2Int playerIndex = GetPlayerIndex();

            //2.移動
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(0, -1)
                );

            //もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //1.メソッド化した処理を使用
            Vector2Int playerIndex = GetPlayerIndex();

            //2.移動
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(0, 1)
                );

            //もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //1.メソッド化した処理を使用
            Vector2Int playerIndex = GetPlayerIndex();

            //2.移動
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(1, 0)
                );
            //もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
            }

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //1.メソッド化した処理を使用
            Vector2Int playerIndex = GetPlayerIndex();

            //2.移動
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(-1, 0)
                );
            //もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
            }
        }
    }
}
