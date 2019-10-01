using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestTaskDirectory.Api.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace TestTaskDirectory.Api.Test
{
    public class DirectoryControllerTest : BaseTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        private readonly HttpClient client;

        private readonly string testFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles");

        public DirectoryControllerTest(WebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper) : base(factory)
        {
            this.testOutputHelper = testOutputHelper;
            client = Factory.CreateClient();
        }

        /// <summary>
        /// Tests <see cref="DirectoryController.GetDirectoryFiles"/> 
        /// </summary>
        [Fact]
        public async Task OrderedFiles()
        {
            var filesRequest = new FilesRequest
            {
                Directory = testFilesDirectory,
                OrderFilesBy = FileProperty.Name
            };

            var result = await client.PostAsync("directory/files", TestExtensions.SerializeRequest(filesRequest));
            var files = await TestExtensions.DeserializeResult<List<string>>(result);

            // check correct files number is returned
            files.Count.Should().Be(6);

            // check files ordered by name correctly
            new FileInfo(files.First()).Name.Should().Be("file1.cs");

            foreach (var file in files)
            {
                File.Exists(file).Should().BeTrue();

                testOutputHelper.WriteLine(file);
            }
        }

        /// <summary>
        /// Tests <see cref="DirectoryController.GetGroupedFiles"/> 
        /// </summary>
        [Fact]
        public async Task GroupedFiles()
        {
            var filesRequest = new FilesGroupingRequest
            {
                Directory = testFilesDirectory,
                OrderFilesBy = FileProperty.Name,
                GroupFilesBy = FileProperty.Extension
            };

            var result = await client.PostAsync("directory/grouped-files", TestExtensions.SerializeRequest(filesRequest));
            var groups = await TestExtensions.DeserializeResult<List<GroupedFilesResult>>(result);

            // check correct groups number is returned
            groups.Count.Should().Be(3);

            var keys = groups.Select(x => x.Key).ToList();
            keys.Should().Contain(".dll");
            keys.Should().Contain(".cs");
            keys.Should().Contain(".docx");
        }

        /// <summary>
        /// Tests <see cref="DirectoryController.FileTree"/> 
        /// </summary>
        [Fact]
        public async Task FileTree()
        {
            var filesRequest = new DirectoryRequest
            {
                Directory = testFilesDirectory,
            };

            var result = await client.PostAsync("directory/file-tree", TestExtensions.SerializeRequest(filesRequest));
            var treeString = await result.Content.ReadAsStringAsync();

            testOutputHelper.WriteLine(treeString);
        }

        /// <summary>
        /// Tests <see cref="DirectoryController.FilesOnDepth"/> 
        /// </summary>
        [Fact]
        public async Task FilesOnDepth()
        {
            var filesOnDepth2 = await GetFilesOnDepth(2);
            filesOnDepth2.Count.Should().Be(1);
            filesOnDepth2.Should().Contain("file1.docx");

            var filesOnDepth3 = await GetFilesOnDepth(3);
            filesOnDepth3.Count.Should().Be(2);
            filesOnDepth3.Should().Contain("file1.dll");
            filesOnDepth3.Should().Contain("file2.dll");

            var filesOnDepth5 = await GetFilesOnDepth(5);
            filesOnDepth5.Count.Should().Be(3);
            filesOnDepth5.Should().Contain("file1.cs");
            filesOnDepth5.Should().Contain("file2.cs");
            filesOnDepth5.Should().Contain("file3.cs");
        }

        private async Task<List<string>> GetFilesOnDepth(int depth)
        {
            var filesOnDepthRequest = new FilesOnDepthRequest
            {
                Directory = testFilesDirectory,
                Depth = depth
            };

            var result =
                await client.PostAsync("directory/files-on-depth", TestExtensions.SerializeRequest(filesOnDepthRequest));
            var files = await TestExtensions.DeserializeResult<List<string>>(result);

            testOutputHelper.WriteLine($"\nFiles on depth {depth}");

            foreach (var file in files)
            {
                File.Exists(file).Should().BeTrue();

                testOutputHelper.WriteLine(file);
            }

            return files.Select(f => new FileInfo(f).Name).ToList();
        }
    }
}
