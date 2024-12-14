List<long> stones = ParseInput("../input");
int blinkCount = 25;
int result = stones.Count;

foreach (var stone in stones)
{
    Node<long>? node = new(stone);

    for (int i = 0; i < blinkCount; i++)
        result += ChangeStones(node);

    node = null;
    GC.Collect();
}

Console.WriteLine(result);

int ChangeStones(Node<long>? stones)
{
    int dividedNodes = 0;
    for (Node<long>? node = stones; node is not null; node = node.Next)
    {
        switch (node.Value)
        {
            case 0:
                node.Value = 1;
                break;
            case long n when n.ToString().Length % 2 == 0:
                var split = n.ToString().Chunk(n.ToString().Length / 2).ToArray();
                node.Value = long.Parse(new string(split[0]));
                node.Next = new(long.Parse(new string(split[1])), node.Next);
                node = node.Next;
                dividedNodes++;
                break;
            default:
                node.Value *= 2024;
                break;
        }
    }
    return dividedNodes;
}


List<long> ParseInput(string path) => (File.ReadAllText(path).Split(" ").Select(long.Parse)).ToList();

class Node<T>(T value, Node<T>? next = null)
{
    public T Value { get; set; } = value;
    public Node<T>? Next { get; set; } = next;
}
