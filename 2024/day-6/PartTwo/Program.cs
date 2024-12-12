using System.Diagnostics;
using System.Drawing;

StreamReader reader = new("../input");
// StreamReader reader = new("../test-input");

var goLeft = (int i, int j) => (i, j - 1);
var goRight = (int i, int j) => (i, j + 1);
var goUp = (int i, int j) => (i - 1, j);
var goDown = (int i, int j) => (i + 1, j);

Point initialPosition = new(-1, -1);
char[][] grid = reader.ReadToEnd().Split("\n").Select((string s, int i) =>
{
    if (initialPosition.X == -1)
    {
        initialPosition.X = s.IndexOf('^');
        initialPosition.Y = i;
    }
    return s.ToCharArray();
}).ToArray();

int result = 0;

List<ValueTuple<int, int>> visited = new();
Debug.Assert(initialPosition.X != -1);
grid[initialPosition.Y][initialPosition.X] = '.';

MapGuardsPositions(grid, initialPosition.Y, initialPosition.X, Direction.Up, ref visited);
List<ValueTuple<int, int, int>> loopVisited = new();
foreach (var item in visited)
{
    if (item.Item1 == initialPosition.Y && item.Item2 == initialPosition.X)
        continue;
    grid[item.Item1][item.Item2] = '#';
    if (HasLoop(grid, initialPosition.Y, initialPosition.X, Direction.Up, ref loopVisited))
        result++;
    grid[item.Item1][item.Item2] = '.';
    loopVisited.Clear();
}

Console.WriteLine(result);

// Slow but works
bool HasLoop(char[][] grid, int i, int j, Direction nextDirection, ref List<ValueTuple<int, int, int>> visited)
{
    Debug.Assert(i > 0 || j > 0);
    if (grid[i][j] == '.')
    {
        var index = visited.FindIndex(x => x.Item1 == i && x.Item2 == j);
        if (index == -1)
        {
            visited.Add((i, j, (int)nextDirection));
        }
        else if ((visited[index].Item3 & (int)nextDirection) == (int)nextDirection)
        {
            return true;
        }
        else
        {
            visited[index] = (i, j, visited[index].Item3 | (int)nextDirection);
        }
    }
    Func<int, int, (int, int)> move = nextDirection switch
    {
        Direction.Up => goUp,
        Direction.Down => goDown,
        Direction.Left => goLeft,
        Direction.Right => goRight,
        _ => throw new NotImplementedException()
    };
    (int newi, int newj) = move(i, j);
    if (newi < 0 || newi >= grid.Length) return false;
    if (newj < 0 || newj >= grid[newi].Length) return false;
    if (grid[newi][newj] == '#')
    {
        return HasLoop(grid, i, j, nextDirection switch
        {
            Direction.Up => Direction.Right,
            Direction.Left => Direction.Up,
            Direction.Down => Direction.Left,
            Direction.Right => Direction.Down,
            _ => throw new NotImplementedException()
        }, ref visited);
    }
    else
    {
        return HasLoop(grid, newi, newj, nextDirection, ref visited);
    }
}

void MapGuardsPositions(char[][] grid, int i, int j, Direction nextDirection, ref List<ValueTuple<int, int>> visited)
{
    Debug.Assert(i > 0 && j > 0);
    if (grid[i][j] == '.')
    {
        var index = visited.FindIndex(x => x.Item1 == i && x.Item2 == j);
        if (index == -1)
        {
            visited.Add((i, j));
        }
    }
    Func<int, int, (int, int)> move = nextDirection switch
    {
        Direction.Up => goUp,
        Direction.Down => goDown,
        Direction.Left => goLeft,
        Direction.Right => goRight,
        _ => throw new NotImplementedException()
    };
    (int newi, int newj) = move(i, j);
    if (newi < 0 || newi >= grid.Length) return;
    if (newj < 0 || newj >= grid[newi].Length) return;
    if (grid[newi][newj] == '#')
    {
        MapGuardsPositions(grid, i, j, nextDirection switch
        {
            Direction.Up => Direction.Right,
            Direction.Left => Direction.Up,
            Direction.Down => Direction.Left,
            Direction.Right => Direction.Down,
            _ => throw new NotImplementedException()
        }, ref visited);
    }
    else
    {
        MapGuardsPositions(grid, newi, newj, nextDirection, ref visited);
    }
}


enum Direction
{
    Up = 1,
    Down = 2,
    Left = 4,
    Right = 8
}
