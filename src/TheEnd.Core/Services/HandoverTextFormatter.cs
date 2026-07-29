using TheEnd.Core.Models;

namespace TheEnd.Core.Services;

public static class HandoverTextFormatter
{
    public static string SectionText(string value) => string.IsNullOrWhiteSpace(value) ? "Aucun élément renseigné." : value.Trim();
    public static string ToPlainText(HandoverDraft draft, DateTime date) =>
$"THE END\nTransmission de fin de journée\nDate : {FrenchDateFormatter.Format(date)}\nÉquipier : {SectionText(draft.Teammate)}\n\nCE QU’IL RESTE À FAIRE POUR DEMAIN\n{SectionText(draft.RemainingTasks)}\n\nOBJECTIFS DE DEMAIN\n{SectionText(draft.TomorrowGoals)}";
}
