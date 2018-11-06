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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.firstTab = new System.Windows.Forms.TabPage();
            this.newFileNameTextBox = new System.Windows.Forms.TextBox();
            this.createNewFileButton = new System.Windows.Forms.Button();
            this.filesList = new System.Windows.Forms.ListView();
            this.tabs = new System.Windows.Forms.TabControl();
            this.firstTab.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // firstTab
            // 
            this.firstTab.Controls.Add(this.newFileNameTextBox);
            this.firstTab.Controls.Add(this.createNewFileButton);
            this.firstTab.Controls.Add(this.filesList);
            this.firstTab.Location = new System.Drawing.Point(4, 25);
            this.firstTab.Name = "firstTab";
            this.firstTab.Padding = new System.Windows.Forms.Padding(3);
            this.firstTab.Size = new System.Drawing.Size(735, 384);
            this.firstTab.TabIndex = 0;
            this.firstTab.Text = "K&K Reader";
            this.firstTab.UseVisualStyleBackColor = true;
            // 
            // newFileNameTextBox
            // 
            this.newFileNameTextBox.Location = new System.Drawing.Point(563, 19);
            this.newFileNameTextBox.Name = "newFileNameTextBox";
            this.newFileNameTextBox.Size = new System.Drawing.Size(148, 22);
            this.newFileNameTextBox.TabIndex = 6;
            this.newFileNameTextBox.Text = "NewFile";
            // 
            // createNewFileButton
            // 
            this.createNewFileButton.Location = new System.Drawing.Point(563, 47);
            this.createNewFileButton.Name = "createNewFileButton";
            this.createNewFileButton.Size = new System.Drawing.Size(148, 23);
            this.createNewFileButton.TabIndex = 4;
            this.createNewFileButton.Text = "Add New File";
            this.createNewFileButton.UseVisualStyleBackColor = true;
            this.createNewFileButton.Click += new System.EventHandler(this.createNewFileButton_Click);
            // 
            // filesList
            // 
            this.filesList.Location = new System.Drawing.Point(0, 0);
            this.filesList.Name = "filesList";
            this.filesList.Size = new System.Drawing.Size(542, 390);
            this.filesList.TabIndex = 3;
            this.filesList.UseCompatibleStateImageBehavior = false;
            this.filesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.filesList_Click);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.firstTab);
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(743, 413);
            this.tabs.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 413);
            this.Controls.Add(this.tabs);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "c# Rest Client";
            this.firstTab.ResumeLayout(false);
            this.firstTab.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage firstTab;
        private System.Windows.Forms.ListView filesList;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.Button createNewFileButton;
        private System.Windows.Forms.TextBox newFileNameTextBox;
    }
}

