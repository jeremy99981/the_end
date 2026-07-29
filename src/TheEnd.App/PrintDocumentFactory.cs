using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using TheEnd.Core.Models;
using TheEnd.Core.Services;

namespace TheEnd.App;

internal static class PrintDocumentFactory
{
    public static FlowDocument Create(HandoverDraft draft, DateTime date)
    {
        var document = new FlowDocument { PagePadding = new Thickness(72), FontFamily = new FontFamily("Segoe UI"), FontSize = 13, ColumnWidth = double.PositiveInfinity };
        Add(document, "RÉCAP BRUN", 28, true, Brushes.Black, 0);
        Add(document, "Transmission de fin de journée", 16, false, Brushes.Gray, 2);
        Add(document, $"Date : {FrenchDateFormatter.Format(date)}", 13, false, Brushes.Black, 24);
        Add(document, $"Équipier : {HandoverTextFormatter.SectionText(draft.Teammate)}", 13, false, Brushes.Black, 2);
        AddSection(document, "CE QU’IL RESTE À FAIRE POUR DEMAIN", draft.RemainingTasks);
        AddSection(document, "OBJECTIFS DE DEMAIN", draft.TomorrowGoals);
        return document;
    }

    private static void AddSection(FlowDocument doc, string title, string value)
    {
        Add(doc, title, 15, true, Brushes.Black, 26);
        var paragraph = new Paragraph { Margin = new Thickness(0, 4, 0, 0), LineHeight = 19 };
        paragraph.Inlines.Add(new Run(HandoverTextFormatter.SectionText(value)));
        doc.Blocks.Add(paragraph);
    }

    private static void Add(FlowDocument doc, string text, double size, bool bold, Brush color, double top)
    {
        doc.Blocks.Add(new Paragraph(new Run(text)) { FontSize = size, FontWeight = bold ? FontWeights.Bold : FontWeights.Normal, Foreground = color, Margin = new Thickness(0, top, 0, 0) });
    }
}
