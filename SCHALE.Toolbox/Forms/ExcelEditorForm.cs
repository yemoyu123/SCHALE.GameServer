using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.FlatBuffers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SCHALE.GameServer.Services;
using Serilog;
using Serilog.Extensions.Logging;
using SCHALE.Common;
using SCHALE.Common.Crypto;
using SCHALE.Common.FlatData;
using Form = System.Windows.Forms.Form;

namespace SCHALE.Toolbox.Forms
{
    public partial class ExcelEditorForm : Form
    {
        private readonly ExcelTableService _tableService;
        private readonly ILogger<ExcelEditorForm> _logger;
        private string _excelDirectory;
        private IList? _currentExcelTable = null;

        public ExcelEditorForm()
        {
            InitializeComponent();

            var logFactory = new SerilogLoggerFactory(Log.Logger);
            _logger = logFactory.CreateLogger<ExcelEditorForm>();
            _tableService = new ExcelTableService(logFactory.CreateLogger<ExcelTableService>());

            _excelDirectory = Path.Join(Path.GetDirectoryName(AppContext.BaseDirectory), "Resources/excel");
            _logger.LogInformation("Set excel directory to {excelDir}", _excelDirectory);
        }

        private void ExcelEditorForm_Load(object sender, EventArgs e)
        {

        }

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            await ExcelTableService.LoadExcels(_excelDirectory);
            _logger.LogInformation("Excel downloaded");
            this.Enabled = true;
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            ReloadExcels();
        }

        private void tableListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItems = tableListView.SelectedItems;
            if (selectedItems.Count < 1) return;

            var type = selectedItems[0].Tag as Type;
            if (type == null) return;

            var fbPath = Path.Join(_excelDirectory, $"{type.Name.ToLower()}.fb");
            if (!File.Exists(fbPath))
            {
                _logger.LogError("Decrypted flatbuffer for {Type} not found", type);
                return;
            }

            var bytes = File.ReadAllBytes(fbPath);
            var inst = type.GetMethod($"GetRootAs{type.Name}", BindingFlags.Static | BindingFlags.Public,
                [typeof(ByteBuffer)])!.Invoke(null, [new ByteBuffer(bytes)]);
            var obj = type.GetMethod("UnPack", BindingFlags.Instance | BindingFlags.Public)!.Invoke(inst, null);
            var dataList =
                obj!.GetType().GetProperty("DataList", BindingFlags.Instance | BindingFlags.Public)!.GetMethod!.Invoke(obj, null)!;
            Type? itemType = null;
            foreach (var iType in dataList!.GetType().GetInterfaces())
            {
                if (iType.IsGenericType && iType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    itemType = iType.GetGenericArguments()[0];
                    break;
                }
            }
            if (itemType == null)
            {
                _logger.LogError("Unknown type error");
                return;
            }

            _currentExcelTable = dataList as IList;
            _logger.LogInformation("{Count} {TypeName} found", _currentExcelTable!.Count, itemType.Name);

            itemListView.Items.Clear();
            for (var i = 0; i < _currentExcelTable!.Count; i++)
            {
                var listItem = new ListViewItem($"{i}");
                listItem.Tag = _currentExcelTable[i];
                itemListView.Items.Add(listItem);
            }

            itemListView.Columns[0].Width = -1;
        }

        private void itemListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItems = itemListView.SelectedItems;
            if (selectedItems.Count < 1) return;

            var obj = selectedItems[0].Tag;
            if (obj == null) return;

            propGrid.SelectedObject = obj;
        }

        private void ReloadExcels()
        {
            var assembly = Assembly.GetAssembly(typeof(AcademyFavorScheduleExcelTable))!;
            var types = assembly.GetTypes().Where(
                t => t.IsAssignableTo(typeof(IFlatbufferObject)) && t.Name.EndsWith("ExcelTable"));
            var validTypes = new List<Type>();

            foreach (var t in types)
            {
                var filePath = Path.Join(_excelDirectory, $"{t.Name.ToLower()}.bytes");
                var fbPath = Path.Join(_excelDirectory, $"{t.Name.ToLower()}.fb");
                if (!File.Exists(filePath)) continue;
                if (File.Exists(fbPath))
                {
                    validTypes.Add(t);
                    continue;
                }

                var bytes = File.ReadAllBytes(filePath);
                TableEncryptionService.XOR(t.Name, bytes);

                // save decrypted
                File.WriteAllBytes(fbPath, bytes);
                _logger.LogInformation("Decrypted excel table {TableName}", t.Name);

                // save json
                var inst = t.GetMethod($"GetRootAs{t.Name}", BindingFlags.Static | BindingFlags.Public,
                    [typeof(ByteBuffer)])!.Invoke(null, [new ByteBuffer(bytes)]);
                var obj = t.GetMethod("UnPack", BindingFlags.Instance | BindingFlags.Public)!.Invoke(inst, null);
                var jsonPath = Path.Join(_excelDirectory, $"{t.Name.ToLower()}.json");
                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(obj, Formatting.Indented, new StringEnumConverter()));
            }

            tableListView.Items.Clear();
            itemListView.Items.Clear();

            foreach (var t in validTypes)
            {
                var item = new ListViewItem(t.Name);
                item.Tag = t;
                tableListView.Items.Add(item);
            }
            tableListView.Columns[0].Width = -1;

        }
    }
}
