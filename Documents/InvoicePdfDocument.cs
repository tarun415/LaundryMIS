using LaudaryMis.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LaudaryMis.Documents
{
    public class InvoicePdfDocument : IDocument
    {
        private readonly InvoiceMaster _invoice;

        public InvoicePdfDocument(InvoiceMaster invoice)
        {
            _invoice = invoice;
        }

        public DocumentMetadata GetMetadata()
        {
            return DocumentMetadata.Default;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                // ==========================
                // PAGE SETTINGS
                // ==========================

                page.Size(PageSizes.A4);

                page.Margin(18);

                page.DefaultTextStyle(x =>
                    x.FontSize(10)
                     .FontFamily("Arial"));

                // ==========================
                // HEADER
                // ==========================

                page.Header()
                    .Element(ComposeHeader);

                // ==========================
                // CONTENT
                // ==========================

                page.Content()
                    .PaddingVertical(8)
                    .Element(ComposeContent);

                // ==========================
                // FOOTER
                // ==========================

                page.Footer()
                    .Element(ComposeFooter);
            });
        }

        //===================================================
        // HEADER
        //===================================================

        private void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    // LEFT LOGO

                    row.ConstantItem(50)
                        .Height(50)
                        .Image("wwwroot/images/up-logo.png");

                    // CENTER

                    row.RelativeItem()
                        .AlignCenter()
                        .Column(col =>
                        {
                            col.Item()
                                .Text("Government of Uttar Pradesh")
                                .FontSize(16)
                                .Bold();

                            col.Item()
                                .Text("Directorate General Medical & Health Services")
                                .FontSize(11);
                         

                            col.Item()
                                .PaddingTop(3)
                                .Text("MONTHLY SERVICE INVOICE")
                                .Bold()
                                .FontSize(13)
                                .FontColor(Colors.Blue.Darken2);
                        });

                    // RIGHT LOGO

                    row.ConstantItem(50)
                        .Height(50)
                        .Image("wwwroot/images/nhm-logo-png.png");
                });

                column.Item()
                    .PaddingTop(6)
                    .LineHorizontal(1);

                column.Item()
                    .PaddingTop(6)
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Text(text =>
                            {
                                text.Span("Invoice No : ")
                                    .SemiBold();

                                text.Span(_invoice.InvoiceNo);
                            });

                        row.RelativeItem()
                            .AlignRight()
                            .Text(text =>
                            {
                                text.Span("Invoice Date : ")
                                    .SemiBold();

                                text.Span(
                                    _invoice.InvoiceDate.ToString("dd-MMM-yyyy"));
                            });
                    });

                column.Item()
                    .PaddingTop(6)
                    .LineHorizontal(1);
            });
        }

        //===================================================
        // BODY
        //===================================================

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(10);

                column.Item()
                    .ShowEntire()
                    .Element(ComposeInvoiceSummary);

                column.Item()
                    .ShowEntire()
                    .Element(ComposeHospitalInformation);

                column.Item()
                    .ShowEntire()
                    .Element(ComposePerformanceDetails);

                column.Item()
                    .ShowEntire()
                    .Element(ComposeFinancialSummary);

                column.Item()
                    .ShowEntire()
                    .Element(ComposeAmountInWords);

                column.Item()
                    .ShowEntire()
                    .Element(ComposeVerification);
            });
        }

        //===================================================
        // FOOTER
        //===================================================

        private void ComposeFooter(IContainer container)
        {
            container.PaddingTop(5)
                .BorderTop(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Row(row =>
                {
                    row.RelativeItem()
                        .Text("Laundry Management Information System")
                        .FontSize(8);

                    row.RelativeItem()
                        .AlignCenter()
                        .Text(DateTime.Now.ToString("dd-MMM-yyyy"))
                        .FontSize(8);

                    row.RelativeItem()
                        .AlignRight()
                        .Text(text =>
                        {
                            text.DefaultTextStyle(x => x.FontSize(8));

                            text.Span("Page ");

                            text.CurrentPageNumber();

                            text.Span(" of ");

                            text.TotalPages();
                        });
                });
        }
        //===================================================
        // INVOICE SUMMARY
        //===================================================

        private void ComposeInvoiceSummary(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Element(c => SectionHeader(c, "INVOICE SUMMARY"));

                column.Item().PaddingTop(5);

                column.Item().Element(c =>
                    InfoRow(c, "Invoice Number", _invoice.InvoiceNo));

                column.Item().Element(c =>
                    InfoRow(c, "Invoice Date",
                        _invoice.InvoiceDate.ToString("dd-MMM-yyyy")));

                //column.Item().Element(c =>
                //    InfoRow(c, "Billing Month",
                //        $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_invoice.InvoiceMonth)} {_invoice.InvoiceYear}"));

                column.Item().Element(c =>
                    InfoRow(c, "Invoice Status", _invoice.Status));

                column.Item().Element(c =>
                    InfoRow(c, "Net Payable",
                        $"₹ {_invoice.NetPayable:N2}",
                        true));
            });
        }

        //
        //===================================================
        // HOSPITAL & PROVIDER INFORMATION
        //===================================================

        private void ComposeHospitalInformation(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Element(c =>
                    SectionHeader(c, "HOSPITAL & PROVIDER INFORMATION"));

                column.Item().PaddingTop(5);

                column.Item().Element(c =>
                    InfoRow(c,
                        "Hospital Name",
                        _invoice.HospitalName));

                column.Item().Element(c =>
                    InfoRow(c,
                        "Provider Name",
                        _invoice.ProviderName));

                //column.Item().Element(c =>
                //    InfoRow(c,
                //        "Agreement ID",
                //        _invoice.AgreementId.ToString()));

                //column.Item().Element(c =>
                //    InfoRow(c,
                //        "Hospital ID",
                //        _invoice.HospitalId.ToString()));

                //column.Item().Element(c =>
                //    InfoRow(c,
                //        "Provider ID",
                //        _invoice.ProviderId.ToString()));
            });
        }

        //
        //===================================================
        // SECTION HEADER
        //===================================================

        private void SectionHeader(
            IContainer container,
            string title)
        {
            container.Column(column =>
            {
                column.Item()
                    .Text(title)
                    .Bold()
                    .FontSize(12)
                    .FontColor(Colors.Blue.Darken2);

                column.Item()
                    .PaddingTop(2)
                    .LineHorizontal(1)
                    .LineColor(Colors.Grey.Lighten2);
            });
        }

        //
        //===================================================
        // INFORMATION ROW
        //===================================================

        private void InfoRow(
            IContainer container,
            string label,
            string value,
            bool highlight = false)
        {
            container
                .PaddingVertical(3)
                .Row(row =>
                {
                    row.ConstantItem(140)
                        .Text(label)
                        .SemiBold();

                    row.ConstantItem(10)
                        .Text(":");

                    row.RelativeItem()
                        .Text(text =>
                        {
                            if (highlight)
                            {
                                text.Span(value)
                                    .Bold()
                                    .FontSize(11)
                                    .FontColor(Colors.Green.Darken2);
                            }
                            else
                            {
                                text.Span(value);
                            }
                        });
                });
        }
        //===================================================
        // PERFORMANCE DETAILS
        //===================================================

        private void ComposePerformanceDetails(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Element(c =>
                    SectionHeader(c, "PERFORMANCE DETAILS"));

                column.Item().PaddingTop(5);

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(TableHeaderCell).Text("Parameter");
                        header.Cell().Element(TableHeaderCell).AlignRight().Text("Value");
                    });

                    AddTableRow(table, "Sanctioned Beds", _invoice.SanctionedBeds.ToString());

                    AddTableRow(table, "No of Operational Beds", _invoice.BedOccupancy.ToString());

                    AddTableRow(table, "Rate Per Bed", $"₹ {_invoice.RatePerBed:N2}");

                    AddTableRow(table, "Average Score", $"{_invoice.AverageScore:N2} %");

                    AddTableRow(table, "Payment Percentage", $"{_invoice.PaymentPercentage:N2} %");
                });
            });
        }

        //
        //===================================================
        // FINANCIAL SUMMARY
        //===================================================

        private void ComposeFinancialSummary(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Element(c =>
                    SectionHeader(c, "FINANCIAL SUMMARY"));

                column.Item().PaddingTop(5);

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                    });

                    AddMoneyRow(table,
                        "Monthly Bill",
                        _invoice.MonthlyBill);

                    AddMoneyRow(table,
                        "Gross Payable",
                        _invoice.GrossPayable);

                    AddMoneyRow(table,
                        $"GST ({_invoice.GSTPercentage:N0}%)",
                        _invoice.GSTAmount);

                    AddMoneyRow(table,
                        "Invoice Amount",
                        _invoice.InvoiceAmount);

                    AddMoneyRow(table,
                        $"TDS ({_invoice.TDSPercentage:N0}%)",
                        -_invoice.TDSAmount);

                    AddMoneyRow(table,
                        "NET PAYABLE",
                        _invoice.NetPayable,
                        true);
                });
            });
        }

        //
        //===================================================
        // TABLE HEADER STYLE
        //===================================================

        private IContainer TableHeaderCell(IContainer container)
        {
            return container
                .Background(Colors.Blue.Lighten4)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(5)
                .PaddingHorizontal(6)
                .DefaultTextStyle(x => x.SemiBold());
        }

        //
        //===================================================
        // TABLE ROW
        //===================================================

        private void AddTableRow(
            TableDescriptor table,
            string parameter,
            string value)
        {
            table.Cell()
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten3)
                .PaddingVertical(4)
                .PaddingHorizontal(6)
                .Text(parameter);

            table.Cell()
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten3)
                .PaddingVertical(4)
                .PaddingHorizontal(6)
                .AlignRight()
                .Text(value);
        }

        //
        //===================================================
        // MONEY ROW
        //===================================================

        private void AddMoneyRow(
            TableDescriptor table,
            string title,
            decimal amount,
            bool total = false)
        {
            var background = total
                ? Colors.Green.Lighten4
                : Colors.White;

            table.Cell()
                .Background(background)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten3)
                .PaddingVertical(5)
                .PaddingHorizontal(6)
                .Text(title)
                .SemiBold();

            table.Cell()
                .Background(background)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten3)
                .PaddingVertical(5)
                .PaddingHorizontal(6)
                .AlignRight()
                .Text($"₹ {amount:N2}")
                .SemiBold();
        }
        //===================================================
        // AMOUNT IN WORDS
        //===================================================

        private void ComposeAmountInWords(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Element(c =>
                    SectionHeader(c, "AMOUNT IN WORDS"));

                column.Item()
                    .PaddingTop(5)
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(8)
                    .Text($"{NumberToWords((long)_invoice.NetPayable)} Rupees Only")
                    .FontSize(10)
                    .Italic();
            });
        }

        //===================================================
        // DIGITAL VERIFICATION
        //===================================================

        private void ComposeVerification(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Element(c =>
                    SectionHeader(c, "DIGITAL VERIFICATION"));

                column.Item().PaddingTop(5);

                // Verification Details
                column.Item().Element(c =>
                    InfoRow(c, "Generated By",
                        "Test..This is Login Operator Name"));

                column.Item().Element(c =>
                    InfoRow(c, "Generated On",
                        DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")));

                //column.Item().Element(c =>
                //    InfoRow(c, "Document Type",
                //        "System Generated Invoice"));

                //column.Item()
                //    .PaddingTop(5)
                //    .Text("This invoice is electronically generated and does not require a physical signature.")
                //    .FontSize(8)
                //    .Italic()
                //    .FontColor(Colors.Grey.Darken1);

                // Signature at Bottom
                column.Item()
      .PaddingTop(18)
      .Row(row =>
      {
          // Left side empty
          row.RelativeItem();

          // Right side signature
          row.RelativeItem()
              .AlignRight()
              .Column(col =>
              {
                  col.Item()
                      .Width(180)
                      .LineHorizontal(1);

                  col.Item()
                      .PaddingTop(4)
                      .AlignCenter()
                      .Text("Authorized Signatory")
                      .SemiBold()
                      .FontSize(10);

                  col.Item()
                      .AlignCenter()
                      .Text("Directorate General Medical & Health Services")
                      .FontSize(9)
                      .FontColor(Colors.Grey.Darken1);
              });
      });
            });
        }

        //
        //===================================================
        // INDIAN NUMBER TO WORDS
        //===================================================

        private static string NumberToWords(long number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string[] units =
            {
        "", "One", "Two", "Three", "Four",
        "Five", "Six", "Seven", "Eight",
        "Nine", "Ten", "Eleven", "Twelve",
        "Thirteen", "Fourteen", "Fifteen",
        "Sixteen", "Seventeen", "Eighteen",
        "Nineteen"
    };

            string[] tens =
            {
        "", "", "Twenty", "Thirty",
        "Forty", "Fifty", "Sixty",
        "Seventy", "Eighty", "Ninety"
    };

            string Convert(long n)
            {
                if (n < 20)
                    return units[n];

                if (n < 100)
                    return tens[n / 10] +
                           ((n % 10 > 0) ? " " + Convert(n % 10) : "");

                if (n < 1000)
                    return Convert(n / 100) + " Hundred" +
                           ((n % 100 > 0) ? " " + Convert(n % 100) : "");

                if (n < 100000)
                    return Convert(n / 1000) + " Thousand" +
                           ((n % 1000 > 0) ? " " + Convert(n % 1000) : "");

                if (n < 10000000)
                    return Convert(n / 100000) + " Lakh" +
                           ((n % 100000 > 0) ? " " + Convert(n % 100000) : "");

                return Convert(n / 10000000) + " Crore" +
                       ((n % 10000000 > 0) ? " " + Convert(n % 10000000) : "");
            }

            return Convert(number);
        }
    }
}
