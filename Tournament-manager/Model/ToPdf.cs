using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Diagnostics;
using iText.Kernel.Colors;

namespace Tournament_manager.Model
{
    public class ToPdf
    {
        public async void GeneratePairingsPdf(Round round)
        {
            string tempFile = Path.Combine(Path.GetTempPath(), $"TournamentResults_{Guid.NewGuid()}.pdf");
            using (var writer = new PdfWriter(tempFile))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);
                    document.Add(new Paragraph("Tournament Pairings").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));

                    document.Add(new Paragraph($"Round {round.RoundNumber}").SetTextAlignment(TextAlignment.CENTER).SetFontSize(16));

                    var table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 3 })).UseAllAvailableWidth(); // Two columns: Player 1 and Player 2

                    foreach (var pairing in round.Matches)
                    {
                        table.AddCell($"Table {pairing.TableNumber}");
                        table.AddCell(pairing.Player1.Name);
                        if (pairing.Player2 == null)
                        {
                            table.AddCell("");
                        }
                        else
                        {
                            table.AddCell(pairing.Player2.Name);
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
            string tempFile = Path.Combine(Path.GetTempPath(), $"TournamentResults_{Guid.NewGuid()}.pdf");
            using (var writer = new PdfWriter(tempFile))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

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

                    var table = new Table(new float[] { 1, 4, 3 })
                        .UseAllAvailableWidth()
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER);

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
                        i++;
                    }

                    document.Add(table);
                }

                Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
            }
        }
    }
}
