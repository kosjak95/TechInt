using System.Windows.Forms;

namespace TechnikiInterentoweClient
{
    partial class Form1
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


            if (tabs != null)
            {
                TabControl.TabPageCollection pages = tabs.TabPages;
                if (pages.Count > 1)
                {
                    RestClient restClient = new RestClient();
                    rClient.endPoint = "http://localhost:8080/ReleaseFileCludge/";
                    //if (rClient.makePostRequest(new { file_name = newFileNameTextBox.Text }))
                    foreach (TabPage page in pages)
                    {

                        if (page.AccessibilityObject.Name.Equals("KK Reader") || page.AccessibilityObject.Name.Equals("    +"))
                            continue;

                        if (page.Controls.Count <= 1)
                            continue;

                        string fileName = page.AccessibilityObject.Name;
                        rClient.makePostRequest(new { fileName });

                    }
                } 
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.firstTab = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.fileIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateTsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.versionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isEditedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.EditorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabs = new System.Windows.Forms.TabControl();
            this.addTab = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.firstTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileDataBindingSource)).BeginInit();
            this.tabs.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // firstTab
            // 
            this.firstTab.Controls.Add(this.dataGridView1);
            this.firstTab.Location = new System.Drawing.Point(4, 25);
            this.firstTab.Margin = new System.Windows.Forms.Padding(2);
            this.firstTab.Name = "firstTab";
            this.firstTab.Padding = new System.Windows.Forms.Padding(2);
            this.firstTab.Size = new System.Drawing.Size(917, 385);
            this.firstTab.TabIndex = 0;
            this.firstTab.Text = "K&K Reader";
            this.firstTab.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileIdDataGridViewTextBoxColumn,
            this.lastUpdateTsDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.versionDataGridViewTextBoxColumn,
            this.isEditedDataGridViewCheckBoxColumn,
            this.EditorName});
            this.dataGridView1.DataSource = this.fileDataBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Size = new System.Drawing.Size(913, 381);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // fileIdDataGridViewTextBoxColumn
            // 
            this.fileIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fileIdDataGridViewTextBoxColumn.DataPropertyName = "FileId";
            this.fileIdDataGridViewTextBoxColumn.HeaderText = "FileId";
            this.fileIdDataGridViewTextBoxColumn.MinimumWidth = 15;
            this.fileIdDataGridViewTextBoxColumn.Name = "fileIdDataGridViewTextBoxColumn";
            this.fileIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdateTsDataGridViewTextBoxColumn
            // 
            this.lastUpdateTsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lastUpdateTsDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateTs";
            this.lastUpdateTsDataGridViewTextBoxColumn.HeaderText = "LastUpdateTs";
            this.lastUpdateTsDataGridViewTextBoxColumn.MinimumWidth = 40;
            this.lastUpdateTsDataGridViewTextBoxColumn.Name = "lastUpdateTsDataGridViewTextBoxColumn";
            this.lastUpdateTsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.MinimumWidth = 30;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // versionDataGridViewTextBoxColumn
            // 
            this.versionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.versionDataGridViewTextBoxColumn.DataPropertyName = "Version";
            this.versionDataGridViewTextBoxColumn.HeaderText = "Version";
            this.versionDataGridViewTextBoxColumn.MinimumWidth = 15;
            this.versionDataGridViewTextBoxColumn.Name = "versionDataGridViewTextBoxColumn";
            this.versionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isEditedDataGridViewCheckBoxColumn
            // 
            this.isEditedDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.isEditedDataGridViewCheckBoxColumn.DataPropertyName = "IsEdited";
            this.isEditedDataGridViewCheckBoxColumn.HeaderText = "IsEdited";
            this.isEditedDataGridViewCheckBoxColumn.MinimumWidth = 15;
            this.isEditedDataGridViewCheckBoxColumn.Name = "isEditedDataGridViewCheckBoxColumn";
            this.isEditedDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // EditorName
            // 
            this.EditorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EditorName.DataPropertyName = "EditorName";
            this.EditorName.HeaderText = "EditorName";
            this.EditorName.MinimumWidth = 30;
            this.EditorName.Name = "EditorName";
            this.EditorName.ReadOnly = true;
            // 
            // fileDataBindingSource
            // 
            this.fileDataBindingSource.DataSource = typeof(TechnikiInterentoweCommon.FileData);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.firstTab);
            this.tabs.Controls.Add(this.addTab);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.ItemSize = new System.Drawing.Size(20, 21);
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Margin = new System.Windows.Forms.Padding(2);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(925, 414);
            this.tabs.TabIndex = 5;
            this.tabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabs_Selecting);
            this.tabs.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabs_MouseClick);
            this.tabs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tabs_MouseDoubleClick);
            // 
            // addTab
            // 
            this.addTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.addTab.ForeColor = System.Drawing.Color.Lime;
            this.addTab.Location = new System.Drawing.Point(4, 25);
            this.addTab.Margin = new System.Windows.Forms.Padding(0);
            this.addTab.Name = "addTab";
            this.addTab.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.addTab.Size = new System.Drawing.Size(917, 385);
            this.addTab.TabIndex = 1;
            this.addTab.Text = "    +";
            this.addTab.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabs);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(925, 414);
            this.panel1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 414);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "c# Rest Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.firstTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileDataBindingSource)).EndInit();
            this.tabs.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage firstTab;
        private System.Windows.Forms.TabControl tabs;
        private DataGridView dataGridView1;
        private BindingSource bindingSource;
        private BindingSource fileDataBindingSource;
        private TabPage addTab;
        private Panel panel1;
        private DataGridViewTextBoxColumn fileIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lastUpdateTsDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn versionDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn isEditedDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn EditorName;
    }
}

