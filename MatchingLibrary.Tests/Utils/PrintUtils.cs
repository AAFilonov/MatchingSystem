using System.Text;
using TestStand.Allocated;

namespace MatchingLibrary.Tests.Utils;

public class PrintUtils
{
    public static string toString(ValueTuple<SimpleAllocated, SimpleAllocated> tuple)
    {
        return $"pair: [{tuple.Item1}:{tuple.Item2}],";
    }

    public static string toString(ValueTuple<SimpleCapaciousAllocated, List<SimpleAllocated>> tuple)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{ ");
        if (tuple.Item2 != null)
        {
            tuple.Item2.ForEach(student => sb.Append(student + " "));
        }
        sb.Append("}");
        return $"pair: [{tuple.Item1} {sb.ToString()}], ";
    }

    public static string toString(KeyValuePair<SimpleAllocated, SimpleAllocated> pair)
    {
        return $"pair: [{pair.Key}: {pair.Value}], ";
    }

    public static string toString(Dictionary<SimpleAllocated, SimpleAllocated> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    public static string toString(List<(SimpleAllocated, SimpleAllocated)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    public static string toString(List<(SimpleCapaciousAllocated, List<SimpleAllocated>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }
}