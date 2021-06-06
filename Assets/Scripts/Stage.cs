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

        const int columns = 5;
        const float lineHeight = 1.5f;
        const int ahead = 8;

        bool block;

        bool[] lastLine = new bool[columns];
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
                snakePos = new Vector3((snake.GetPos().x + 2 * lineHeight)  % lineHeight, 0, snakeIndex);
                line = new GameObject[columns];

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
            for (int i = 0; i < columns; i++)
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
            bool[] curLine = new bool[columns];

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
                for (int i = 0; i < columns; i++)
                    line[i] = InstantiateBlock(i, GetZ());
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

            bool[] available = GetAvailableBlocks();
            var followerSlots = new List<int>();
            var nonFollowerSlots = new List<int>();

            for (int i = 0; i < columns; i++)
            {
                if (available[i])
                {
                    if (lastLine[i] || (i < columns - 1 && lastLine[i + 1]))
                        followerSlots.Add(i);
                    else
                        nonFollowerSlots.Add(i);
                }
            }

            List<int> slots;
            if (followerSlots.Count > 0 && random < 79 || nonFollowerSlots.Count == 0)
                slots = followerSlots;
            else
                slots = nonFollowerSlots;
           
            random = rng.Next(0, slots.Count);
            int index = slots[random];

            curLine[index] = true;
            line[index] = InstantiateBlock(index, GetZ());
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
            bool[] curLine = new bool[columns];
            int random = rng.Next(0, 100);

            if (random < 32)
            {
                // 0 Wall
            }
            else if (random < 76)
            {
                GenerateWall(curLine);
            }
            else if (random < 95)
            {
                GenerateWall(curLine);
                GenerateWall(curLine);
            }
            else
            {
                GenerateWall(curLine);
                GenerateWall(curLine);
                GenerateWall(curLine);
            }

            obstacles.Add(line);
            GeneratePowerUps();

            nextZ += lineHeight;
            lastLine = curLine;
        }

        public void GenerateWall(bool[] curLine)
        {
            int random = rng.Next(0, 100);

            bool[] available = GetAvailableWalls();
            var followerSlots = new List<int>();
            var nonFollowerSlots = new List<int>();

            for (int i = 0; i < columns; i++)
            {
                if (available[i])
                {
                    if (lastLine[i] || (i > 0 && lastLine[i - 1]))
                        followerSlots.Add(i);
                    else
                        nonFollowerSlots.Add(i);
                }
            }

            List<int> slots;
            if (followerSlots.Count > 0 && random < 62 || nonFollowerSlots.Count == 0)
                slots = followerSlots;
            else
                slots = nonFollowerSlots;
           
            random = rng.Next(0, slots.Count);
            int index = slots[random];

            curLine[index] = true;
            line[index] = InstantiateWall(index, GetZ());
        }

        public void GenerateLongWallLine()
        {
            GenerateShortWallLine();

            line = new GameObject[columns];

            for (int i = 0; i < columns; i++)
            {
                if (obstacles[obstacles.Count - 1][i] != null && obstacles[obstacles.Count - 1][i].GetComponent<Wall>())
                {
                    line[i] = InstantiateWall(i, GetZ());
                }
            }

            obstacles.Add(line);
            GeneratePowerUps();
            nextZ += lineHeight;
        }

        #endregion

        GameObject InstantiateWall(int col, float z)
        {
            return InstantiateObstacle(wallPrefab.gameObject, col, z);
        }

        GameObject InstantiateBlock(int col, float z)
        {
            return InstantiateObstacle(blockPrefab.gameObject, col, z);
        }
        
        PowerUp InstantiatePowerUp(int col, float z)
        {
            return InstantiateObstacle(powerUpPrefab.gameObject, col, z).GetComponent<PowerUp>();
        }

        GameObject InstantiateObstacle(GameObject prefab, int col, float z)
        {
            float x = (col - (columns / 2)) * lineHeight;
            Vector3 pos = new Vector3(x, 0, z);

            return Instantiate(prefab, pos, Quaternion.identity, transform);
        }

        void GeneratePowerUps()
        {
            for (int i = 0; i < columns; i++)
            {
                if (!line[i] && rng.Next(0, 100) < 5)
                {
                    PowerUp powerUp = InstantiatePowerUp(i, GetZ());
                    powerUp.Init(rng.Next(1, 6));
                    line[i] = powerUp.gameObject;
                }
            }
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

        public bool[] GetAvailableBlocks()
        {
            var available = new bool[columns];
            for (int i = 0; i < columns; i++)
                available[i] = true;

            for (int i = 0; i < columns; i++)
            {
                if (line[i] != null)
                {
                    available[i] = false;
                    if (i > 0)
                        available[i - 1] = false;
                    if (i < 4)
                        available[i + 1] = false;
                }
            }

            return available;
        }

        public bool[] GetAvailableWalls()
        {
            var available = new bool[columns];
            available[0] = false;
            for (int i = 1; i < columns; i++)
                available[i] = true;

            for (int i = 1; i < columns; i++)
            {
                if (line[i] != null)
                {
                    available[i] = false;
                }
            }

            return available;
        }
    }
}
