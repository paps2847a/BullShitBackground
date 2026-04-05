using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace work;

public class Worker : BackgroundService
{
    private async Task RunAudio()
    {
        AudioManager audioManager = new AudioManager();
        audioManager.CambiarVolumenSistema(0.2f); // Ajustar el volumen al 50%

        using WaveOutEvent waveOutEvent = audioManager.InitAll();

        if (waveOutEvent.PlaybackState == PlaybackState.Stopped)
            waveOutEvent.Play();

        while (waveOutEvent.PlaybackState == PlaybackState.Playing)
        {
            await Task.Delay(1000);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.WhenAll(RunAudio(), PurpetterManager.Start());
        }
    }
}
