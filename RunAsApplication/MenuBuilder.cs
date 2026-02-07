using RunAsApplication.Models;

namespace RunAsApplication;

public class MenuBuilder
{
    private readonly AppLaucnher _appLaucnher;

    internal MenuBuilder(AppLaucnher appLaucnher)
    {
        _appLaucnher = appLaucnher;
    }

    internal void BuildMenu(RootConfig config, ContextMenuStrip contextMenuStrip)
    {
        contextMenuStrip.Items.Clear();

        foreach (var group in config)
        {
            string groupName = group.Key;
            var applicationsConfigs = group.Value;

            // Если группа одна — не делаем вложенное меню
            if (config.Count == 1)
            {
                foreach (var applicationConfig in applicationsConfigs)
                    AddAppItem(contextMenuStrip.Items, applicationConfig);

                return;
            }

            // Создаем выпадающее меню для группы
            var groupItem = new ToolStripMenuItem(groupName);
            foreach (var app in applicationsConfigs)
                AddAppItem(groupItem.DropDownItems, app);

            contextMenuStrip.Items.Add(groupItem);
        }

        contextMenuStrip.Items.Add(new ToolStripSeparator());
        contextMenuStrip.Items.Add("Выход", null, (s, e) => Application.Exit());
    }

    private void AddAppItem(ToolStripItemCollection collection, ApplicationConfig applicationConfig)
    {
        // В качестве текста берем имя файла или само значение
        string label = Path.GetFileNameWithoutExtension(applicationConfig.FilePath);
        var item = new ToolStripMenuItem(label);

        item.Click += (s, e) => _appLaucnher.Execute(applicationConfig);
        collection.Add(item);
    }
}
