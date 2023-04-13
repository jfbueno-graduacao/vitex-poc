using System.Collections.Concurrent;

namespace Connector.Producer.Worker;

internal sealed class HighTemperatureSharedState
{
    private readonly object _previousStates = new();

    private readonly List<Guid[]> _pastStates = new();
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

    public void ResetHistory()
    {
        lock (_previousStates)
        {
            _pastStates.Clear();
        }
    }

    public Guid[] GetRepeatedValues(int threshold)
    {
        Guid[][] previousStates;
        lock (_previousStates)
        {
            previousStates = _pastStates.ToArray();
        }

        if (previousStates is not { Length: > 0 })
            return Array.Empty<Guid>();

        return previousStates.SelectMany(g => g)
            .GroupBy(g => g)
            .Select(grp => new { Value = grp.Key, Count = grp.Count() })
            .Where(x => x.Count >= threshold)
            .Select(x => x.Value)
            .ToArray();
    }

    private void SavePastState()
    {
        if (_state.IsEmpty) 
            return;

        lock (_previousStates)
        {
            _pastStates.Add(_state.ToArray());
        }
    }
}
