using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


public class Generator : MonoBehaviour
{


    public int width = 256;
    public int length = 256;
    public float height = 20;
    public float scale = 1f;
    public GameObject wallPrefab;
    public int wallScale = 5;

    private List<List<Cell>> cells = new List<List<Cell>>();
    List<List<Cell>> unaltered_cells;

    // Start is called before the first frame update
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = MakeTerrain(terrain.terrainData);
        cells = MakeCells(cells);
        UpdateCellList(cells);
        MakeAdjacents();
        makeMaze();
        foreach (List<Cell> row in cells)
        {
            foreach (Cell cell in row)
            {
                cell.Show(wallPrefab);
            }
        }

    }

    TerrainData MakeTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, height, length);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float r = Random.Range(0, 100);
        float[,] heights = new float[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = Mathf.PerlinNoise((float)(x+r)/height, (float)(y+r)/height) *scale;
            }
        }

        return heights;
    }

    public bool SolverOn = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            unaltered_cells = ObjectCopier.Clone(cells);
            if (!SolverOn)
            {
                SolveMaze();
                line.enabled = true;
                SolverOn = true;
                
            } else
            {
                line.enabled = false;
                SolverOn = false;
            }
            cells = unaltered_cells;
        }
    }
    [System.Serializable]

    public class Cell
    {
        public float wallScale;
        //coordinates of middle point
        public float xCoord;
        public float yCoord;
        public bool inMaze;
        public List<List<Cell>> cells;

        public Wall[] walls = new Wall[4]; //top, bottom, left, right

        public Cell(float x, float y, float scale, List<List<Cell>> c)
        {
            wallScale = scale;
            inMaze = false;
            cells = c;
            xCoord = x;
            yCoord = y;
            for (int i = 0; i < 4; i++)
            {
                walls[i] = new Wall(this, i);
            }
        }

        public void Show(GameObject wallPrefab)
        {

            if (!walls[1].isPassage)
            {
                Instantiate(wallPrefab, new Vector3(xCoord, 0, yCoord), Quaternion.Euler(0f, 0f, 0f));
            }

            if (!walls[0].isPassage)
            {
                Instantiate(wallPrefab, new Vector3(xCoord, 0, yCoord + wallScale), Quaternion.Euler(0f, 0f, 0f));
            }

            if (!walls[3].isPassage)
            {
                Instantiate(wallPrefab, new Vector3(xCoord + wallScale/2, 0, yCoord + wallScale/2), Quaternion.Euler(0f, 90f, 0f));
            }

            if (!walls[2].isPassage)
            {
                Instantiate(wallPrefab, new Vector3(xCoord - wallScale / 2, 0, yCoord + wallScale / 2), Quaternion.Euler(0f, 90f, 0f));
            }
        }

        public Cell getAdjacent(int ap, Cell current)
        {
            
            int row = (int)((current.xCoord-15) / current.wallScale);
            int col = (int)((current.yCoord-15) / current.wallScale);
            if (ap == 0)
            {
                col -= 1;
            }
            if (ap == 1)
            {
                col += 1;
            }
            if (ap == 2)
            {
                row += 1;
            }
            if (ap == 3)
            {
                row -= 1;
            }

            if (row > -1 && row < cells.Count && col > -1 && col < cells[0].Count)
            {
                return cells[row][col];
            }
            return null;
            

        }

    }

    [System.Serializable]
    public class Wall
    {
        public Cell current;
        public Cell adjacent;
        public bool isPassage;
        public int adjacentPos;
        public bool isEnd;
        public Wall(Cell parent, int position)
        {
            current = parent;
            isPassage = false;
            isEnd = false;
            adjacentPos = getAP(position);
            //adjacent = current.getAdjacent(adjacentPos, current);

        }

        int getAP(int p)
        {
            if (p == 0)
            {
                return 1;
            }
            if (p == 1)
            {
                return 0;
            }
            if (p == 2)
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
    }
    
    List<List<Cell>> MakeCells(List<List<Cell>> cells)
    {
        List<List<Cell>> list = new List<List<Cell>>();
        for (int x = 15; x < width - 15; x += wallScale)
        {
            List<Cell> row = new List<Cell>();
            for (int y = 15; y < length - 15; y += wallScale)
            {
                row.Add(new Cell(x, y, wallScale, cells));
            }
            list.Add(row);
        }
        return list;
    }

    void MakeAdjacents()
    {
        foreach (List<Cell> row in cells)
        {
            foreach (Cell cell in row)
            {
                foreach (Wall wall in cell.walls)
                {
                    wall.adjacent = cell.getAdjacent(wall.adjacentPos, cell);
                }
            }
        }
    }

    void UpdateCellList(List<List<Cell>> cells)
    {
        foreach (List<Cell> row in cells)
        {
            foreach (Cell cell in row)
            {
                cell.cells = cells;
            }
        }
    }

    void makeMaze()
    {
        List<Wall> walls = new List<Wall>();
        int first_row = (int)Random.Range(0, cells.Count);
        int first_col = (int)Random.Range(0, cells[first_row].Count);
        Cell first_cell = cells[first_row][first_col];
        first_cell.inMaze = true;

        foreach (Wall wall in first_cell.walls)
        {
            walls.Add(wall);
        }

        while (walls.Count > 0)
        {
            int random_index = (int)Random.Range(0, walls.Count);
            Wall random_wall = walls[random_index];
            walls.RemoveAt(random_index);
            
            if (random_wall.adjacent != null && !(random_wall.adjacent.inMaze))
            {
                random_wall.isPassage = true;
                random_wall.adjacent.walls[random_wall.adjacentPos].isPassage = true;
                random_wall.adjacent.inMaze = true;

                foreach (Wall wall in random_wall.adjacent.walls)
                {
                    if (!wall.isPassage)
                    {
                        walls.Add(wall);
                    }
                }
            }

        }
        makeOpenings();
    }

    public GameObject player;
    public Vector3 solveOffset;
    public LineRenderer line;
    public float segmentSize;
    
    void SolveMaze()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Cell> visitedCells = new List<Cell>();
        Cell current = GetCell();
        if (current != null)
        {
            visitedCells.Add(current);
            vertices.Add(new Vector3(current.xCoord, 0, current.yCoord) + solveOffset);
            vertices = RecursiveSolve(current, vertices, visitedCells, current);

            vertices = Randomize(vertices);
            vertices = Smooth(vertices, 20);
           
            line.positionCount = vertices.Count;
            line.SetPositions(vertices.ToArray());
        }     
    }

    List<Vector3> RecursiveSolve(Cell current, List<Vector3> vertices, List<Cell> visitedCells, Cell globalCurrent)
    {
        visitedCells.Add(current);
        Cell next = null;
        foreach(Wall wall in current.walls)
        {
            if (wall.isEnd)
            {
                vertices.Add(new Vector3(current.xCoord, 0, current.yCoord) + solveOffset);
                return vertices;
            }
        }

        List<Wall> openWalls = new List<Wall>();
        foreach (Wall wall in current.walls)
        {
            if (wall.isPassage)
            {
                openWalls.Add(wall);
            }
        }
        if (openWalls.Count == 1)
        {
            next = openWalls[0].adjacent;
            openWalls[0].isPassage = false;
            if (vertices.Count >= 1)
            {
                vertices.RemoveAt(vertices.Count - 1);
            } else
            {
                next = globalCurrent;
                visitedCells = new List<Cell>();
            }
            

        } else
        {
            int n = 0;
            while (next == null)
            {
                n += 1;
                if (n > 1000)
                {
                    Debug.Log("loop");
                    vertices.Add(new Vector3(0, 0, 0));
                    break;
                }
                int index = (int)Random.Range(0, openWalls.Count);
                if (!visitedCells.Contains(openWalls[index].adjacent) && (openWalls[index].adjacent != null))
                {
                    next = openWalls[index].adjacent;
                    vertices.Add(new Vector3(current.xCoord, 0, current.yCoord) + solveOffset);
                    //globalCurrent = current;
                    openWalls[index].isPassage = false;
                }
            }
        }
        return RecursiveSolve(next, vertices, visitedCells, globalCurrent);

     }

    List<Vector3> Smooth (List<Vector3> vertices, int iterations)
    {
        if (vertices.Count < 3)
        {
            return vertices;
        }
        for (int n  = 0; n < iterations; n++)
        {
            for (int i = vertices.Count-1; i >= 2; i -= 4)
            {
                Vector3 firstPoint = vertices[i];
                Vector3 middlePoint = vertices[i - 1];
                Vector3 lastPoint = vertices[i - 2];
                Vector3 firstAdd = Vector3.Lerp(firstPoint, middlePoint, .75f);
                Vector3 secondAdd = Vector3.Lerp(middlePoint, lastPoint, .25f);
                vertices.Insert(i - 1, firstAdd);
                vertices.Insert(i - 1, secondAdd);
                vertices.Remove(middlePoint);
            }
           

        }
        return vertices;
    }

    public float randomMagnitutde = 1f;
    public float lineHeight = 1.5f;
    List<Vector3> Randomize(List<Vector3> vertices)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            
            Vector3 offset = new Vector3(Random.value, Random.value, Random.value) * randomMagnitutde;
            vertices[i] += offset;
            vertices[i] += Vector3.down * lineHeight;
        }
        return vertices;
    }

    Cell GetCell()
    {
        int row = (int)((player.transform.localPosition.x - 15) / wallScale);
        int col = (int)((player.transform.localPosition.z - 15) / wallScale);
        if (row > -1 && row < cells.Count && col > -1 && col < cells[0].Count)
        {
            return cells[row][col];
        }
        return null;
    }
    void makeOpenings()
    {
        List<Wall> walls = new List<Wall>();

        foreach (Cell cell in cells[0])
        {
            walls.Add(cell.walls[2]);
        }
        foreach (Cell cell in cells[cells.Count - 1])
        {
            walls.Add(cell.walls[3]);
        }
        foreach (List<Cell> col in cells)
        {
            walls.Add(col[0].walls[1]);
            walls.Add(col[col.Count-1].walls[0]);
        }

        int index = Random.Range(0, walls.Count);
        Wall start = walls[index];
        walls.RemoveAt(index);
        start.isPassage = true;
        player.transform.position = new Vector3(start.current.xCoord, .5f, start.current.yCoord);
        player.transform.LookAt(new Vector3(width / 2, 0, length / 2));
        player.transform.Translate(Vector3.back * 15);

        Wall end = walls[Random.Range(0, walls.Count)];
        end.isPassage = true;
        end.isEnd = true;
    }

}



/// <summary>
/// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
/// Provides a method for performing a deep copy of an object.
/// Binary Serialization is used to perform the copy.
/// </summary>
public static class ObjectCopier
{
    /// <summary>
    /// Perform a deep copy of the object via serialization.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>A deep copy of the object.</returns>
    public static T Clone<T>(T source)
    {
        if (!typeof(T).IsSerializable)
        {
            Debug.Log("The type must be serializable.");
        }

        // Don't serialize a null object, simply return the default for that object
        if (ReferenceEquals(source, null)) return default;

        Stream stream = new MemoryStream();
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, source);
        stream.Seek(0, SeekOrigin.Begin);
        return (T)formatter.Deserialize(stream);
    }
}
