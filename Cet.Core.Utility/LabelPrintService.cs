using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cet.Core.Utility
{
    public class LabelPrintService<T1>
       where T1 : PrintJobInfo
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;

        // Routine to provide to the report renderer, in order to
        //    save an image for each page of the report.
        private Stream CreateStream(string name,
          string fileNameExtension, Encoding encoding,
          string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        // Export the given report as an EMF (Enhanced Metafile) file.
        public void Export(LocalReport report, string DeviceInfo)
        {
            string deviceInfo = DeviceInfo;
            //string deviceInfo = string.Empty;
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        public void Printing(IList<T1> reports)
        {
            foreach (var report in reports)
            {
                PrintingOnePage(report);
            }
        }

        public virtual void PrintingOnePage(T1 report)
        {
            Export(PrepareReport(report), report.DeviceInfo);
            Print(report.PrinterName, report.IsStricker);
        }


        protected virtual LocalReport PrepareReport(T1 printJobInfo)
        {
            string currentApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.LocalReport.ReportPath = currentApplicationPath + printJobInfo.ReportPath;

            LocalReport report = reportViewer.LocalReport;
            string deviceInfo = string.Empty;
            if (printJobInfo.Type == LabelPrintType.BarcodeSticker)
            {
                report.DataSources.Add(new ReportDataSource("DataSet", printJobInfo.LabelPrintConditionData.OfType<StickerBarcodeData>().ToList()));
            }
            else if (printJobInfo.Type == LabelPrintType.ShelfLabel)
            {
                report.DataSources.Add(new ReportDataSource("DataSet", printJobInfo.LabelPrintConditionData.OfType<ShelfLabelData>().ToList()));
            }
            else if (printJobInfo.Type == LabelPrintType.PromotionShelfLabel)
            {
                report.DataSources.Add(new ReportDataSource("DataSet", printJobInfo.LabelPrintConditionData.OfType<PromotionLabelData>().ToList()));
            }

            try
            {
                IList<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("Type", "CODE128"));
                report.SetParameters(parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return report;
            //reportViewer.LocalReport.ReportPath = string.Format("{0}{1}{2}", AppDomain.CurrentDomain.BaseDirectory, labelType.ReportPath, labelType.ReportFileName);

            //return new PrintJobInfo() { ReportPath = printJobInfo.ReportPath + "/" + printJobInfo.ReportFileName, DeviceInfo = deviceInfo, LabelPrintConditionData = list };

            //Export(report, deviceInfo);
            //Print(labelType.PrinterName, type == LabelPrintType.BarcodeSticker ? true : false);
            //LabelPrintService labelprintService = new LabelPrintService();

            //labelprintService.Export(report, deviceInfo);
            //labelprintService.Print(labelType.PrinterName, type == LabelPrintType.BarcodeSticker ? true : false);
        }

        public void Print(string printerName, bool isSticker, PaperSize paperSizeCostomize = null, short printCopies = 1)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = printerName;

            if (isSticker)
            {
                if (paperSizeCostomize != null)
                    printDoc.DefaultPageSettings.PaperSize = paperSizeCostomize;
                else
                    printDoc.DefaultPageSettings.PaperSize = new PaperSize("PrintLabel", 550, 500);

                printDoc.DefaultPageSettings.PrinterSettings.Copies = printCopies;
            }
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.EndPrint += printDoc_EndPrint;
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        void printDoc_EndPrint(object sender, PrintEventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }
    //public class PrintJobInfoList
    //{
    //    PrintJobInfoList()
    //    {
    //        list = new List<PrintJobInfo>();
    //    }
    //    public IList<PrintJobInfo> list { get; set; }
    //}
    public class PrintJobInfo
    {
        //public LocalReport Report { get; set; }
        public string DeviceInfo { get; set; }
        public string PrinterName { get; set; }
        public bool IsStricker { get; set; }
        public string ReportPath { get; set; }
        // public string ReportName { get; set; }
        public LabelPrintType Type { get; set; }
        public IList<LabelPrintConditionData> LabelPrintConditionData { get; set; }
    }
}
