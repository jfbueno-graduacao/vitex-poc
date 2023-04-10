using System.Collections.Concurrent;

namespace Integrador.Produtor.Worker;

internal sealed class HighTemperatureSharedState
{
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
        => _state = new ConcurrentBag<Guid>(peopleIds);

    public void Clear() => _state.Clear();
}
