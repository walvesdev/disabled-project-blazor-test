using DinkToPdf;
using DinkToPdf.Contracts;

using Sindvamb.Domain.Entities;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Security;

using System.Security.Cryptography.X509Certificates;

namespace Sindvamb.Files.Pdf;

public class PdfGenerator : IDisposable
{
    public PdfDocument Pdf { get; set; }
    private IConverter PdfConverter { get; }

    public ISindvambRepository<FileControl, Guid> FileControlRepository { get; set; }

    public PdfGenerator(IConverter pdfConverter, ISindvambRepository<FileControl, Guid> fileControlRepository,
        PdfDocument pdfDocument)
    {
        PdfConverter = pdfConverter;
        FileControlRepository = fileControlRepository;
        Pdf = pdfDocument;
    }

    [STAThread]
    public async Task<byte[]> Create(string htmlContent, string tittle = "Documento", bool hasSign = false)
    {

        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings =
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                DocumentTitle = tittle,
                PaperSize = PaperKind.A4,
                ImageQuality = 200,
                DPI = 300,
                ImageDPI = 1000,
                UseCompression = true,
                Margins = new MarginSettings
                {
                    Top = 0,
                    Bottom = 0,
                    Unit = Unit.Centimeters,
                    Left = 0,
                    Right = 0
                }
            },
            Objects =
            {
                new ObjectSettings()
                {
                    HtmlContent = htmlContent,
                    IncludeInOutline = true,
                    WebSettings =
                    {
                        DefaultEncoding = "utf-8",
                        Background = true,
                        LoadImages = true,
                        EnableJavascript = true
                    }
                }
            }
        };

        var result = PdfConverter.Convert(pdf);

        if (hasSign)
        {
            result = await Sign(result);
        }

        return result;
    }

    public async Task<byte[]> Sign(byte[] pdf)
    {
        string pfxFilePath = Path.Combine(AppServices.GetRootDirectory(), "sindvamb.pfx");
        string pfxPassword = "33841172";

        // Create a PdfDocument object
        Pdf = new PdfDocument(pdf);
        Pdf.Metadata = new Spire.Pdf.Interchange.Metadata.PdfXmpMetadata();

        // Load a sample PDF file
        // Pdf.LoadFromBytes(pdf);

        // Load the certificate 
        PdfCertificate cert = new PdfCertificate(pfxFilePath, pfxPassword);

        // Create a PdfSignature object 
        PdfSignature signature = new PdfSignature(Pdf, Pdf.Pages[^1], cert, "Sindvamb");
        

        // Alterar o tamanho da fonte da assinatura
        signature.SignDetailsFont = new PdfFont(PdfFontFamily.Helvetica, 10f); 

        // Set the document permission to forbid changes but allow form fill
        signature.DocumentPermissions = PdfCertificationFlags.ForbidChanges;// | PdfCertificationFlags.AllowFormFill;

        // Configurar a aparência da assinatura
        signature.Name = $"Documento assinado eletronicamente por: \nSINDICATO DO COMÉRCIO DE VENDEDORES AMBULANTES.\n\n";

        // Define a posição da assinatura no PDF
        // X e Y → Coordenadas do canto superior esquerdo.
        // Width e Height → Largura e altura do retângulo.
        signature.Bounds = new System.Drawing.RectangleF(150, 470, 350, 80);

        // signature.ContactInfoLabel = "Contato:";
        // signature.ContactInfo = " contato@sindvamb.com.br";
        signature.LocationInfoLabel = "Local:";
        signature.LocationInfo = $" Brasília {DateTime.Now.ToShortDateString()}, às {DateTime.Now.ToShortTimeString()} - Distrito Federal";
        signature.ReasonLabel = "Motivo:";
        signature.Reason = $" Autenticação do documento: DO:{Guid.NewGuid().ToString().GetDigits()}";
        signature.Certificated = true;

        // Save to another PDF file 
        var ms = new MemoryStream();
        Pdf.SaveToStream(ms);

        return await Task.FromResult(ms.ToArray());
    }


    public void Dispose()
    {
        Pdf.Close();
    }
}