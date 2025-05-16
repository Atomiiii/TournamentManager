using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Diagnostics;
using iText.Kernel.Colors;
using iText.IO.Font;
using iText.Kernel.Font;
using static iText.Kernel.Font.PdfFontFactory;

namespace Tournament_manager.Model
{
    public class ToPdf
    {
        public async void GeneratePairingsPdf(Round round)
        {
            // Generate to a temp file => doesn't fill the memory
            string tempFile = Path.Combine(Path.GetTempPath(), $"TournamentPairing_{Guid.NewGuid()}.pdf");
            using (var writer = new PdfWriter(tempFile))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);
                    // Set font which supports special characters (č, š, ř,...)
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    PdfFont font = CreateFont(fontPath, PdfEncodings.IDENTITY_H, EmbeddingStrategy.PREFER_EMBEDDED);
                    document.SetFont(font);
                    document.Add(new Paragraph("Tournament Pairings").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));

                    document.Add(new Paragraph($"Round {round.RoundNumber}").SetTextAlignment(TextAlignment.CENTER).SetFontSize(16));

                    var table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 3 })).UseAllAvailableWidth();
                    // Header
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Table"))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(5));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Player1"))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(5));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Player2"))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(5));

                    // filling table
                    foreach (var pairing in round.Matches)
                    {
                        table.AddCell($"Table {pairing.TableNumber}");
                        table.AddCell($"{pairing.Player1.Name} \n {pairing.Player1.Wins}-{pairing.Player1.Draws}-{pairing.Player1.Losses} \n VP: {pairing.Player1.Points}");
                        if (pairing.Player2 == null)
                        {
                            table.AddCell("");
                        }
                        else
                        {
                            table.AddCell($"{pairing.Player2.Name} \n {pairing.Player2.Wins}-{pairing.Player2.Draws}-{pairing.Player2.Losses} \n VP: {pairing.Player2.Points}");
                        }
                    }

                    document.Add(table);
                    document.Add(new Paragraph("\n"));
                }
                Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
            }
        }

        public async void GenerateResultsPdf(Tournament tournament)
        {
            // Generate to a temp file => doesn't fill the memory
            string tempFile = Path.Combine(Path.GetTempPath(), $"TournamentResults_{Guid.NewGuid()}.pdf");
            using (var writer = new PdfWriter(tempFile))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);
                    // Set font which supports special characters (č, š, ř,...)
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    PdfFont font = CreateFont(fontPath, PdfEncodings.IDENTITY_H, EmbeddingStrategy.PREFER_EMBEDDED);

                    document.Add(new Paragraph("Tournament Results")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(24)
                        .SetMarginBottom(20));
                    document.Add(new Paragraph($"Tournament Name: {tournament.Name}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16)
                        .SetMarginBottom(10));
                    document.Add(new Paragraph($"Date: {tournament.Id}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16)
                        .SetMarginBottom(10));

                    var table = new Table(new float[] { 1, 4, 3, 1 })
                        .UseAllAvailableWidth()
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    // header
                    table.AddHeaderCell(new Cell().Add(new Paragraph("#"))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(5));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Player Name"))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(5));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("W-D-L"))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(5));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Victory Points"))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(5));

                    // filling table
                    int i = 1;
                    foreach (var player in tournament.Players)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(i.ToString()))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPadding(5));
                        table.AddCell(new Cell().Add(new Paragraph(player.Name))
                            .SetPadding(5));
                        table.AddCell(new Cell().Add(new Paragraph($"{player.Wins}-{player.Draws}-{player.Losses}"))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPadding(5));
                        table.AddCell(new Cell().Add(new Paragraph(player.Points.ToString()))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPadding(5));
                        i++;
                    }

                    document.Add(table);
                }

                Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
            }
        }
    }
}
