using Microsoft.Maui.Graphics;

namespace PettoV1.Charts
{
    /// <summary>
    /// Gráfica de barras nativa (IDrawable) para mostrar Total / Completadas / Pendientes.
    /// No requiere ningún paquete externo — usa el motor de dibujo de MAUI.
    /// </summary>
    public class TareasChartDrawable : IDrawable
    {
        public int Total { get; set; }
        public int Completadas { get; set; }
        public int Pendientes { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float w = dirtyRect.Width;
            float h = dirtyRect.Height;

            // ── Sin datos ─────────────────────────────────────────────────
            if (Total == 0 && Completadas == 0 && Pendientes == 0)
            {
                canvas.FontSize = 13;
                canvas.FontColor = Color.FromArgb("#80D5F9FF");
                canvas.DrawString("Sin tareas en el período seleccionado",
                    w / 2f, h / 2f, HorizontalAlignment.Center);
                return;
            }

            // ── Métricas de layout ────────────────────────────────────────
            const float padLeft = 20f;
            const float padRight = 20f;
            const float padTop = 30f;   // espacio sobre las barras (números)
            const float padBottom = 38f;   // espacio bajo las barras (etiquetas)

            float chartH = h - padTop - padBottom;
            float chartW = w - padLeft - padRight;

            int maxVal = Math.Max(1, Total);
            float sectionW = chartW / 3f;
            float barW = sectionW * 0.52f;

            int[] values = { Total, Completadas, Pendientes };
            string[] labels = { "Total", "Completadas", "Pendientes" };
            Color[] colors =
            {
                Color.FromArgb("#66FFFFFF"),   // blanco glass
                Color.FromArgb("#FF26C6DA"),   // cyan
                Color.FromArgb("#FFFFCC80")    // ámbar
            };

            // ── Línea base ────────────────────────────────────────────────
            canvas.StrokeColor = Color.FromArgb("#33FFFFFF");
            canvas.StrokeSize = 1;
            canvas.DrawLine(padLeft, padTop + chartH, padLeft + chartW, padTop + chartH);

            // ── Barras ────────────────────────────────────────────────────
            for (int i = 0; i < 3; i++)
            {
                float ratio = values[i] / (float)maxVal;
                float barH = values[i] > 0 ? Math.Max(ratio * chartH, 4f) : 0f;
                float sectionX = padLeft + i * sectionW;
                float barX = sectionX + (sectionW - barW) / 2f;
                float barY = padTop + chartH - barH;

                // Sombra suave (barra ligeramente más ancha y oscura)
                canvas.FillColor = Color.FromArgb("#1A000000");
                canvas.FillRoundedRectangle(barX + 3, barY + 3, barW, barH, 7);

                // Barra principal
                canvas.FillColor = colors[i];
                canvas.FillRoundedRectangle(barX, barY, barW, barH, 7);

                // Número sobre la barra
                if (values[i] > 0)
                {
                    canvas.FontSize = 16;
                    canvas.FontColor = Colors.White;
                    canvas.DrawString(values[i].ToString(),
                        barX + barW / 2f, barY - 8f, HorizontalAlignment.Center);
                }

                // Etiqueta bajo la barra
                canvas.FontSize = 11;
                canvas.FontColor = Color.FromArgb("#99D5F9FF");
                canvas.DrawString(labels[i],
                    barX + barW / 2f, padTop + chartH + 18f, HorizontalAlignment.Center);
            }
        }
    }
}