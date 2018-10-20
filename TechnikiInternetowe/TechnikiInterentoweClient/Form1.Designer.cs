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
            this.txtRestRequestURL = new System.Windows.Forms.TextBox();
            this.txtRestResponse = new System.Windows.Forms.TextBox();
            this.btnSendReq = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.filesList = new System.Windows.Forms.ListView();
            this.firstTab.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // firstTab
            // 
            this.firstTab.Controls.Add(this.filesList);
            this.firstTab.Controls.Add(this.btnSendReq);
            this.firstTab.Controls.Add(this.txtRestResponse);
            this.firstTab.Controls.Add(this.txtRestRequestURL);
            this.firstTab.Location = new System.Drawing.Point(4, 25);
            this.firstTab.Name = "firstTab";
            this.firstTab.Padding = new System.Windows.Forms.Padding(3);
            this.firstTab.Size = new System.Drawing.Size(734, 390);
            this.firstTab.TabIndex = 0;
            this.firstTab.Text = "K&K Reader";
            this.firstTab.UseVisualStyleBackColor = true;
            // 
            // txtRestRequestURL
            // 
            this.txtRestRequestURL.AccessibleName = "txtRestRequestURL";
            this.txtRestRequestURL.Location = new System.Drawing.Point(470, 21);
            this.txtRestRequestURL.Margin = new System.Windows.Forms.Padding(4);
            this.txtRestRequestURL.Name = "txtRestRequestURL";
            this.txtRestRequestURL.Size = new System.Drawing.Size(87, 22);
            this.txtRestRequestURL.TabIndex = 0;
            this.txtRestRequestURL.TextChanged += new System.EventHandler(this.txtRestRequestURL_TextChanged);
            // 
            // txtRestResponse
            // 
            this.txtRestResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRestResponse.Location = new System.Drawing.Point(491, 54);
            this.txtRestResponse.Margin = new System.Windows.Forms.Padding(4);
            this.txtRestResponse.Multiline = true;
            this.txtRestResponse.Name = "txtRestResponse";
            this.txtRestResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRestResponse.Size = new System.Drawing.Size(210, 294);
            this.txtRestResponse.TabIndex = 1;
            this.txtRestResponse.TextChanged += new System.EventHandler(this.txtRestResponse_TextChanged);
            // 
            // btnSendReq
            // 
            this.btnSendReq.Location = new System.Drawing.Point(601, 18);
            this.btnSendReq.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendReq.Name = "btnSendReq";
            this.btnSendReq.Size = new System.Drawing.Size(100, 28);
            this.btnSendReq.TabIndex = 2;
            this.btnSendReq.Text = "Send";
            this.btnSendReq.UseVisualStyleBackColor = true;
            this.btnSendReq.Click += new System.EventHandler(this.btnSendReq_Click);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.firstTab);
            this.tabs.Location = new System.Drawing.Point(0, -1);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(742, 419);
            this.tabs.TabIndex = 5;
            // 
            // filesList
            // 
            this.filesList.Location = new System.Drawing.Point(0, 0);
            this.filesList.Name = "filesList";
            this.filesList.Size = new System.Drawing.Size(463, 390);
            this.filesList.TabIndex = 3;
            this.filesList.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 411);
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
        private System.Windows.Forms.Button btnSendReq;
        private System.Windows.Forms.TextBox txtRestResponse;
        private System.Windows.Forms.TextBox txtRestRequestURL;
        private System.Windows.Forms.TabControl tabs;
    }
}

