using Plugin.Maui.Audio;

namespace TurnBase.Services;

public class AudioService
{
    private readonly IAudioManager _audioManager;

    public AudioService(IAudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    public async Task PlaySfx(string fileName)
    {
        try
        {
            var stream =
                await FileSystem.OpenAppPackageFileAsync(fileName);

            var player = _audioManager.CreatePlayer(stream);

            player.Play();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}