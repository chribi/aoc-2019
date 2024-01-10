namespace LibAoc;

public static class LinqExt {
    /// <summary>
    /// Aggregate and return all intermediate results.
    /// </summary>
    public static IEnumerable<TAcc> AggregateCollect<T, TAcc>(this IEnumerable<T> source,
            TAcc initial,
            Func<TAcc, T, TAcc> aggregate) {
        yield return initial;

        foreach (var element in source) {
            initial = aggregate(initial, element);
            yield return initial;
        }
    }

    public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> source) {
        var first = true;
        var last = default(T);
        foreach (var elem in source) {
            if (first) {
                last = elem;
                first = false;
                continue;
            }
            yield return (last!, elem);
            last = elem;
        }
    }
}
