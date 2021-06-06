using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class Stage : MonoBehaviour
    {
        #region Singleton

        public static Stage m;

        void Awake()
        {
            m = this;
        }

        #endregion

        //INSPECTOR
        [SerializeField]
        Wall wallPrefab;

        [SerializeField]
        Block blockPrefab;

        [SerializeField]
        PowerUp powerUpPrefab;


        //PRIVATE
        Snake snake;
        List<GameObject[]> obstacles = new List<GameObject[]>();

        float nextZ;
        int snakeIndex;
        Vector3 snakePos;

        const float lineHeight = 1.5f;
        const int ahead = 8;

        bool block;

        bool[] lastLine = new bool[5];
        GameObject[] line;

        System.Random rng;

        public void Init(Snake snake, int level)
        {
            nextZ = lineHeight/2;
            snakeIndex = 0;
            block = true;
            rng = new System.Random(level);
            this.snake = snake;
        }

        void Update()
        {
            if (nextZ == lineHeight/2)
            {
                //FirstInit
            }

            if (snake.GetPos().z >= nextZ)
            {
                snakeIndex++;
                snakePos = new Vector3((snake.GetPos().x + 3.0f)  % lineHeight, 0, snakeIndex);
                line = new GameObject[5];

                if (block)
                    GenerateBlockLine();
                else
                    GenerateWallLine();

                if (obstacles.Count >= 20)
                    CleanOldestLine();

                block = !block;
            }
        }

        public void CleanOldestLine()
        {
            for (int i = 0; i < 5; i++)
            {
                if (obstacles[0][i] != null)
                    Destroy(obstacles[0][i]);
            }

            obstacles.RemoveAt(0);
            snakeIndex--;
        }

        #region Block

        public void GenerateBlockLine()
        {
            bool[] curLine = new bool[5];

            int random = rng.Next(0, 100);
            if (random < 40)
            {
                GenerateBlock(curLine);
            }
            else if (random < 78)
            {
                GenerateBlock(curLine);
                GenerateBlock(curLine);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                    InstantiateBlock(i, GetZ());
            }

            obstacles.Add(line);

            GeneratePowerUps();

            //Dfs
            //if not enough paths then lower or destroy one cube

            nextZ += lineHeight;
            lastLine = curLine;
        }

        public void GenerateBlock(bool[] curLine)
        {
            int random = rng.Next(0, 100);

            bool[] notAvailable = new bool[5];
            for (int i = 0; i < 5; i++)
            {
                if (line[i] != null)
                {
                    notAvailable[i] = true;
                    if (i > 0)
                        notAvailable[i - 1] = true;
                    if (i < 4)
                        notAvailable[i + 1] = true;
                }
            }

            int followersSlot = 0;
            int notFollowersSlot = 0;
            for (int i = 0; i < 5; i++)
            {
                if (!notAvailable[i])
                {
                    if (lastLine[i] || (i < 4 && lastLine[i + 1]))
                        followersSlot++;
                    else
                        notFollowersSlot++;
                }
            }

            if (followersSlot > 0 && random < 79)
                GenerateFollowerBlock(curLine, notAvailable, followersSlot);
            else
                GenerateNonFollowerBlock(curLine, notAvailable, notFollowersSlot);
        }

        public void GenerateFollowerBlock(bool[] curLine, bool[] notAvailable, int followersSlot)
        {
            int random = rng.Next(0, 100);
            float ceil = 100f / followersSlot;
            for (int i = 0; i < 5; i++)
            {
                if (!notAvailable[i] && (lastLine[i] || (i < 4 && lastLine[i + 1])))
                {
                    if (random < ceil)
                    {
                        curLine[i] = true;

                        notAvailable[i] = true;
                        if (i > 0)
                            notAvailable[i - 1] = true;
                        if (i < 4)
                            notAvailable[i + 1] = true;

                        InstantiateBlock(i, GetZ());

                        return;
                    }
                    else
                        ceil += 100f / followersSlot;
                }
            }
        }

        public void GenerateNonFollowerBlock(bool[] curLine, bool[] notAvailable, int notFollowersSlot)
        {
            int random = rng.Next(0, 100);
            float ceil = 100f / notFollowersSlot;
            for (int i = 0; i < 5; i++)
            {
                if (!notAvailable[i] && (!lastLine[i] && (i >= 4 || !lastLine[i + 1])))
                {
                    if (random < ceil)
                    {
                        curLine[i] = true;

                        notAvailable[i] = true;
                        if (i > 0)
                            notAvailable[i - 1] = true;
                        if (i < 4)
                            notAvailable[i + 1] = true;

                        InstantiateBlock(i, GetZ());

                        return;
                    }
                    else
                        ceil += 100f / notFollowersSlot;
                }
            }
        }

        #endregion

        #region Wall

        public void GenerateWallLine()
        {
            if (rng.Next(0, 1) >= 0.5f)
                GenerateShortWallLine();
            else
                GenerateLongWallLine();
        }

        public void GenerateShortWallLine()
        {
            bool[] curLine = new bool[5];
            int random = rng.Next(0, 100);

            if (random < 32)
            {
                // 0 Wall
            }
            else if (random < 76)
            {
                // 1 Wall -> Where to place it ? More chance in front of block
                GenerateWall(curLine);
            }
            else if (random < 95)
            {
                GenerateWall(curLine);
                GenerateWall(curLine);
                // 2 Wall -> Where to place it ? More chance in front of block
            }
            else
            {
                GenerateWall(curLine);
                GenerateWall(curLine);
                GenerateWall(curLine);
                // 3 Wall -> Where to place it ? More chance in front of block
            }

            obstacles.Add(line);
            GeneratePowerUps();

            nextZ += lineHeight;
            lastLine = curLine;
        }

        public void GenerateWall(bool[] curLine)
        {
            int random = rng.Next(0, 100);

            bool[] notAvailable = new bool[5];
            notAvailable[0] = true;

            int followersSlot = 0;
            int notFollowersSlot = 0;
            for (int i = 0; i < 5; i++)
            {
                if (!notAvailable[i])
                {
                    if (lastLine[i] || (i > 0 && lastLine[i - 1]))
                        followersSlot++;
                    else
                        notFollowersSlot++;
                }
            }

            if (followersSlot > 0 && random < 62)
                GenerateFollowerWall(curLine, notAvailable, followersSlot);
            else
                GenerateNonFollowerWall(curLine, notAvailable, notFollowersSlot);
        }

        public void GenerateFollowerWall(bool[] curLine, bool[] notAvailable, int followersSlot)
        {
            int random = rng.Next(0, 100);
            float ceil = 100f / followersSlot;
            for (int i = 0; i < 5; i++)
            {
                if (!notAvailable[i] && (lastLine[i] || (i > 0 && lastLine[i - 1])))
                {
                    if (random < ceil)
                    {
                        curLine[i] = true;

                        notAvailable[i] = true;

                        InstantiateWall(i, GetZ());

                        return;
                    }
                    else
                        ceil += 100f / followersSlot;
                }
            }
        }

        public void GenerateNonFollowerWall(bool[] curLine, bool[] notAvailable, int notFollowersSlot)
        {
            int random = rng.Next(0, 100);
            float ceil = 100f / notFollowersSlot;
            for (int i = 0; i < 5; i++)
            {
                if (!notAvailable[i] && (!lastLine[i] && (i == 0 || !lastLine[i - 1])))
                {
                    if (random < ceil)
                    {
                        curLine[i] = true;

                        notAvailable[i] = true;

                        InstantiateWall(i, GetZ());

                        return;
                    }
                    else
                        ceil += 100f / notFollowersSlot;
                }
            }
        }

        public void GenerateLongWallLine()
        {
            GenerateShortWallLine();

            line = new GameObject[5];

            for (int i = 0; i < 5; i++)
            {
                if (obstacles[obstacles.Count - 1][i] != null)
                {
                    InstantiateWall(i, GetZ());
                }
            }

            obstacles.Add(line);
            GeneratePowerUps();
            nextZ += lineHeight;
        }

        #endregion

        Wall InstantiateWall(int col, float z)
        {
            return InstantiateObstacle(wallPrefab.gameObject, col, z).GetComponent<Wall>();
        }

        Block InstantiateBlock(int col, float z)
        {
            return InstantiateObstacle(blockPrefab.gameObject, col, z).GetComponent<Block>();
        }
        
        PowerUp InstantiatePowerUp(int col, float z)
        {
            return InstantiateObstacle(powerUpPrefab.gameObject, col, z).GetComponent<PowerUp>();
        }

        GameObject InstantiateObstacle(GameObject prefab, int col, float z)
        {
            float x = (col - 2) * lineHeight;
            Vector3 pos = new Vector3(x, 0, z);

            line[col] = Instantiate(prefab, pos, Quaternion.identity, transform);
            return line[col];
        }

        void GeneratePowerUps()
        {

        }

        public int Dfs(int x, int y)
        {
            if (snakePos.x == x && snakePos.z == y)
            {
                return snake.GetFollower();
            }

            int res = -99999;

            if (y < snakePos.y)
                return res;

            if ((!obstacles[y][x] || !obstacles[y][x].GetComponent<Wall>()) && (!obstacles[y][x - 1] || !obstacles[y][x - 1].GetComponent<Block>()))
            {
                res = Mathf.Max(res, Dfs(x - 1, y));
                if (res >= 0)
                    return res;
            }

            if (!obstacles[y - 1][x] || !obstacles[y - 1][x].GetComponent<Block>())
            {
                res = Mathf.Max(res, Dfs(x, y - 1));
                if (res >= 0)
                    return res;
            }

            if (!obstacles[y][x + 1] || (!obstacles[y][x + 1].GetComponent<Wall>() && !obstacles[y][x + 1].GetComponent<Block>()))
                return Mathf.Max(res, Dfs(x + 1, y));

            return res;
        }

        public float GetZ()
        {
            return nextZ + lineHeight * ahead + lineHeight / 2;
        }
    }
}
