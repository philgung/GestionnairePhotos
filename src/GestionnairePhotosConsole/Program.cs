// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;using System.Text;
using System.Text.Json;


// \\NASGungLeroy\Photos
// "\\NASGungLeroy\Photos\100NIKON\DSCN1860.JPG"
// connaitre la date de prise de la photos
// trier dans dossier et ranger


// Trouver doublons
//ScanPhotos(@"\\NASGungLeroy\Photos", "scan.json");
var doublons = ChargeScanPhotos("scan.json");
Console.WriteLine($"Scan photos : {doublons.Count}");

var doublonsGroupés = new Dictionary<byte[], string[]>();

var photoCourante = doublons.FirstOrDefault();

doublons.Remove(photoCourante.Key);

while (doublons.Any())
{
    var lesDoublons = doublons.Where(_ => _.Value.SequenceEqual(photoCourante.Value));
    if (lesDoublons.Any())
    {
        var liste = lesDoublons.Select(_ => _.Key).ToList();
        liste.Add(photoCourante.Key);
        doublonsGroupés.Add(photoCourante.Value, liste.ToArray());
    }

    photoCourante = doublons.FirstOrDefault();
    if (!photoCourante.Equals(default(KeyValuePair<string, byte[]>)))
    {
        doublons.Remove(photoCourante.Key);
    }
}

var sb = new StringBuilder();
sb.AppendLine("<html><head>" +
              "<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/css/bootstrap.min.css'>" +
              "</head><body>");
sb.AppendLine("<script>function toClipboard(textValue) { navigator.clipboard.writeText(textValue);}</script>" +
              $"<p>Il y a {doublonsGroupés.Count} doublons :</p>");

foreach (var doublon in doublonsGroupés)
{
    sb.AppendLine("<div class='row'>");
    foreach (var chemin in doublon.Value)
    {
        var command = $"\"rm {chemin}\"";
        var line = $"<div class='p-2 column'><a href='{chemin}'>{chemin}</a>&nbsp;" +
                                              $"<a href='#' onclick='toClipboard({command})'>Remove</a></div>";
        sb.AppendLine(line);
    }
    //Console.WriteLine(string.Join(',', doublon.Value));
    sb.AppendLine("</div>");
}

sb.AppendLine("</body></html>");
File.WriteAllText("index.html", sb.ToString());

Dictionary<string, byte[]> ChargeScanPhotos(string chemin)
{
    return JsonSerializer.Deserialize<Dictionary<string, byte[]>>(File.ReadAllText(chemin));
}

void ScanPhotos(string chemin, string destination)
{
    var photos = new Dictionary<string, byte[]>();
    var files = Directory.GetFiles(chemin, "*.jpg", SearchOption.AllDirectories);
//var files = Directory.GetFiles(@"../../../../GestionnairePhotosTests/Ressources", "*.png", SearchOption.AllDirectories);
    using var sha256 = SHA256.Create();

    foreach (var file in files)
    {
        using var input = File.OpenRead(file);
        photos.Add(file, sha256.ComputeHash(input));
    }

    File.WriteAllText(destination, JsonSerializer.Serialize(photos));

//17198 : jpg
    Console.WriteLine(files.Length);
}