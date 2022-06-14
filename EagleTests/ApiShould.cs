using Eagle;
using Eagle.Models;

namespace EagleTests;

public class ApiShould
{
    private Eagle.Api _api;

    [SetUp]
    public void Setup() => _api = new Api();

    [Test]
    public async Task List_Folders()
    {
        var folders = await _api.GetFolders(CancellationToken.None);
        int i = 0;
    }


    [Test]
    public async Task List_Search()
    {
        var req = new Search
        {
            Keyword = ""
        };
        var results = await _api.Search(req, CancellationToken.None);

        int i = 0;

    }

    [Test]
    public async Task Get_Thumbnail()
    {
        string specific = "L4CLNA6VSMNDM";
        var thumb = await _api.GetThumbnailFor(specific, CancellationToken.None);
        int i = 0;
    }


    [Test]
    public async Task List_Libraries()
    {
        var libs = await _api.GetLibraries(CancellationToken.None);

        int index = 2;

        await _api.OpenLibrary(libs[index], CancellationToken.None);
        
        
        int i = 0;
    }


    [Test]
    public async Task Get_Library_Info()
    {
        var response = await _api.GetLibraryInfo(CancellationToken.None);
        int i = 0;
    }
    
    
    

}