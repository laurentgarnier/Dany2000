using PdfiumViewer;
using PdfiumViewer.Core;
using System.Windows;

namespace Dany2000.Models
{
    public static class PdfRendererUtility
    {
        public static readonly DependencyProperty PdfFileProperty =
            DependencyProperty.RegisterAttached("PdfFile", typeof(string), typeof(PdfRendererUtility), new UIPropertyMetadata(null, PdfFilePropertyChanged));

        public static PdfDocument GetPdfFile(DependencyObject obj)
        {
            return (PdfDocument)obj.GetValue(PdfFileProperty);
        }

        public static void SetPdfFile(DependencyObject obj, PdfDocument value)
        {
            obj.SetValue(PdfFileProperty, value);
        }

        public static void PdfFilePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PdfRenderer renderer = o as PdfRenderer;
            if (renderer != null)
            {
                var pdfFile = e.NewValue as string;
                renderer.OpenPdf(pdfFile);
            }
        }
    }
}
