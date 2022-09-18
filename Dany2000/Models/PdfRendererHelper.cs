using PdfiumViewer;
using PdfiumViewer.Core;
using PdfiumViewer.Enums;
using System;
using System.Windows;

namespace Dany2000.Models
{
    public static class PdfRendererHelper
    {
        public static readonly DependencyProperty PdfFileProperty =
            DependencyProperty.RegisterAttached("PdfFile", typeof(string), typeof(PdfRendererHelper), new UIPropertyMetadata(null, PdfFilePropertyChanged));

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

                renderer.PagesDisplayMode = PdfViewerPagesDisplayMode.SinglePageMode;
                renderer.OpenPdf(pdfFile);
                renderer.PagesDisplayMode = renderer.Document.PageCount > 1 ? PdfViewerPagesDisplayMode.BookMode : PdfViewerPagesDisplayMode.SinglePageMode;
            }
        }

        //public static readonly DependencyProperty PdfDisplayModeProperty =
        //    DependencyProperty.RegisterAttached("PdfDisplayMode", typeof(string), typeof(PdfRendererUtility), new UIPropertyMetadata(null, PdfDisplayModePropertyChanged));

        //public static PdfViewerPagesDisplayMode GetPdfDisplayMode(DependencyObject obj)
        //{
        //    return (PdfViewerPagesDisplayMode)obj.GetValue(PdfDisplayModeProperty);
        //}

        //public static void SetPdfDisplayMode(DependencyObject obj, PdfViewerPagesDisplayMode value)
        //{
        //    obj.SetValue(PdfDisplayModeProperty, value);
        //}

        //public static void PdfDisplayModePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    PdfRenderer renderer = o as PdfRenderer;
        //    if (renderer != null)
        //    {
        //        var displayMode = (PdfViewerPagesDisplayMode)Enum.Parse(typeof(PdfViewerPagesDisplayMode), (string)e.NewValue);
        //        renderer.PagesDisplayMode = PdfViewerPagesDisplayMode.SinglePageMode;
        //    }
        //}
    }
}
