using System.IO;
using System.Linq;
using System.Text;
using DataIngestion.TestAssignment.DataImporter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DataIngestion.UnitTests
{
    public class FileImporterTests
    {
        [Fact]
        public void AlwaysTrue()
        {
            // This helps troubleshooting testing
            Assert.True(true);
        }

        [Fact]
        public void ImportArtists_FileNotFound_ReturnEmptyList()
        {
            // Arrange
            var loggerStub = new Mock<ILogger<FileImporter>>();
            var configurationStub = new Mock<IConfiguration>();
            var readerStub = new Mock<IReader>();

            readerStub.Setup(x => x
                .GetReader(It.IsAny<string>()))
                .Throws(new IOException("File not found"));

            var sut = new FileImporter(configurationStub.Object, loggerStub.Object, readerStub.Object);

            // Act
            var result = sut.ImportArtists();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void ImportArtists_FileIsEmpty_ReturnEmptyList()
        {
            // Arrange
            string fakeFileContents = "";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            MemoryStream fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var loggerStub = new Mock<ILogger<FileImporter>>();
            var configurationStub = new Mock<IConfiguration>();
            var readerStub = new Mock<IReader>();

            readerStub.Setup(x => x
                .GetReader(It.IsAny<string>()))
                .Returns(() => new StreamReader(fakeMemoryStream));

            var sut = new FileImporter(configurationStub.Object, loggerStub.Object, readerStub.Object);

            // Act
            var result = sut.ImportArtists();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void ImportArtists_Success_ReturnArtistList()
        {
            // Arrange
            string fakeFileContents =
            "1\u0001157458\u0001Daniel Johnston\u00011\u0001https://lnk.fire.com/artist/daniel-johnston/id157458?uo=5\u00011\u0002\n" +
            "1\u0001754259\u0001Mariss Jansons\u00011\u0001https://lnk.fire.com/artist/mariss-jansons/id754259?uo=5\u00011\u0002\n" +
            "1\u0001875728\u0001Clay Blaker\u00011\u0001https://lnk.fire.com/artist/clay-blaker/id875728?uo=5\u00011\u0002\n";

            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            MemoryStream fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var loggerStub = new Mock<ILogger<FileImporter>>();
            var configurationStub = new Mock<IConfiguration>();
            var readerStub = new Mock<IReader>();

            readerStub.Setup(x => x
                .GetReader(It.IsAny<string>()))
                .Returns(() => new StreamReader(fakeMemoryStream));

            var sut = new FileImporter(configurationStub.Object, loggerStub.Object, readerStub.Object);

            // Act
            var result = sut.ImportArtists();

            // Assert
            Assert.True(result.Any());
        }
    }
}