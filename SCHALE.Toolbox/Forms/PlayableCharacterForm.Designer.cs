namespace SCHALE.Toolbox.Forms
{
    partial class PlayableCharacterForm
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
            loadButton = new Button();
            splitContainer1 = new SplitContainer();
            chListView = new ListView();
            columnHeader1 = new ColumnHeader();
            chPropertyGrid = new PropertyGrid();
            exportButton = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // loadButton
            // 
            loadButton.Location = new Point(3, 3);
            loadButton.Name = "loadButton";
            loadButton.Size = new Size(280, 59);
            loadButton.TabIndex = 0;
            loadButton.Text = "Load from excel CharacterExcelTable";
            loadButton.UseVisualStyleBackColor = true;
            loadButton.Click += loadButton_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(12, 77);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(chListView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(chPropertyGrid);
            splitContainer1.Size = new Size(1086, 901);
            splitContainer1.SplitterDistance = 362;
            splitContainer1.TabIndex = 1;
            // 
            // chListView
            // 
            chListView.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            chListView.Dock = DockStyle.Fill;
            chListView.HeaderStyle = ColumnHeaderStyle.None;
            chListView.Location = new Point(0, 0);
            chListView.MultiSelect = false;
            chListView.Name = "chListView";
            chListView.Size = new Size(362, 901);
            chListView.TabIndex = 0;
            chListView.UseCompatibleStateImageBehavior = false;
            chListView.View = View.Details;
            chListView.SelectedIndexChanged += chListView_SelectedIndexChanged;
            // 
            // chPropertyGrid
            // 
            chPropertyGrid.Dock = DockStyle.Fill;
            chPropertyGrid.Location = new Point(0, 0);
            chPropertyGrid.Name = "chPropertyGrid";
            chPropertyGrid.Size = new Size(720, 901);
            chPropertyGrid.TabIndex = 0;
            // 
            // exportButton
            // 
            exportButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            exportButton.Location = new Point(289, 3);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(280, 59);
            exportButton.TabIndex = 2;
            exportButton.Text = "Export";
            exportButton.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(loadButton);
            flowLayoutPanel1.Controls.Add(exportButton);
            flowLayoutPanel1.Location = new Point(12, 5);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1086, 66);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // PlayableCharacterForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1110, 990);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(splitContainer1);
            Name = "PlayableCharacterForm";
            Text = "PlayableCharacter";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button loadButton;
        private SplitContainer splitContainer1;
        private ListView chListView;
        private ColumnHeader columnHeader1;
        private PropertyGrid chPropertyGrid;
        private Button exportButton;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}