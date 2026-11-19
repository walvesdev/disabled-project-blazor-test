using QRCoder;

using System.Drawing;
using System.Drawing.Imaging;

namespace Sindvamb.Files.QrCode;

public class QRCodeFactory
{
    public QRCodeGenerator QRCodeGenerator { get; set; }

    public QRCodeFactory(QRCodeGenerator qRCodeGenerator)
    {
        QRCodeGenerator = qRCodeGenerator;
    }

    public Stream GetQrcodeStream(string qrcodeText)
    {
        var qrCodeData = QRCodeGenerator.CreateQrCode(qrcodeText, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);

        var qrCodeAsPngByteArr = qrCode.GetGraphic(20);
        using var ms = new MemoryStream(qrCodeAsPngByteArr);
        var qrCodeImage = new Bitmap(ms);
        qrCodeImage.Save(ms, ImageFormat.Png);

        return ms;
    }

    public string GetQrcodeBase64(string qrcodeText)
    {
        if (string.IsNullOrWhiteSpace(qrcodeText)) return string.Empty;

        var qrCodeData = QRCodeGenerator.CreateQrCode(qrcodeText, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);

        var qrCodeAsPngByteArr = qrCode.GetGraphic(20);
        using var ms = new MemoryStream(qrCodeAsPngByteArr);
        var qrCodeImage = new Bitmap(ms);
        qrCodeImage.Save(ms, ImageFormat.Png);

        return $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
    }

    public string GetQrcodToBase64String(string qrcodeText)
    {
        var qrCodeData = QRCodeGenerator.CreateQrCode(qrcodeText, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);

        var qrCodeAsPngByteArr = qrCode.GetGraphic(20);
        using var ms = new MemoryStream(qrCodeAsPngByteArr);
        var qrCodeImage = new Bitmap(ms);
        qrCodeImage.Save(ms, ImageFormat.Png);

        return Convert.ToBase64String(ms.ToArray());
    }

    public byte[] GetQrcodeByteArray(string qrcodeText)
    {
        var qrCodeData = QRCodeGenerator.CreateQrCode(qrcodeText, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);

        var qrCodeAsPngByteArr = qrCode.GetGraphic(20);
        using var ms = new MemoryStream(qrCodeAsPngByteArr);
        var qrCodeImage = new Bitmap(ms);
        qrCodeImage.Save(ms, ImageFormat.Png);

        return ms.ToArray(); ;
    }

    public byte[] GetQrcodeByFilePath(string filePath)
    {
        using var fs = new FileStream(filePath, FileMode.Open);
        using var ms = new MemoryStream();
        fs.CopyTo(ms);
        var image = new Bitmap(1, 1);
        image.Save(ms, ImageFormat.Png);
        return ms.ToArray();
    }
}