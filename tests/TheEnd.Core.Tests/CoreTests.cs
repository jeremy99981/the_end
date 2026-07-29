using TheEnd.Core.Models;
using TheEnd.Core.Services;
using Xunit;

namespace TheEnd.Core.Tests;

public sealed class CoreTests
{
    [Fact] public void FrenchDateIsReadable() => Assert.Equal("Lundi 27 juillet 2026", FrenchDateFormatter.Format(new DateTime(2026, 7, 27)));

    [Fact] public void EmptySectionsHaveFallbackText()
    {
        var text = HandoverTextFormatter.ToPlainText(new HandoverDraft("", "", ""), new DateTime(2026, 7, 27));
        Assert.Contains("RÉCAP BRUN", text);
        Assert.Contains("Aucun élément renseigné.", text);
        Assert.Contains("OBJECTIFS DE DEMAIN", text);
    }

    [Fact] public async Task DraftStoreRoundTripsAccentsAndLongText()
    {
        var root = Path.Combine(Path.GetTempPath(), "RecapBrunTests", Guid.NewGuid().ToString("N"));
        var store = new DraftStore(root);
        var draft = new HandoverDraft("Élodie O’Neil", string.Join("\n", Enumerable.Repeat("Contrôler les ruptures : café, thé, œufs — demain", 100)), "Préparer l’exposition promotionnelle.");
        await store.SaveAsync(draft);
        var restored = await store.LoadAsync();
        Assert.Equal(draft, restored);
        store.Delete();
        Assert.False(File.Exists(store.FilePath));
        Directory.Delete(root, true);
    }
}
