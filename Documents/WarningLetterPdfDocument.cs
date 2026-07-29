using LaudaryMis.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LaudaryMis.Documents
{
    public class WarningLetterPdfDocument : IDocument
    {
        private readonly WarningLetterMaster _model;

        public WarningLetterPdfDocument(WarningLetterMaster model)
        {
            _model = model;
        }

        public DocumentMetadata GetMetadata()
            => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(40);

                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .Element(ComposeHeader);

                page.Content()
                    .Element(ComposeBody);

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                    });
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item()
                    .AlignCenter()
                    .Text("WARNING LETTER")
                    .Bold()
                    .FontSize(20);

                column.Item()
                    .AlignCenter()
                    .Text("Laundry Management System");

                column.Item()
                    .PaddingTop(15);

                column.Item()
                    .Text($"Warning No : {_model.WarningNo}");

                column.Item()
                    .Text($"Warning Date : {_model.WarningDate:dd-MMM-yyyy}");
            });
        }

        private void ComposeBody(IContainer container)
        {
            container.PaddingTop(20)
                .Column(column =>
                {
                    column.Spacing(8);

                    column.Item().Text($"To,");
                    column.Item().Text(_model.ProviderName);
                    column.Item().Text(_model.HospitalName);

                    column.Item().PaddingTop(10);

                    column.Item()
                        .Text($"Subject : {_model.Subject}")
                        .Bold();

                    column.Item().PaddingTop(10);

                    column.Item().Text(
                        $"This warning letter is issued to your organization for unsatisfactory laundry services provided under Agreement No {_model.AgreementNo}.");

                    column.Item().PaddingTop(10);

                    column.Item().Text($"Performance Score : {_model.PerformanceScore}%");

                    column.Item().Text($"Payment Percentage : {_model.PaymentPercentage}%");

                    column.Item().PaddingTop(15);

                    column.Item()
                        .Text("Reason")
                        .Bold();

                    column.Item().Text(_model.Reason);

                    column.Item().PaddingTop(15);

                    column.Item()
                        .Text("Remarks")
                        .Bold();

                    column.Item().Text(_model.Remarks);

                    column.Item().PaddingTop(25);

                    column.Item().Text(
                        "You are instructed to improve the quality of service immediately. Failure to comply may result in further action as per the agreement terms.");

                    column.Item().PaddingTop(60);

                    column.Item()
                        .AlignRight()
                        .Text("Authorized Signatory")
                        .Bold();
                });
        }
    }
}