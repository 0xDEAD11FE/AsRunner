using System.Drawing;
using ConfigReader.Models;
using WinApiWrapper;

namespace AsRunner;

public class MenuBuilder : IDisposable
{
    private readonly AppLauncher _appLauncher;

    // Bitmap'ы иконок пунктов меню — это GDI-ресурсы; ToolStrip их не освобождает,
    // поэтому держим ссылки и диспозим сами при пересборке меню и на выходе.
    private readonly List<Bitmap> _itemImages = new();

    internal MenuBuilder(AppLauncher appLauncher)
    {
        _appLauncher = appLauncher;
    }

    internal void BuildMenu(RootConfig config, ContextMenuStrip contextMenuStrip)
    {
        contextMenuStrip.Items.Clear();
        DisposeItemImages();

        foreach (var group in config)
        {
            string groupName = group.Key;
            var applicationsConfigs = group.Value;

            // Если группа одна — не делаем вложенное меню
            if (config.Count == 1)
            {
                foreach (var applicationConfig in applicationsConfigs)
                    AddAppItem(contextMenuStrip.Items, applicationConfig);
            }
            else
            {
                // Создаем выпадающее меню для группы
                var groupItem = new ToolStripMenuItem(groupName);
                foreach (var app in applicationsConfigs)
                    AddAppItem(groupItem.DropDownItems, app);

                contextMenuStrip.Items.Add(groupItem);
            }
        }

        contextMenuStrip.Items.Add(new ToolStripSeparator());
        contextMenuStrip.Items.Add("Выход", null, (s, e) => Application.Exit());
    }

    private void AddAppItem(ToolStripItemCollection collection, ApplicationConfig applicationConfig)
    {
        string label = Path.GetFileNameWithoutExtension(applicationConfig.FilePath);
        var item = new ToolStripMenuItem(label)
        {
            Image = TryGetIcon(applicationConfig.FilePath),
            ImageScaling = ToolStripItemImageScaling.SizeToFit
        };

        item.Click += (s, e) => _appLauncher.Execute(applicationConfig);
        collection.Add(item);
    }

    private Bitmap? TryGetIcon(string filePath)
    {
        IntPtr hIcon = IconExtractor.ExtractSmallIcon(filePath);
        if (hIcon == IntPtr.Zero)
            return null;

        try
        {
            // Icon.FromHandle не владеет хэндлом; ToBitmap() делает независимую копию.
            using var icon = Icon.FromHandle(hIcon);
            var bitmap = icon.ToBitmap();
            _itemImages.Add(bitmap);
            return bitmap;
        }
        finally
        {
            IconExtractor.DestroyIcon(hIcon);
        }
    }

    private void DisposeItemImages()
    {
        foreach (var image in _itemImages)
            image.Dispose();

        _itemImages.Clear();
    }

    public void Dispose()
    {
        DisposeItemImages();
        GC.SuppressFinalize(this);
    }
}
