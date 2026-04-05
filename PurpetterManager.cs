using System;
using Microsoft.Win32;
using PuppeteerSharp;

namespace work;

public class PurpetterManager
{
    private static string urlToVisit = "https://www.youtube.com/watch?v=VCwqmMDyWNk";

    private static string? GetDefaultBrowserPath()
    {
        // 1. Buscamos el ID del programa asociado a HTTP
        string userChoicePath = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";

        using RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoicePath);
        if (userChoiceKey == null)
            return null; // No se pudo encontrar la clave

        object progId = userChoiceKey.GetValue("ProgId");
        if (progId == null)
            return null; // No se pudo encontrar el ProgId

        // 2. Buscamos la ruta del ejecutable para ese ProgId
        string exePath = $@"{progId.ToString()}\shell\open\command";
        using RegistryKey commandKey = Registry.ClassesRoot.OpenSubKey(exePath);
        if (commandKey == null)
            return null; // No se pudo encontrar la clave del comando

        object? path = commandKey.GetValue(null);
        if (path == null)
            return null; // No se pudo encontrar la ruta del ejecutable

        // El resultado suele venir entre comillas y con parámetros (ej: "C:\...\chrome.exe" -- "%1")
        // Limpiamos la cadena:
        return path.ToString().Split('"')[1];
    }

    public static async Task Start()
    {
        try
        {
            string? gg = GetDefaultBrowserPath();
            if (gg is null)
            {
                Console.WriteLine("No se pudo determinar el navegador predeterminado.");
                return;
            }

            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false, ExecutablePath = gg });
            var page = await browser.NewPageAsync();
            await page.GoToAsync(urlToVisit);
            await page.Keyboard.PressAsync("Enter");

            await Task.Delay(30000); // Esperamos 5 segundos para ver la página antes de cerrar
            await page.CloseAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al iniciar Puppeteer: {ex.Message}");
        }
    }

}
