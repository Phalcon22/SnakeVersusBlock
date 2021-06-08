using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class LevelGenerator : MonoBehaviour
    {
        #region Singleton

        public static LevelGenerator m;

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

        [SerializeField]
        GameObject arrivalPrefab;


        //PRIVATE
        Snake snake;
        List<GameObject[]> obstacles = new List<GameObject[]>();

        float nextZ;
        int snakeIndex;
        int snakePosX;
        int snakePosY;

        const int columns = 5;
        const float lineHeight = 1.5f;
        const int ahead = 6;
        const int infinity = 9999;
        const int cleanDist = 20;

        bool block;

        bool[] lastLine = new bool[columns];
        GameObject[] line;

        System.Random rng;

        Stats stats;

        public Level[] levels;
        public Level level { get; private set; }

        int currentLength = 0;

        bool infiniteMode;

        public void Init(Snake snake, int levelIndex)
        {
            this.snake = snake;

            nextZ = lineHeight/2f + lineHeight * 2;
            snakeIndex = 0;

            LoadRng(levelIndex);
            LoadLevel(levelIndex);

            for (int i = 0; i < 10; i++)
                obstacles.Add(new GameObject[columns]);
        }

        void LoadRng(int levelIndex)
        {
            if (levelIndex < 0)
                rng = new System.Random();
            else
                rng = new System.Random(levelIndex);
        }

        void LoadLevel(int levelIndex)
        {
            block = true;
            infiniteMode = levelIndex == -1;

            if (levelIndex < 0)
                level = levels[rng.Next(0, levels.Length)];
            else
                level = levels[levelIndex];
            stats = level.stats;
        }

        void Update()
        {
            if (!GameManager.m.started)
                return;

            if (snake.GetPos().z >= nextZ)
            {
                if (currentLength < level.length || infiniteMode)
                    GenerateLine();
                else if ((currentLength == level.length || currentLength == level.length + 1) && !infiniteMode)
                    GenerateArrival();
            }
        }

        void GenerateLine()
        {
            snakeIndex++;
            currentLength++;
            snakePosX =(int)((snake.GetPos().x + (columns / 2f) * lineHeight) / lineHeight);
            snakePosY = snakeIndex;
            line = new GameObject[columns];

            if (block)
                GenerateBlockLine();
            else
                GenerateWallLine();

            CleanOldestLine();

            block = !block;
        }

        void GenerateArrival()
        {
            currentLength = infinity;
            lastLine = new bool[columns];

            obstacles.Add(new GameObject[columns]);
            obstacles.Add(new GameObject[columns]);
            nextZ += lineHeight * 2;

            InstantiateObstacle(arrivalPrefab, 2, GetZ());
        }

        void CleanOldestLine()
        {
            while (obstacles.Count >= cleanDist)
            {
                for (int i = 0; i < columns; i++)
                {
                    if (obstacles[0][i] != null)
                        Destroy(obstacles[0][i]);
                }

                obstacles.RemoveAt(0);
                snakeIndex--;
            }
        }

        #region Block

        void GenerateBlockLine()
        {
            bool[] curLine = new bool[columns];

            int random = rng.Next(0, 100);
            if (random < stats.OneBlockPercent)
            {
                GenerateBlock(curLine);
            }
            else if (random < stats.OneBlockPercent + stats.TwoBlockPercent)
            {
                GenerateBlock(curLine);
                GenerateBlock(curLine);
            }
            else
            {
                for (int i = 0; i < columns; i++)
                    line[i] = InstantiateBlock(i, GetZ());
            }

            GeneratePowerUps();
            obstacles.Add(line);

            InitNumbers(curLine);

            nextZ += lineHeight;
            lastLine = curLine;
        }

        void GenerateBlock(bool[] curLine)
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

        void GenerateWallLine()
        {
            if (rng.Next(0, 100) >= stats.shortWallPercent)
                GenerateShortWallLine();
            else
                GenerateLongWallLine();
        }

        void GenerateShortWallLine()
        {
            bool[] curLine = new bool[columns];
            int random = rng.Next(0, 100);

            if (random < stats.ZeroWallPercent)
            {
                // 0 Wall
            }
            else if (random < stats.ZeroWallPercent + stats.OneWallPercent)
            {
                GenerateWall(curLine);
            }
            else if (random < stats.ZeroWallPercent + stats.OneWallPercent + stats.TwoWallPercent)
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

            GeneratePowerUps();
            obstacles.Add(line);

            nextZ += lineHeight;
            lastLine = curLine;
        }

        void GenerateWall(bool[] curLine)
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
            if (followerSlots.Count > 0 && random < stats.wallFollowsBlock || nonFollowerSlots.Count == 0)
                slots = followerSlots;
            else
                slots = nonFollowerSlots;
           
            random = rng.Next(0, slots.Count);
            int index = slots[random];

            curLine[index] = true;
            line[index] = InstantiateWall(index, GetZ());
        }

        void GenerateLongWallLine()
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

            GeneratePowerUps();
            obstacles.Add(line);
            nextZ += lineHeight;
            snakeIndex++;
            currentLength++;
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
                if (!line[i] && rng.Next(0, 100) < stats.powerUps)
                {
                    PowerUp powerUp = InstantiatePowerUp(i, GetZ());
                    powerUp.Init(rng.Next(stats.powerUpsMinValue, stats.powerUpsMaxValue + 1));
                    line[i] = powerUp.gameObject;
                }
            }
        }

        void InitNumbers(bool[] curLine)
        {
            bool feasible = false;
            int max = -infinity;
            int maxIndex = -1;
            for (int i = 0; i < columns; i++)
            {
                int rest = Dfs(i, obstacles.Count - 2);
                if (line[i] && line[i].GetComponent<Block>())
                {
                    Block block = line[i].GetComponent<Block>();

                    int random = rng.Next(stats.blocksMinValue, stats.blocksMaxValue + 1);
                    block.SetAmount(random);

                    if (rest > max)
                    {
                        max = rest;
                        maxIndex = i;
                    }

                    if (rest - random >= 0)
                        feasible = true;
                }
                else
                {
                    if (rest >= 0)
                        feasible = true;
                }
            }

            if (feasible)
                return;

            if (max == 0)
            {
                Destroy(line[maxIndex].gameObject);
                line[maxIndex] = null;
                curLine[maxIndex] = false;
            }
            else if (max > 0)
                line[maxIndex].GetComponent<Block>().SetAmount(rng.Next(1, max + 1));
        }

        enum EDirection
        {
            LEFT,
            RIGHT,
            UP
        }

        int Dfs(int x, int y, int dist = 0, EDirection from = EDirection.UP, int lateralMoveInRow = 0)        
        {
            int res = -infinity;

            if (dist > ahead * 2)
            {
                return -9997;
            }

            if (snakePosX == x && snakePosY == y)
                return snake.GetFollower();

            if (res < 0 && y - 1 >= snakePosY)
            {
                res = Mathf.Max(res, Dfs(x, y - 1, dist + 1, EDirection.UP, 0));
            }

            if (lateralMoveInRow < 2 && res < 0 && x + 1 < columns && from != EDirection.RIGHT)
            {
                GameObject obstacle = obstacles[y][x + 1];
                if (!obstacle || !obstacle.GetComponent<Wall>())
                {
                    res = Mathf.Max(res, Dfs(x + 1, y, dist + 1, EDirection.LEFT, lateralMoveInRow));
                }
            }

            if (lateralMoveInRow < 2 && res < 0 && x - 1 >= 0 && from != EDirection.LEFT)
            {
                GameObject obstacle = obstacles[y][x];
                if (!obstacle || !obstacle.GetComponent<Wall>())
                {
                    res = Mathf.Max(res, Dfs(x - 1, y, dist + 1, EDirection.RIGHT, lateralMoveInRow));
                }
            }

            if (obstacles[y][x])
            {
                if (obstacles[y][x].GetComponent<Block>())
                    res -= obstacles[y][x].GetComponent<Block>().amount;
                else if (obstacles[y][x].GetComponent<PowerUp>())
                    res += obstacles[y][x].GetComponent<PowerUp>().amount;
            }

            return res;
        }

        float GetZ()
        {
            return nextZ + lineHeight * ahead + lineHeight / 2;
        }

        bool[] GetAvailableBlocks()
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

        bool[] GetAvailableWalls()
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
