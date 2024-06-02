namespace SCHALE.Toolbox.Forms
{
    partial class MainForm
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
            functionListView = new ListView();
            SuspendLayout();
            // 
            // functionListView
            // 
            functionListView.Dock = DockStyle.Fill;
            functionListView.Location = new Point(0, 0);
            functionListView.Name = "functionListView";
            functionListView.Size = new Size(889, 751);
            functionListView.TabIndex = 0;
            functionListView.UseCompatibleStateImageBehavior = false;
            functionListView.View = View.List;
            functionListView.DoubleClick += functionListView_DoubleClick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(889, 751);
            Controls.Add(functionListView);
            Name = "MainForm";
            Text = "SCHALE Toolbox";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListView functionListView;
    }
}