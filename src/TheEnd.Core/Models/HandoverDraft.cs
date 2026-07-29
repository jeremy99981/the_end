namespace TheEnd.Core.Models;

public sealed record HandoverDraft(string Teammate, string RemainingTasks, string TomorrowGoals)
{
    public bool IsEmpty => string.IsNullOrWhiteSpace(Teammate)
        && string.IsNullOrWhiteSpace(RemainingTasks)
        && string.IsNullOrWhiteSpace(TomorrowGoals);
}
