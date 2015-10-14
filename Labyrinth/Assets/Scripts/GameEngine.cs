using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour 
{
    public GameObject player;
    public GameObject finish;
    public Transform left_wall;
    public Transform up_wall;
    public GameObject Canvas;
    public Text keysText;
    public Text timeText;

    public int width = 5;
    public int height = 5;

    public GameObject starV0;
    public GameObject starV1;
    public GameObject starV2;
    public GameObject starV3;

    bool isPaused = false;
    GameObject p, f;
    LevelData level;
    int countOfKeys;
    int keysToFinish;

    Loc[,] Maze;
    int[,] Mark;
    int x, y, xc, yc;
    int[] dx = { 1, 0, -1, 0 };
    int[] dy = { 0, -1, 0, 1 };
    float cellSize = 0.8f;

	void Start () 
    {
        Application.targetFrameRate = 60;
        if (Data.IsContinue)
        {
            Data.Load();
            Maze = Data.lastGame.Maze;
            ShowMaze(Maze);
            p = (GameObject)Instantiate(player, Data.lastGame.playerPosition, Quaternion.identity);
            p.name = "Player";
            f = (GameObject)Instantiate(finish, Data.lastGame.keysPosition[Data.lastGame.keysPosition.Length - 1], Quaternion.identity);

            Data.IsContinue = false;
        }
        else
        {
            if (Data.campaignLevel != 0)
            {
                Data.LoadLevels();
                level = Data.levels[Data.campaignLevel - 1];
                countOfKeys = level.countOfKeys;
                keysToFinish = level.countOfKeysDuringGame;
                Debug.Log(countOfKeys);
                keysText.text = (countOfKeys - 1).ToString();
                timeText.text = level.totalTime.ToString();
                BuildLabytinth();
                p = (GameObject)Instantiate(player, new Vector3(0.0f, 1.5f, height * 0.8f - 0.4f), Quaternion.identity);
                p.name = "Player";
                if (countOfKeys > 1)
                {
                    if (countOfKeys > level.countOfKeysDuringGame)
                    {
                        CreateKeys(level.countOfKeysDuringGame);
                        //countOfKeys -= level.countOfKeysDuringGame;
                    }
                    else if (countOfKeys <= level.countOfKeysDuringGame)
                    {
                        CreateKeys(countOfKeys - 1);
                        //countOfKeys -= countOfKeys - 1;
                    }
                }
                else if (countOfKeys == 1)
                {
                    f = (GameObject)Instantiate(finish, new Vector3(16 * 0.8f, 0, 0.4f), Quaternion.identity);
                    countOfKeys--;
                }

                InvokeRepeating("UpdateText", 2.2f, 1);
            }
            else
            {
                BuildLabytinth();
                p = (GameObject)Instantiate(player, new Vector3(0.0f, 1.5f, height * 0.8f - 0.4f), Quaternion.identity);
                p.name = "Player";
                f = (GameObject)Instantiate(finish, new Vector3((height - 1) * 0.8f * 1.8f - 0.15f, 0, 0.4f), Quaternion.identity);
            }
        }
	}

    void CreateKeys(int number)
    {
        countOfKeys -= number;
        for (int i = 0; i < number; i++)
        {
            Instantiate(finish, new Vector3(Random.Range(0, 16) * 0.8f, 0, Random.Range(0, 9) * 0.8f + 0.4f), Quaternion.identity);
        }
    }
    public void KeyFounded() 
    {
        Debug.Log(countOfKeys);
        Debug.Log(keysToFinish);
        if (countOfKeys > 1)
        {
            CreateKeys(1);
            Canvas.transform.Find("timeText").gameObject.GetComponent<Text>().text = (int.Parse(Canvas.transform.Find("timeText").gameObject.GetComponent<Text>().text) + level.keyTime).ToString();
        }
        else if (countOfKeys == 1 && keysToFinish-- == 1)
        {
            f = (GameObject)Instantiate(finish, new Vector3(16 * 0.8f, 0, 0.4f), Quaternion.identity);
            countOfKeys--;
            Canvas.transform.Find("timeText").gameObject.GetComponent<Text>().text = (int.Parse(Canvas.transform.Find("timeText").gameObject.GetComponent<Text>().text) + level.keyTime).ToString();
        }
        else if (countOfKeys == 0)
        {
            //Stop the game and show victory screen:
            Time.timeScale = 0;
            GameObject victoryScreen = Canvas.transform.Find("VictoryScreen").gameObject;
            victoryScreen.SetActive(true);
            victoryScreen.transform.Find("Text").gameObject.GetComponent<Text>().text = "Уровень пройден!";
                
            //Record player rating and show star:
            Data.levels[Data.campaignLevel - 1].timeLeft = level.totalTime;
            float time = int.Parse(Canvas.transform.Find("timeText").gameObject.GetComponent<Text>().text);
            if (time < level.timeFor2star)
            {
                Data.levels[Data.campaignLevel - 1].star = 1;
                GameObject star = (GameObject)Instantiate(starV1, new Vector2(0, 50), Quaternion.identity);
                star.transform.SetParent(Canvas.transform.Find("VictoryScreen"), false);
            }
            else if (time < level.timeFor3star)
            {
                Data.levels[Data.campaignLevel - 1].star = 2;
                GameObject star = (GameObject)Instantiate(starV2, new Vector2(0, 50), Quaternion.identity);
                star.transform.SetParent(Canvas.transform.Find("VictoryScreen"), false);
            }
            else 
            {
                Data.levels[Data.campaignLevel - 1].star = 3;
                GameObject star = (GameObject)Instantiate(starV3, new Vector2(0, 50), Quaternion.identity);
                star.transform.SetParent(Canvas.transform.Find("VictoryScreen"), false);
            }
        }
        
    }
    void UpdateText()
    {
        keysText.text = countOfKeys.ToString();
        timeText.text = level.totalTime--.ToString();

    }
    public void SetPause(bool pause)
    {
        isPaused = pause;
    }

    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
            {
                if (!isPaused)
                {
                    Canvas.transform.Find("PausePanel").gameObject.SetActive(true);
                    (Canvas.GetComponent("Image") as Image).enabled = true;
                    Canvas.transform.Find("MobileJoystick").gameObject.GetComponent<UnityStandardAssets.CrossPlatformInput.Joystick>().enabled = false;

                    Time.timeScale = 0;

                    isPaused = true;
                }
                else
                {
                    Canvas.transform.Find("PausePanel").gameObject.SetActive(false);
                    (Canvas.GetComponent("Image") as Image).enabled = false;
                    Canvas.transform.Find("MobileJoystick").gameObject.GetComponent<UnityStandardAssets.CrossPlatformInput.Joystick>().enabled = true;

                    Time.timeScale = 1;

                    isPaused = false;
                }
            }
    }

    public void Restart()
    {
        //UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.get

        Application.LoadLevel("Game");
        
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
        GameObject empty = new GameObject("Labyrinth");
        empty.isStatic = true;
        empty.transform.position = new Vector3(0, 0, 0);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                //Если есть вертикальная стена - рисуем ее
                if (Maze[x, y].up_wall)
                    (Instantiate(up_wall, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity) as Transform).SetParent(empty.transform);
                //Если есть горизонтальная стена - рисуем ее
                if (Maze[x, y].left_wall)
                    (Instantiate(left_wall, new Vector3(x * cellSize - 0.4f, 0, y * cellSize + 0.4f), Quaternion.identity) as Transform).SetParent(empty.transform);
            }
        //Рисуем стену снизу и справа от лабиринта
        for (int i = 0; i < width; i++)
            (Instantiate(up_wall, new Vector3(i * cellSize, 0, width * cellSize / 1.7f), Quaternion.identity) as Transform).SetParent(empty.transform);
        for (int i = 0; i < height; i++)
            (Instantiate(left_wall, new Vector3(height * cellSize * 1.7f - 0.4f, 0, i * cellSize + 0.4f), Quaternion.identity) as Transform).SetParent(empty.transform);

        //Рисуем пол
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale += new Vector3(1, 0, 1);
        plane.GetComponent<Renderer>().material = Resources.Load("planeMaterial", typeof(Material)) as Material;
        plane.transform.SetParent(empty.transform);
        //plane.transform.localScale += new Vector3(0.1f * (width - 11), 0, 0.1f * (height - 11));
        //Debug.Log(plane.transform.position);

        plane.transform.position = new Vector3(width * 0.4f, -0.5f, height * 0.4f);
        //empty.transform.SetParent(empty.transform);

    }

    public void SaveLastGame()
    {
        LastGame game = new LastGame();
        game.Maze = Maze;
        game.playerPosition = p.transform.position;
        game.keyLeft = 0;
        game.keyTime = new float[1];
        game.keyTime[0] = 1;
        game.keysPosition = new Vector3[1];
        game.keysPosition[0] = new Vector3(f.transform.position.x, 0, f.transform.position.z);
        game.timeLeft = 30;

        Data.lastGame = game;
    }

}

public struct Loc
{
    public bool left_wall, up_wall;
}

struct Wall
{
    public int x, y, dx, dy;
}

