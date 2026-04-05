using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace work;

public class AudioManager
{
    public void CambiarVolumenSistema(float nivel)
    {
        // 1. Obtener el enumerador de dispositivos
        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();

        // 2. Obtener el dispositivo de salida por defecto (altavoces/auriculares)
        MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        // 3. Ajustar el volumen (Rango de 0.0 a 1.0)
        // 0.0f es silencio total, 1.0f es 100% de volumen
        device.AudioEndpointVolume.MasterVolumeLevelScalar = nivel;

        Console.WriteLine($"Volumen ajustado al: {nivel * 100}%");
    }

    public WaveOutEvent InitAll()
    {
        string audioFilePath = $".{Path.DirectorySeparatorChar}assets{Path.DirectorySeparatorChar}audio.mp3";

        AudioFileReader audioFileReader = new AudioFileReader(audioFilePath);
        WaveOutEvent waveOutEvent = new WaveOutEvent();
        waveOutEvent.Init(audioFileReader);

        return waveOutEvent;
    }

}
