using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace work;

public class Worker : BackgroundService
{
    private async Task RunAudio()
    {
        AudioManager audioManager = new AudioManager();
        audioManager.CambiarVolumenSistema(0.1f); // Ajustar el volumen al 50%

        using WaveOutEvent waveOutEvent = audioManager.InitAll();

        if (waveOutEvent.PlaybackState == PlaybackState.Stopped)
            waveOutEvent.Play();

        while (waveOutEvent.PlaybackState == PlaybackState.Playing)
        {
            await Task.Delay(1000);
        }
    }

    private async Task MonitorAudioAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunAudio();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el audio: {ex.Message}");
            }

            if (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }

    private async Task MonitorPuppeteerAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PurpetterManager.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Puppeteer: {ex.Message}");
            }

            if (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task audioTask = MonitorAudioAsync(stoppingToken);
        Task puppeteerTask = MonitorPuppeteerAsync(stoppingToken);

        await Task.WhenAll(audioTask, puppeteerTask);
    }
}
