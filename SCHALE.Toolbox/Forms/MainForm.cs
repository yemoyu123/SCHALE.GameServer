using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCHALE.Toolbox.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AddFormItem<ExcelEditorForm>("Excel Editor");
            AddFormItem<PlayableCharacterForm>("Playable Characters");
            new ExcelEditorForm().Show();
        }

        private void AddFormItem<T>(string description)
        {
            var item = new ListViewItem();
            item.Text = description;
            item.Tag = typeof(T);
            functionListView.Items.Add(item);
        }

        private void functionListView_DoubleClick(object sender, EventArgs e)
        {
            var items = functionListView.SelectedItems;
            if (items.Count < 1) return;

            var item = items[0];
            var formType = item.Tag as Type;
            if (formType == null) return;

            var form = Activator.CreateInstance(formType) as Form;
            if (form == null) return;

            form.Show();
        }
    }
}
