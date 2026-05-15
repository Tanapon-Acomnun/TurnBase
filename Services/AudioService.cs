using Plugin.Maui.Audio;

namespace TurnBase.Services;

public class AudioService
{
    private readonly IAudioManager _audioManager;

    private IAudioPlayer _bgmPlayer;

    // กัน player โดน GC ลบ
    private readonly List<IAudioPlayer> _sfxPlayers = new();

    public AudioService(IAudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    // =========================
    // SOUND EFFECT
    // =========================
    public async Task PlaySfx(string fileName)
    {
        try
        {
            var stream =
                await FileSystem.OpenAppPackageFileAsync(fileName);

            var player =
                _audioManager.CreatePlayer(stream);

            _sfxPlayers.Add(player);

            player.Play();

            // ลบเมื่อเล่นจบ
            player.PlaybackEnded += (s, e) =>
            {
                player.Dispose();
                _sfxPlayers.Remove(player);
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    // =========================
    // BGM
    // =========================
    public async Task PlayBgm(string fileName)
    {
        try
        {
            _bgmPlayer?.Stop();

            var stream =
                await FileSystem.OpenAppPackageFileAsync(fileName);

            _bgmPlayer =
                _audioManager.CreatePlayer(stream);

            _bgmPlayer.Loop = true;

            _bgmPlayer.Play();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    public void StopBgm()
    {
        _bgmPlayer?.Stop();
    }
}