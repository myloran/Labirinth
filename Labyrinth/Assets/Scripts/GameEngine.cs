using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour 
{
    public Transform player;
    public Transform finish;
    public Transform left_wall;
    public Transform up_wall;

    public int width = 5;
    public int height = 5;

    Loc[,] Maze;
    int[,] Mark;
    int x, y, xc, yc;
    int[] dx = { 1, 0, -1, 0 };
    int[] dy = { 0, -1, 0, 1 };
    float cellSize = 0.8f;

    struct Loc
    {
        public bool left_wall, up_wall;
    }

    struct Wall
    {
        public int x, y, dx, dy;
    }

	void Start () 
    {
        Application.targetFrameRate = 60;
        BuildLabytinth();
        Object p = Instantiate(player, new Vector3(0.0f, 1.5f, height * 0.8f - 0.4f), Quaternion.identity);
        p.name = "Player";
        Instantiate(finish, new Vector3((height - 1) * 0.8f, 0, 0.4f), Quaternion.identity);
	}

    public void Restart()
    {
        Application.LoadLevel(1);
        Time.timeScale = 1;
    }

    void BuildLabytinth()
    {
        int WallCount = (width - 1) * height + (height - 1) * width; //Количество стен
        Wall[] Walls = new Wall[WallCount]; //Стены
        int[] Temp = new int[WallCount]; //Массив для сортировки стен
        Maze = new Loc[width + 1, height + 1]; //Лабиринт 

        //Заполняем лабиринт стенами
        for (int i = 0; i < width + 1; i++)
            for (int j = 0; j < height + 1; j++)
            {
                Maze[i, j].left_wall = true;
                Maze[i, j].up_wall = true;
            }

        //Заполняем массив Temp случайными числами
        for (int i = 0; i < WallCount - 1; i++)
            Temp[i] = Random.Range(0, WallCount);

        //Заполняем массив стен
        int counter = 0;
        for (int i = 1; i < width; i++) //сначала горизонтальными
            for (int j = 0; j < height; j++)
            {
                Walls[counter].x = i;
                Walls[counter].y = j;
                Walls[counter].dx = -1;
                Walls[counter].dy = 0;
                counter++;
            }
        for (int i = 0; i < width; i++) //затем вертикальными
            for (int j = 1; j < height; j++)
            {
                Walls[counter].x = i;
                Walls[counter].y = j;
                Walls[counter].dx = 0;
                Walls[counter].dy = -1;
                counter++;
            }

        //Перемешиваем массив стен
        for (int i = 0; i < WallCount - 1; i++)
            for (int j = 1; j < WallCount - 1; j++)
                if (Temp[i] < Temp[j])
                {
                    int temp_i = Temp[i];
                    Temp[i] = Temp[j];
                    Temp[j] = temp_i;
                    Wall temp_w = Walls[i];
                    Walls[i] = Walls[j];
                    Walls[j] = temp_w;
                }

        int countOfLocations = width * height;
        int n = 0;
        while (countOfLocations > 1)
        {
            Wall currWall = Walls[n];
            n++;
            //Если прохода нет - разрушаем стену
            if (!IsConnected(currWall.x, currWall.y, currWall.x + currWall.dx, currWall.y + currWall.dy))
            {
                BreakWall(currWall.x, currWall.y, currWall.dx, currWall.dy);
                countOfLocations--;
                //ShowMaze(Maze);
            }
        }
        ShowMaze(Maze);
    }

    bool IsConnected(int x, int y, int dx, int dy)
    {
        width = Maze.GetLength(0);
        height = Maze.GetLength(1);
        Mark = new int[width, height];

        for (int i = 0; i < width - 1; i++)
            for (int j = 0; j < height - 1; j++)
                Mark[i, j] = 0;

        Mark[x, y] = 1;
        if (Solve(dx, dy))
            return true;
        return false;
    }

    bool Solve(int xd, int yd)
    {
        int n = 1;
        bool noSolution;
        do
        {
            noSolution = true;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (Mark[x, y] == n)
                        for (int i = 0; i < 4; i++)
                        {
                            //MessageBox.Show((CanGo(x, y, dx[i], dy[i]) && Mark[x + dx[i], y + dy[i]] == 0).ToString());
                            if (CanGo(x, y, dx[i], dy[i]) && Mark[x + dx[i], y + dy[i]] == 0)
                            {
                                noSolution = false;
                                Mark[x + dx[i], y + dy[i]] = n + 1;
                                if (x + dx[i] == xd && y + dy[i] == yd)
                                {
                                    //MessageBox.Show("123");
                                    return true;
                                }
                            }
                        }
            n++;
            //MessageBox.Show(n.ToString());
        }
        while (!noSolution);
        return false;
    }

    bool CanGo(int x, int y, int dx, int dy)
    {
        bool canGo = false;
        if (dx == -1) canGo = !Maze[x, y].left_wall;
        else if (dx == 1) canGo = !Maze[x + 1, y].left_wall;
        else if (dy == -1) canGo = !Maze[x, y].up_wall;
        else canGo = !Maze[x, y + 1].up_wall;
        //MessageBox.Show(canGo.ToString());
        return canGo;
    }

    void BreakWall(int x, int y, int dx, int dy)
    {
        if (dx == -1) Maze[x, y].left_wall = false;
        else if (dx == 1) Maze[x + 1, y].left_wall = false;
        else if (dy == -1) Maze[x, y].up_wall = false;
        else Maze[x, y + 1].up_wall = false;
    }

    void ShowMaze(Loc[,] Maze)
    {
        width = Maze.GetLength(0) - 1;
        height = Maze.GetLength(1) - 1;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                //Если есть вертикальная стена - рисуем ее
                if (Maze[x, y].up_wall)
                    Instantiate(up_wall, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
                //Если есть горизонтальная стена - рисуем ее
                if (Maze[x, y].left_wall)
                    Instantiate(left_wall, new Vector3(x * cellSize - 0.4f, 0, y * cellSize + 0.4f), Quaternion.identity);
            }
        //Рисуем стену снизу и справа от лабиринта
        for (int i = 0; i < width; i++)
            Instantiate(up_wall, new Vector3(i * cellSize, 0, width * cellSize), Quaternion.identity);
        for (int i = 0; i < height; i++)
            Instantiate(left_wall, new Vector3(height * cellSize - 0.4f, 0, i * cellSize + 0.4f), Quaternion.identity);

        //Рисуем пол
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale += new Vector3(100, 0, 100);
        plane.GetComponent<Renderer>().material = Resources.Load("planeMaterial", typeof(Material)) as Material;
        //plane.transform.localScale += new Vector3(0.1f * (width - 11), 0, 0.1f * (height - 11));
        //Debug.Log(plane.transform.position);

        plane.transform.position = new Vector3(width * 0.3f/*1.5f*/, -0.5f, height * 0.3f + 0.4f/*1.9f*/);
        
    }

}
