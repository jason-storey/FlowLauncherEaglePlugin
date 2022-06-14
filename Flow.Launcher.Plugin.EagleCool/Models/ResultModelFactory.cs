using Eagle;

namespace Flow.Launcher.Plugin.EagleCool
{
#pragma warning disable CS1591
    public class ResultModelFactory
    {
        public static Result ToResult(Folder x)
        {
            var r = new Result
            {
                Title = x.Name,
                Action = _ =>
                {
                    x.OpenInEagle();
                    return true;
                },
                IcoPath = "Images/folder.png"
            };
            
            FormatFolderSubtitleLine(x, r);
            return r;
        }
        
        static void FormatFolderSubtitleLine(Folder x, Result r)
        {
            if (string.IsNullOrWhiteSpace(x.Description))
            {
                if (x.NumberOfItemsIncludingChildren <= 0)
                    r.SubTitle = "[Empty]";
                else
                {
                    r.SubTitle = $"{x.NumberOfItemsIncludingChildren} Items";
                    if (x.ChildFolders.Count > 1) r.SubTitle += $" in {x.ChildFolders.Count} Folders.";
                }
            }
            else
            {
                if (x.NumberOfItemsIncludingChildren > 0)
                    r.SubTitle = $"{x.NumberOfItemsIncludingChildren} Items - ";
                r.SubTitle += x.Description;
            }
        }

    }
}