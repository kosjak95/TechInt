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

            TabControl.TabPageCollection pages = tabs.TabPages;
            if (pages.Count > 1)
            {
                RestClient restClient = new RestClient();
                rClient.endPoint = "http://localhost:8080/ReleaseFileCludge/";
                //if (rClient.makePostRequest(new { file_name = newFileNameTextBox.Text }))
                foreach (TabPage page in pages)
                {
                    if (page.AccessibilityObject.Name.Equals("KK Reader"))
                        continue;

                    if (page.Controls.Count <= 1)
                        continue;

                    string fileName = page.AccessibilityObject.Name;
                    rClient.makePostRequest(new { fileName });

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
            this.firstTab.Location = new System.Drawing.Point(4, 22);
            this.firstTab.Margin = new System.Windows.Forms.Padding(2);
            this.firstTab.Name = "firstTab";
            this.firstTab.Padding = new System.Windows.Forms.Padding(2);
            this.firstTab.Size = new System.Drawing.Size(549, 310);
            this.firstTab.TabIndex = 0;
            this.firstTab.Text = "K&K Reader";
            this.firstTab.UseVisualStyleBackColor = true;
            // 
            // newFileNameTextBox
            // 
            this.newFileNameTextBox.Location = new System.Drawing.Point(422, 15);
            this.newFileNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.newFileNameTextBox.Name = "newFileNameTextBox";
            this.newFileNameTextBox.Size = new System.Drawing.Size(112, 20);
            this.newFileNameTextBox.TabIndex = 6;
            this.newFileNameTextBox.Text = "NewFile";
            // 
            // createNewFileButton
            // 
            this.createNewFileButton.Location = new System.Drawing.Point(422, 38);
            this.createNewFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.createNewFileButton.Name = "createNewFileButton";
            this.createNewFileButton.Size = new System.Drawing.Size(111, 19);
            this.createNewFileButton.TabIndex = 4;
            this.createNewFileButton.Text = "Add New File";
            this.createNewFileButton.UseVisualStyleBackColor = true;
            this.createNewFileButton.Click += new System.EventHandler(this.createNewFileButton_Click);
            // 
            // filesList
            // 
            this.filesList.Location = new System.Drawing.Point(0, 0);
            this.filesList.Margin = new System.Windows.Forms.Padding(2);
            this.filesList.Name = "filesList";
            this.filesList.Size = new System.Drawing.Size(408, 318);
            this.filesList.TabIndex = 3;
            this.filesList.UseCompatibleStateImageBehavior = false;
            this.filesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.filesList_Click);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.firstTab);
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Margin = new System.Windows.Forms.Padding(2);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(557, 336);
            this.tabs.TabIndex = 5;
            this.tabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabs_Selecting);
            this.tabs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tabs_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 336);
            this.Controls.Add(this.tabs);
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

