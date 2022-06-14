using Eagle;
using Eagle.Models;
using FluentAssertions;

namespace EagleTests;

[TestFixture]
public class EagleServiceShould
{
    EagleService _service;
    
    [SetUp]
    public void Setup() => _service = new EagleService();

    [Test]
    public async Task List_Root_Folders()
    {
        var rootFolders = (await _service.GetFoldersAsync()).Select(x=>new
        {
            Children = x.ChildFolders,
            Name = x.Name,
            Id = x.Id
        }).ToList();

        rootFolders.Count.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task List_details_of_a_folder()
    {
        Folder folder = (await _service.GetFoldersAsync()).First();

        folder.ChildFolders.Count.Should().BeGreaterThan(0);
        folder.NumberOfItems.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task Open_folder_in_eagle()
    {
        Folder folder = (await _service.GetFoldersAsync()).First();
        folder.OpenInEagle();
    }

    [Test]
    public async Task Modify_folder_color()
    {
        var id = (await _service.GetFoldersAsync()).First().Id;

        await _service.SetFolderColor(id, FolderColors.blue);
        
    }

    [Test]
    public async Task Modify_folder_name()
    {
        var id = (await _service.GetFoldersAsync()).First().Id;
       // await _service.SetFolderName(id,"TEST");
    }

    [Test]
    public async Task Draw_folder_tree()
    {
        var folders = (await _service.GetFoldersAsync()).ToList();
        string rootTree = "";
        foreach (var folder in folders)
            AppendFolder(ref rootTree,folder,0);
        
        Console.WriteLine(rootTree);
        rootTree.Should().NotBeEmpty();

    }

    [Test]
    public async Task List_Contents_of_a_Folder()
    {
        var id = (await _service.GetFoldersAsync()).First().Id;

        var files = (await _service.GetFilesInFolderAsync(id)).ToList();

        files.Count.Should().BeGreaterThan(0);
    }
    
    [Test]
    public async Task List_Contents_of_a_Folder_and_its_children()
    {
        var id = (await _service.GetFoldersAsync()).ToList()[1].Id;

        var files = (await _service.GetFilesInFolderRecursivelyAsync(id,CancellationToken.None)).ToList();

        files.Count.Should().BeGreaterThan(0);
    }


    [Test]
    public async Task List_Items_with_specific_tags()
    {
        var files = (await _service.GetFilesWithAnyOfTheTags(CancellationToken.None,"Ui", "Reference"));
        int i = 0;
        
    }


    [Test]
    public async Task Get_List_of_tags_in_library()
    {
        var tags = (await _service.GetAllTags(CancellationToken.None));
        int i = 0;
    }


    [Test]
    public async Task Search_with_a_keyword()
    {

        var results = (await _service.SearchAsync(CancellationToken.None, A.Search.WithKeyword("Potato")));

        int i = 0;

    }
    

    void AppendFolder(ref string str, Folder folder, int depth)
    {

        str += "\n";
        str += new string(' ', depth);
        str += $"- {folder.Name}";
        foreach (var childFolder in folder.ChildFolders) AppendFolder(ref str, childFolder, depth + 1);
    }


    [Test]
    public async Task Open_specific_library()
    {
        string path = @"D:\Eagle Libraries\Entertainment\Entertainment.library";
       await _service.OpenLibrary(path);
    }

    [Test]
    public async Task Get_list_of_recent_libraries()
    {

        var libraries = await _service.GetLibraries();
        int i = 0;
        
    }


    [Test]
    public async Task Get_Icon_For_File()
    {
        const string FILE = "L4BE32V9H80VB";
        var icon = await _service.GetThumbnailPathFor(FILE);

        int i = 0;
    }

    [Test]
    public async Task Goto_Library_By_Name()
    {
      //  await _service.GotoLibrary("Delame");
        await _service.GotoLibrary("Audio");
    }

    [Test]
    public async Task Return_Library_Summary()
    {
        var summary = await _service.GetLibrarySummary(CancellationToken.None);
        int i = 0;
    }
    
    
    

    
}