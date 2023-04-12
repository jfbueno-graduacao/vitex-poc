using System.Collections.Concurrent;

namespace Connector.Producer.Worker;

internal sealed class HighTemperatureSharedState
{
    private List<Guid[]> _pastStates = new();

    private ConcurrentBag<Guid> _state = new();

    /// <summary>
    /// Define se há algum item no estado compartilhado
    /// </summary>
    public bool HasAny => !_state.IsEmpty;

    /// <summary>
    /// Retorna a quantidade de itens existentes no estado compartilhado
    /// </summary>
    public int Count => _state.Count;

    /// <summary>
    /// Retorna todos os itens do estado compartilhado 
    /// </summary>
    public IReadOnlyCollection<Guid> Items => _state;
    
    /// <summary>
    /// Define os itens do estado compartilhado
    /// </summary>
    public void SetItems(IReadOnlyCollection<Guid> peopleIds)
    {
        SavePastState();
        _state = new ConcurrentBag<Guid>(peopleIds);
    }

    public void Clear()
    {
        SavePastState();
        _state.Clear();
    }

    public void ResetHistory() => _pastStates.Clear();
    
    public Guid[] GetRepeatedValues(int threshold)
    {
        if (_pastStates is not { Count: > 0 })
            return Array.Empty<Guid>();

        return _pastStates.SelectMany(g => g)
            .GroupBy(g => g)
            .Select(grp => new { Value = grp.Key, Count = grp.Count() })
            .Where(x => x.Count >= threshold)
            .Select(x => x.Value)
            .ToArray();
    }

    private void SavePastState()
        => _pastStates.Add(_state.ToArray());
}
