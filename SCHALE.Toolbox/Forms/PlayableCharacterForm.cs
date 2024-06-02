using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SCHALE.Common.Extensions;
using SCHALE.Common.FlatData;
using SCHALE.GameServer.Services;
using SCHALE.Toolbox.Models.SchaleDB;
using Serilog;
using Serilog.Extensions.Logging;
using Form = System.Windows.Forms.Form;

namespace SCHALE.Toolbox.Forms
{
    public partial class PlayableCharacterForm : Form
    {
        private readonly ExcelTableService _tableService;
        private readonly ILogger<PlayableCharacterForm> _logger;
        private string _excelDirectory;
        private ListViewGroup _viewGroupNormal;
        private ListViewGroup _viewGroupUnique;
        private ListViewGroup _viewGroupEvent;

        public PlayableCharacterForm()
        {
            InitializeComponent();
            var logFactory = new SerilogLoggerFactory(Log.Logger);
            _logger = logFactory.CreateLogger<PlayableCharacterForm>();
            _tableService = new ExcelTableService(logFactory.CreateLogger<ExcelTableService>());
            _viewGroupNormal = new ListViewGroup("Normal");
            _viewGroupUnique = new ListViewGroup("Unique");
            _viewGroupEvent = new ListViewGroup("Event");
            chListView.Groups.Add(_viewGroupNormal);
            chListView.Groups.Add(_viewGroupUnique);
            chListView.Groups.Add(_viewGroupEvent);

            _excelDirectory = Path.Join(Path.GetDirectoryName(AppContext.BaseDirectory), "Resources/excel");
            _logger.LogInformation("Set excel directory to {excelDir}", _excelDirectory);
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            chPropertyGrid.SelectedObject = null;
            chListView.Items.Clear();

            // load schaledb json
            List<StudentInfo>? studentList = null;
            try
            {
                studentList = JsonConvert.DeserializeObject<List<StudentInfo>>(File.ReadAllText("Misc/students.json", Encoding.UTF8));
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to read SchaleDB student info: {ErrorMessage}", ex.Message);
            }

            var table = _tableService.GetTable<CharacterExcelTable>();
            var chList = table.UnPack().DataList;
            chList = chList
                .Where(x =>
                    x is
                    {
                        IsPlayable: true,
                        IsPlayableCharacter: true,
                        IsDummy: false,
                        IsNPC: false,
                        ProductionStep: ProductionStep.Release,
                    })
                .ToList();
            var listItems = chList
                .Select(x =>
                {
                    var studentName = studentList?.FirstOrDefault(info => info.Id == x.Id)?.Name ?? "(Unknown)";
                    return new ListViewItem($"{x.Id} {studentName}", x.GetStudentType() switch
                    {
                        StudentType.Unique => _viewGroupUnique,
                        StudentType.Event => _viewGroupEvent,
                        _ => _viewGroupNormal,
                    })
                    {
                        Tag = x
                    };
                }).ToArray();
            chListView.Items.AddRange(listItems);
            chListView.Columns[0].Width = -1;

            _logger.LogInformation("Loaded {count} items", listItems.Length);
            if (studentList?.Count > listItems.Length)
            {
                _logger.LogWarning("Student list has {Count} items, excel data may be outdated", studentList.Count);
            }
        }

        private void chListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItems = chListView.SelectedItems;
            if (selectedItems.Count < 1) return;

            var item = selectedItems[0].Tag as CharacterExcelT;
            if (item == null) return;

            chPropertyGrid.SelectedObject = item;
        }
    }
}
