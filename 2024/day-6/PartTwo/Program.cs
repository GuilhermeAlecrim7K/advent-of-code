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

List<ValueTuple<int, int, Direction>> visited = new();
grid[initialPosition.Y][initialPosition.X] = '.';

MapGuardsPositions(grid, initialPosition.Y, initialPosition.X, Direction.Up, ref visited);
List<ValueTuple<int, int, int>> loopVisited = new();
ValueTuple<int, int, Direction> previous = new(initialPosition.Y, initialPosition.X, Direction.Up);
for (int i = 0; i < visited.Count; i++)
{
    var item = visited[i];
    if (item.Item1 == initialPosition.Y && item.Item2 == initialPosition.X)
        continue;
    grid[item.Item1][item.Item2] = '#';
    if (HasLoop(grid, previous.Item1, previous.Item2, previous.Item3, ref loopVisited))
        result++;
    grid[item.Item1][item.Item2] = '.';
    loopVisited.Clear();
    previous = (visited[i]);
}

Console.WriteLine(result);

bool HasLoop(char[][] grid, int i, int j, Direction nextDirection, ref List<ValueTuple<int, int, int>> visited)
{
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

void MapGuardsPositions(char[][] grid, int i, int j, Direction nextDirection, ref List<ValueTuple<int, int, Direction>> visited)
{
    if (grid[i][j] == '.')
    {
        if (!visited.Any(x => x.Item1 == i && x.Item2 == j))
            visited.Add(new(i, j, nextDirection));
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
