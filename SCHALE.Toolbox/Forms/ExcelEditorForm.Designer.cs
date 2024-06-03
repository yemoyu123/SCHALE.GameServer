namespace SCHALE.Toolbox.Forms
{
    partial class ExcelEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            downloadButton = new Button();
            reloadButton = new Button();
            propGrid = new PropertyGrid();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            tableListView = new ListView();
            columnHeader1 = new ColumnHeader();
            itemListView = new ListView();
            columnHeader2 = new ColumnHeader();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // downloadButton
            // 
            downloadButton.Location = new Point(12, 12);
            downloadButton.Name = "downloadButton";
            downloadButton.Size = new Size(188, 44);
            downloadButton.TabIndex = 0;
            downloadButton.Text = "Download Excels";
            downloadButton.UseVisualStyleBackColor = true;
            downloadButton.Click += downloadButton_Click;
            // 
            // reloadButton
            // 
            reloadButton.Location = new Point(206, 12);
            reloadButton.Name = "reloadButton";
            reloadButton.Size = new Size(188, 44);
            reloadButton.TabIndex = 1;
            reloadButton.Text = "Reload Excels";
            reloadButton.UseVisualStyleBackColor = true;
            reloadButton.Click += reloadButton_Click;
            // 
            // propGrid
            // 
            propGrid.Dock = DockStyle.Fill;
            propGrid.Location = new Point(0, 0);
            propGrid.Name = "propGrid";
            propGrid.Size = new Size(695, 831);
            propGrid.TabIndex = 4;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(12, 62);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(propGrid);
            splitContainer1.Size = new Size(1048, 831);
            splitContainer1.SplitterDistance = 349;
            splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(tableListView);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(itemListView);
            splitContainer2.Size = new Size(349, 831);
            splitContainer2.SplitterDistance = 373;
            splitContainer2.TabIndex = 0;
            // 
            // tableListView
            // 
            tableListView.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            tableListView.Dock = DockStyle.Fill;
            tableListView.HeaderStyle = ColumnHeaderStyle.None;
            tableListView.Location = new Point(0, 0);
            tableListView.MultiSelect = false;
            tableListView.Name = "tableListView";
            tableListView.Size = new Size(349, 373);
            tableListView.TabIndex = 0;
            tableListView.UseCompatibleStateImageBehavior = false;
            tableListView.View = View.Details;
            tableListView.SelectedIndexChanged += tableListView_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "";
            columnHeader1.Width = 240;
            // 
            // itemListView
            // 
            itemListView.Columns.AddRange(new ColumnHeader[] { columnHeader2 });
            itemListView.Dock = DockStyle.Fill;
            itemListView.HeaderStyle = ColumnHeaderStyle.None;
            itemListView.Location = new Point(0, 0);
            itemListView.MultiSelect = false;
            itemListView.Name = "itemListView";
            itemListView.Size = new Size(349, 454);
            itemListView.TabIndex = 0;
            itemListView.UseCompatibleStateImageBehavior = false;
            itemListView.View = View.Details;
            itemListView.SelectedIndexChanged += itemListView_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Enabled = false;
            button1.Location = new Point(872, 12);
            button1.Name = "button1";
            button1.Size = new Size(188, 44);
            button1.TabIndex = 6;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            // 
            // ExcelEditorForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1072, 905);
            Controls.Add(button1);
            Controls.Add(splitContainer1);
            Controls.Add(reloadButton);
            Controls.Add(downloadButton);
            Name = "ExcelEditorForm";
            Text = "ExcelEditor";
            Load += ExcelEditorForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button downloadButton;
        private Button reloadButton;
        private PropertyGrid propGrid;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private ListView tableListView;
        private ListView itemListView;
        private Button button1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
    }
}