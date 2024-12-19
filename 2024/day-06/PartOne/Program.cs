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
Debug.Assert(initialPosition.X != -1);
MapGuardsPositions(grid, initialPosition.Y, initialPosition.X, Direction.Up, ref result);
Console.WriteLine(result);

void MapGuardsPositions(char[][] grid, int i, int j, Direction nextDirection, ref int newPositionCount)
{
    if (grid[i][j] != 'X')
        newPositionCount++;
    grid[i][j] = 'X';
    var move = nextDirection switch
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
        }, ref newPositionCount);
    }
    else
    {
        MapGuardsPositions(grid, newi, newj, nextDirection, ref newPositionCount);
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}