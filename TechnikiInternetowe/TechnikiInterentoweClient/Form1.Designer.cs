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
            this.txtRestRequestURL = new System.Windows.Forms.TextBox();
            this.txtRestResponse = new System.Windows.Forms.TextBox();
            this.btnSendReq = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtRestRequestURL
            // 
            this.txtRestRequestURL.AccessibleName = "txtRestRequestURL";
            this.txtRestRequestURL.Location = new System.Drawing.Point(212, 12);
            this.txtRestRequestURL.Name = "txtRestRequestURL";
            this.txtRestRequestURL.Size = new System.Drawing.Size(216, 20);
            this.txtRestRequestURL.TabIndex = 0;
            // 
            // txtRestResponse
            // 
            this.txtRestResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRestResponse.Location = new System.Drawing.Point(212, 74);
            this.txtRestResponse.Multiline = true;
            this.txtRestResponse.Name = "txtRestResponse";
            this.txtRestResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRestResponse.Size = new System.Drawing.Size(291, 177);
            this.txtRestResponse.TabIndex = 1;
            // 
            // btnSendReq
            // 
            this.btnSendReq.Location = new System.Drawing.Point(449, 9);
            this.btnSendReq.Name = "btnSendReq";
            this.btnSendReq.Size = new System.Drawing.Size(75, 23);
            this.btnSendReq.TabIndex = 2;
            this.btnSendReq.Text = "Send";
            this.btnSendReq.UseVisualStyleBackColor = true;
            this.btnSendReq.Click += new System.EventHandler(this.btnSendReq_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Your Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Response:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 334);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSendReq);
            this.Controls.Add(this.txtRestResponse);
            this.Controls.Add(this.txtRestRequestURL);
            this.Name = "Form1";
            this.Text = "c# Rest Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRestRequestURL;
        private System.Windows.Forms.TextBox txtRestResponse;
        private System.Windows.Forms.Button btnSendReq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

