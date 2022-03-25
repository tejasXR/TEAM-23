using System.IO;
using System.IO.Compression;
using System.Linq;

public class ZipAndSend {
    public void Zip(string toFilename, params string[] fromFilenames) {
        using var stream = new FileStream(toFilename, FileMode.Create);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
        C
        archive.CreateEntryFromFile()
    }
}