// var input = "12345";
// var input = "2333133121414131402";

var input = File.ReadAllText("../input").Trim();

var diskmap = input.ToCharArray()
    .Select(c => c - '0')
    .Chunk(2)
    .SelectMany((c, i) => Enumerable.Repeat(i, c[0])
    .Concat(Enumerable.Repeat(-1, c.Length == 2 ? c[1] : 0)))
    .ToArray()
    .MakeContiguousFreeSpace()
    ;

Console.WriteLine(diskmap
    .TakeWhile(n => n >= 0)
    .Select((n, i) => (long)n * i)
    .Sum()
);

internal static class Extensions
{

    public static IList<int> MakeContiguousFreeSpace(this IList<int> list)
    {
        Func<int, bool> isFree = n => n == -1;
        Func<int, bool> isTaken = n => n >= 0;
        for (
            int l = list.IndexOfFirst(isFree), r = list.IndexOfLast(isTaken, list.Count - 1);
            l < r && l >= 0 && r >= 0;
            l = list.IndexOfFirst(isFree, l + 1), r = list.IndexOfLast(isTaken, r - 1))
        {
            (list[l], list[r]) = (list[r], list[l]);
        }
        return list;
    }

    public static int IndexOfFirst<T>(this IList<T> list, Func<T, bool> predicate, int start = 0)
    {
        if (start < 0 || start >= list.Count) return -1;
        for (int i = start; i < list.Count; i++)
            if (predicate(list[i])) return i;
        return -1;
    }

    public static int IndexOfLast<T>(this IList<T> list, Func<T, bool> predicate, int start)
    {
        if (start < 0 || start >= list.Count) return -1;
        for (int i = start; i >= 0; i--)
            if (predicate(list[i])) return i;
        return -1;
    }

}