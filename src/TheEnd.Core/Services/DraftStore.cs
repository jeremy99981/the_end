using System.Text.Json;
using TheEnd.Core.Models;

namespace TheEnd.Core.Services;

public sealed class DraftStore
{
    private readonly string _filePath;

    public DraftStore(string? localAppData = null)
    {
        var root = localAppData ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _filePath = Path.Combine(root, "RecapBrun", "draft.json");
    }

    public string FilePath => _filePath;

    public async Task SaveAsync(HandoverDraft draft, CancellationToken cancellationToken = default)
    {
        var directory = Path.GetDirectoryName(_filePath)!;
        Directory.CreateDirectory(directory);
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, draft, cancellationToken: cancellationToken);
    }

    public async Task<HandoverDraft?> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_filePath)) return null;
        try
        {
            await using var stream = File.OpenRead(_filePath);
            return await JsonSerializer.DeserializeAsync<HandoverDraft>(stream, cancellationToken: cancellationToken);
        }
        catch (JsonException) { return null; }
        catch (IOException) { return null; }
    }

    public void Delete() { try { if (File.Exists(_filePath)) File.Delete(_filePath); } catch (IOException) { } }
}
