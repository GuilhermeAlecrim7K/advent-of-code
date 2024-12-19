// var input = "12345";
// var input = "2333133121414131402";

var input = File.ReadAllText("../input").Trim();

int accIndex = 0;
var diskmap = input.ToCharArray()
    .Select(c => c - '0')
    .Chunk(2)
    .SelectMany((chunk, index) =>
    {
        var contentChunk = new DiskAddress(accIndex, index, chunk[0]);
        accIndex += chunk[0];
        if (chunk.Length == 1)
            return [contentChunk];

        DiskAddress[] result = [contentChunk, new DiskAddress(accIndex, -1, chunk[1])];
        accIndex += chunk[1];
        return result;
    }
    )
    .ToList()
    .RearrangeFiles();


Console.WriteLine(
    diskmap
        .SelectMany((d, i) =>
            d.ContentId < 0 ? [] : Enumerable.Range(d.Address, d.Length).Select((n) => (long)d.ContentId * n)
        )
        .Sum());

internal static class Extensions
{

    public static IList<DiskAddress> RearrangeFiles(this IList<DiskAddress> list)
    {
        for (int i = list.Count - (list.Count % 2 == 0 ? 2 : 1); i >= 1; i--)
        {
            if (list[i].ContentId < 0)
                continue;
            int j = list.IndexOfFirst(d => d.ContentId < 0 && d.Length >= list[i].Length);
            if (j < 0 || j > i)
                continue;
            var contentChunk = list[i];
            var freeChunk = list[j];
            if (freeChunk.Length > contentChunk.Length)
            {
                list.Insert(j + 1, new(freeChunk.Address + contentChunk.Length, -1, freeChunk.Length - contentChunk.Length));
                i++;
            }
            freeChunk.ContentId = contentChunk.ContentId;
            freeChunk.Length = contentChunk.Length;
            contentChunk.ContentId = -1;
        }
        return list;
    }

    public static int IndexOfFirst<T>(this IList<T> list, Func<T, bool> predicate)
    {
        for (int i = 0; i < list.Count; i++)
            if (predicate(list[i])) return i;
        return -1;
    }

}

class DiskAddress(int address, int contentId, int length)
{
    public int Address { get; set; } = address;
    public int ContentId { get; set; } = contentId;
    public int Length { get; set; } = length;
}