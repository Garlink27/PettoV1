using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PettoV1.Services
{
    /// <summary>
    /// Servicio que conecta con la API de Groq para generar respuestas de IA.
    /// Groq es gratuito, muy rápido y sin límites diarios estrictos.
    /// </summary>
    public class APIService
    {
        // ── API Key cargada desde Secrets.cs (archivo local, no sube a GitHub) ─
        private const string ApiKey = Secrets.GroqApiKey;
        // ─────────────────────────────────────────────────────────────────────

        private const string Endpoint = "https://api.groq.com/openai/v1/chat/completions";

        // llama-3.1-8b-instant: rápido, gratuito, excelente en español
        private const string Model = "llama-3.1-8b-instant";

        /// <summary>
        /// Personalidad del asistente. Puedes editarla para cambiar el comportamiento.
        /// </summary>
        private const string SystemPrompt =
            "Eres Petto, un asistente virtual amigable y experto en cuidado de buenos habitos. " +
            "Ayudas a los dueños con consejos sobre alimentación, salud, higiene, ejercicio " +
            "y bienestar general" +
            "Responde en el idioma en el que te esten hablando, de forma clara, cariñosa y concisa. "
            ;

        private readonly HttpClient _httpClient;

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Envía un mensaje a Groq incluyendo el historial de la conversación.
        /// </summary>
        public async Task<string> EnviarMensajeAsync(
            string mensajeUsuario,
            IEnumerable<MensajeHistorial>? historialPrevio = null)
        {
            // Groq usa el formato OpenAI: array de messages con role/content
            var messages = new List<object>
            {
                new { role = "system", content = SystemPrompt }
            };

            if (historialPrevio != null)
            {
                foreach (var h in historialPrevio)
                {
                    messages.Add(new
                    {
                        role    = h.EsIA ? "assistant" : "user",
                        content = h.Texto
                    });
                }
            }

            messages.Add(new { role = "user", content = mensajeUsuario });

            var requestBody = new
            {
                model       = Model,
                messages,
                max_tokens  = 512,
                temperature = 0.8
            };

            var json        = JsonSerializer.Serialize(requestBody);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Groq usa autenticación Bearer en el header (no en la URL)
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Content = httpContent
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.TooManyRequests)
                        return "🕐 Demasiadas solicitudes. Espera un momento e intenta de nuevo.";

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return "🔑 La clave de API no es válida. Revisa la configuración en APIService.cs.";

                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        return "🚫 Acceso denegado. Verifica que la API key tenga permisos.";

                    var errorBody = await response.Content.ReadAsStringAsync();
                    return $"⚠️ Error {(int)response.StatusCode}: {errorBody}";
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var doc          = JsonDocument.Parse(responseJson);

                return doc.RootElement
                           .GetProperty("choices")[0]
                           .GetProperty("message")
                           .GetProperty("content")
                           .GetString()
                       ?? "No pude generar una respuesta, intenta de nuevo.";
            }
            catch (HttpRequestException)
            {
                return "⚠️ Sin conexión a internet. Verifica tu red e intenta de nuevo.";
            }
            catch (TaskCanceledException)
            {
                return "⏱️ La solicitud tardó demasiado. Intenta de nuevo.";
            }
            catch (Exception ex)
            {
                return $"⚠️ Error inesperado: {ex.Message}";
            }
        }
    }

    /// <summary>Representa un mensaje del historial para enviar como contexto.</summary>
    public record MensajeHistorial(bool EsIA, string Texto);
}
