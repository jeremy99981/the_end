using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TheEnd.Core.Models;
using TheEnd.Core.Services;

namespace TheEnd.App;

internal static class PrintDocumentFactory
{
    // A4 in WPF device-independent pixels at 96 DPI.
    private const double A4Width = 793.7008;
    private const double A4Height = 1122.5197;
    private const double PageMargin = 56.6929; // 15 mm

    public static FixedDocument Create(HandoverDraft draft, DateTime date)
    {
        var document = new FixedDocument();
        var page = new FixedPage { Width = A4Width, Height = A4Height, Background = Brushes.White };
        var contentWidth = A4Width - (PageMargin * 2);
        var contentHeight = A4Height - (PageMargin * 2);

        var content = BuildContent(draft, date, contentWidth);
        var fittedContent = new Viewbox
        {
            Width = contentWidth,
            Height = contentHeight,
            Stretch = Stretch.Uniform,
            StretchDirection = StretchDirection.DownOnly,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Child = content
        };

        var root = new Grid { Width = contentWidth, Height = contentHeight, Margin = new Thickness(PageMargin) };
        root.Children.Add(fittedContent);
        page.Children.Add(root);

        var pageContent = new PageContent();
        pageContent.Child = page;
        document.Pages.Add(pageContent);
        return document;
    }

    private static StackPanel BuildContent(HandoverDraft draft, DateTime date, double width)
    {
        var content = new StackPanel { Width = width };
        content.Children.Add(new TextBlock
        {
            Text = "RÉCAP BRUN",
            FontFamily = new FontFamily("Segoe UI"),
            FontSize = 30,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.Black,
            Margin = new Thickness(0, 0, 0, 3)
        });
        content.Children.Add(new TextBlock
        {
            Text = "Transmission de fin de journée",
            FontFamily = new FontFamily("Segoe UI"),
            FontSize = 17,
            Foreground = Brushes.Gray,
            Margin = new Thickness(0, 0, 0, 25)
        });

        var metadata = new Grid { Margin = new Thickness(0, 0, 0, 28) };
        metadata.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        metadata.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        metadata.Children.Add(MetadataBlock("Date", FrenchDateFormatter.Format(date), 0));
        metadata.Children.Add(MetadataBlock("Équipier", HandoverTextFormatter.SectionText(draft.Teammate), 1));
        content.Children.Add(metadata);

        content.Children.Add(Section("CE QU’IL RESTE À FAIRE POUR DEMAIN", draft.RemainingTasks));
        content.Children.Add(Section("OBJECTIFS DE DEMAIN", draft.TomorrowGoals));
        return content;
    }

    private static FrameworkElement MetadataBlock(string label, string value, int column)
    {
        var block = new StackPanel { Margin = new Thickness(column == 0 ? 0 : 20, 0, 0, 0) };
        block.Children.Add(new TextBlock { Text = label.ToUpperInvariant(), FontSize = 11, FontWeight = FontWeights.Bold, Foreground = Brushes.Gray });
        block.Children.Add(new TextBlock { Text = value, FontSize = 15, Foreground = Brushes.Black, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 3, 0, 0) });
        Grid.SetColumn(block, column);
        return block;
    }

    private static Border Section(string title, string value)
    {
        var body = new StackPanel();
        body.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 16,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.Black,
            Margin = new Thickness(0, 0, 0, 10)
        });
        body.Children.Add(new TextBlock
        {
            Text = HandoverTextFormatter.SectionText(value),
            FontSize = 15,
            LineHeight = 22,
            Foreground = Brushes.Black,
            TextWrapping = TextWrapping.Wrap
        });
        return new Border
        {
            Background = new SolidColorBrush(Color.FromRgb(244, 247, 251)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(218, 224, 232)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(5),
            Padding = new Thickness(18),
            Margin = new Thickness(0, 0, 0, 18),
            Child = body
        };
    }
}
