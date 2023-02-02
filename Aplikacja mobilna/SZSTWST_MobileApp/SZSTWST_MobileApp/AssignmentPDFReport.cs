using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.Rendering;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;
using Xamarin.Essentials;
using Colors = MigraDocCore.DocumentObjectModel.Colors;
using Orientation = MigraDocCore.DocumentObjectModel.Orientation;

namespace SZSTWST_MobileApp
{
    internal class AssignmentPDFReport
    {
        private AssignmentTable thisAssignment;
        private List<AssetTable> assets;
        private string userName;
        private string userCreatorName;

        private Document document;

        public AssignmentPDFReport(AssignmentTable thisAssignment, List<AssetTable> assets, string userName, string userCreatorName)
        {
            try
            {
                GlobalFontSettings.FontResolver = new GenericFontResolver();
            }
            catch { }

            CultureInfo.CurrentCulture = new CultureInfo("pl-PL", false);
            this.thisAssignment = thisAssignment;
            this.assets = assets;
            this.userName = userName;
            this.userCreatorName = userCreatorName;
        }

        public async Task CreateReport()
        {
            CreateDocument();
            SetStyles();

            AddHeader();
            AddContent();
            AddFooter();

            await SaveShowPDF();
        }

        private void CreateDocument()
        {
            document = new Document();
            document.Info.Title = $"Asset Allocation Report - Assignment No. {thisAssignment.Id}";
            document.Info.Author = "Paweł Idryjan";
        }

        private void SetStyles()
        {
            // Modifying default style
            Style style = document.Styles[StyleNames.Normal];
            style.Font.Name = "OpenSans";
            style.Font.Size = 15;
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.PageBreakBefore = false;

            // Header style
            style = document.Styles[StyleNames.Header];
            style.Font.Name = "OpenSans";
            style.Font.Size = 18;
            style.Font.Color = Colors.Black;
            style.Font.Bold = true;
            style.Font.Underline = Underline.Single;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            // Footer style
            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Right);

            style = document.Styles.AddStyle("MyTableStyle", "Normal");
            style.Font.Size = 13;
            style.Font.Color = Colors.SlateBlue;
        }

        private void AddHeader()
        {
            var section = document.AddSection();

            var config = section.PageSetup;
            config.Orientation = Orientation.Portrait;
            config.TopMargin = "3cm";
            config.LeftMargin = 15;
            config.BottomMargin = "3cm";
            config.RightMargin = 15;
            config.PageFormat = PageFormat.A4;
            config.OddAndEvenPagesHeaderFooter = true;
            config.StartingNumber = 1;

            var oddHeader = section.Headers.Primary;

            var content = new Paragraph();
            content.AddText($"\tAsset Allocation Report - Assignment No. {thisAssignment.Id}\t");
            oddHeader.Add(content);
            oddHeader.AddTable();
        }

        void AddContent()
        {
            AddText1();
            AddTable();
        }

        private void AddFooter()
        {
            var content = new Paragraph();
            content.AddText(" Page ");
            content.AddPageField();
            content.AddText(" of ");
            content.AddNumPagesField();

            var section = document.LastSection;
            section.Footers.Primary.Add(content);

            var contentForEvenPages = content.Clone();
            contentForEvenPages.AddTab();
            contentForEvenPages.AddText("\tDate: ");
            contentForEvenPages.AddDateField(StringFormater.DateTimeFormat);

            section.Footers.EvenPage.Add(contentForEvenPages);
        }

        private void AddText1()
        {
            var text = $"Assignment Creation Time: {thisAssignment.CreationDate.ToString(StringFormater.ShortDateTimeFormat)}\n" +
                $"Start Time: {thisAssignment.StartDate.ToString(StringFormater.ShortDateTimeFormat)}\n" +
                $"Shedule End Time: {thisAssignment.StartDate.ToString(StringFormater.ShortDateTimeFormat)}\n" +
                $"Reported End Time: {(thisAssignment.IsFinished? ((DateTime)thisAssignment.FinishDate).ToString(StringFormater.ShortDateTimeFormat) : ".................")}";
            var section = document.LastSection;
            var mainParagraph = section.AddParagraph(text, "Normal");
            mainParagraph.AddLineBreak();

            text = $"Created By User: {userCreatorName}\n" +
                $"For The User: {userName}";
            mainParagraph = section.AddParagraph(text, "Normal");
            mainParagraph.AddLineBreak();
        }

        private void AddTable()
        {
            var titles = new string[] { "Id", "Name", "Code Name" };
            var borderColor = new Color(81, 125, 192);

            var section = document.LastSection;

            var table = section.AddTable();
            table.Style = "MyTableStyle";
            table.Borders.Color = borderColor;
            table.Borders.Visible = true;
            table.Borders.Width = 0.75;
            table.Rows.LeftIndent = 5;

            var column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("8cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("8cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            table.Rows.HeightRule = RowHeightRule.Exactly;
            table.Rows.Height = "2cm";

            var headerRow = table.AddRow();
            headerRow.HeadingFormat = true;
            headerRow.Format.Alignment = ParagraphAlignment.Center;
            headerRow.Format.Font.Bold = true;

            for (int i = 0; i < titles.Length; i++)
            {
                headerRow.Cells[i].AddParagraph(titles[i]);
                headerRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                headerRow.Cells[i].VerticalAlignment = VerticalAlignment.Center;
                headerRow.Shading.Color = Colors.PaleGoldenrod;
                headerRow.Borders.Width = 1;
            }

            foreach (var asset in assets)
            {
                var rowItem = table.AddRow();
                rowItem.TopPadding = 1.5;
                rowItem.Borders.Left.Width = 0.25;

                var IdCell = rowItem.Cells[0];
                IdCell.AddParagraph(asset.Id.ToString());

                var NameCell = rowItem.Cells[1];
                NameCell.AddParagraph(asset.Name);

                var CodeNameCell = rowItem.Cells[2];
                CodeNameCell.AddParagraph(asset.CodeName);
            }

            var row = table.AddRow();
            row.Borders.Visible = false;
        }

        private async Task SaveShowPDF()
        {
            string directoryPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            string fileName = $"AssetAllocationReport_{thisAssignment.Id}.pdf";
            string filePath = Path.Combine(directoryPath, fileName);

            PdfDocumentRenderer printer = new PdfDocumentRenderer(true);
            printer.Document = document;
            printer.RenderDocument();
            printer.PdfDocument.Save(filePath);

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
    }
}