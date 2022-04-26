using TestStand.Allocated;

namespace MatchingLibrary;

public class OneToOneAllocation<T, U> //OneToOneAllocation only
    where T : class, IAllocated
    where U : class, IAllocated
{
    public OneToOneAllocation(List<T> active, List<U> passive)
    {
        this.active = active;
        this.passive = passive;
        active.ForEach(allocated => activePreference.Add(allocated, new List<U>()));
        passive.ForEach(allocated => passivePreference.Add(allocated, new List<T>()));
    }

    public List<T> active { get; set; }
    public List<U> passive { get; set; }

    public Dictionary<T, List<U>> activePreference { get; set; } = new();
    public Dictionary<U, List<T>> passivePreference { get; set; } = new();

    public Dictionary<U, T> pairs { get; set; } = new();

    public void setPair(U u, T t)
    {
        pairs[u] = t;
    }

    public T? getPairT(U u)
    {
        try
        {
            return pairs[u];
        }
        catch (KeyNotFoundException e)
        {
            return null;
        }
    }


    public List<(T, U)> calcFinalAllocation()
    {
        var result = new List<(T, U)>();
        foreach (var pair in this.pairs)
        {
            result.Add(new(pair.Value, pair.Key));
        }

        passive.Where(allocated => getPairT(allocated) == null).ToList().ForEach(allocated =>
        {
            result.Add(new ValueTuple<T, U>(null as T, allocated));
        });

        active.Where(allocated => !pairs.ContainsValue(allocated)).ToList().ForEach(allocated =>
        {
            result.Add(new ValueTuple<T, U>(allocated, null as U));
        });
        return result;
    }

    public List<U> getPreferencesActive(T t)
    {
        if (active.Contains(t))
            return activePreference[t];
        else
            throw new ArgumentException($"Аргумент {t} не входит в список распределяемых!");
    }

    public List<T> getPreferencesPassive(U u)
    {
        if (passive.Contains(u))
            return passivePreference[u];
        else
            throw new ArgumentException($"Аргумент {u} не входит в список распределяемых!");
    }

    public void setPreferencesActive(T t, List<U> pref)
    {
        if (active.Contains(t))
            activePreference[t] = pref;
        else
            throw new ArgumentException($"Аргумент {t} не входит в список распределяемых!");
    }

    public void setPreferencesPassive(U u, List<T> pref)
    {
        if (passive.Contains(u))
            passivePreference[u] = pref;
        else
            throw new ArgumentException($"Аргумент {u} не входит в список распределяемых!");
    }

    public void deleteFromPref(T t, U u)
    {
        var preferencess = getPreferencesActive(t);
        preferencess.Remove(u); //лучше не удалять а сделать неактивными
        setPreferencesActive(t, preferencess);
    }
}