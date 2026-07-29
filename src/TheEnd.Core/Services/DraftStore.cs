using System.Text.Json;
using System.Text;
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

    public void Save(HandoverDraft draft)
    {
        var directory = Path.GetDirectoryName(_filePath)!;
        Directory.CreateDirectory(directory);
        var temporaryPath = $"{_filePath}.{Guid.NewGuid():N}.tmp";
        try
        {
            var json = JsonSerializer.Serialize(draft);
            File.WriteAllText(temporaryPath, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            File.Move(temporaryPath, _filePath, overwrite: true);
        }
        finally
        {
            try { if (File.Exists(temporaryPath)) File.Delete(temporaryPath); } catch (IOException) { }
        }
    }

    public Task SaveAsync(HandoverDraft draft, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => Save(draft), cancellationToken);
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
        catch (UnauthorizedAccessException) { return null; }
    }

    public void Delete() { try { if (File.Exists(_filePath)) File.Delete(_filePath); } catch (IOException) { } }
}
