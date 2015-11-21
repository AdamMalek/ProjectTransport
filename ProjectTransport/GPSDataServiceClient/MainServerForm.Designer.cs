namespace GPSDataServiceClient
{
    partial class MainServerForm
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Wymagana metoda obsługi projektanta — nie należy modyfikować 
        /// zawartość tej metody z edytorem kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnSendRequest = new System.Windows.Forms.Button();
            this.txtRequestParam = new System.Windows.Forms.TextBox();
            this.lblServerResponse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Request parameter:";
            // 
            // btnSendRequest
            // 
            this.btnSendRequest.Location = new System.Drawing.Point(12, 35);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new System.Drawing.Size(452, 43);
            this.btnSendRequest.TabIndex = 1;
            this.btnSendRequest.Text = "Send Request";
            this.btnSendRequest.UseVisualStyleBackColor = true;
            this.btnSendRequest.Click += new System.EventHandler(this.btnSendRequest_Click);
            // 
            // txtRequestParam
            // 
            this.txtRequestParam.Location = new System.Drawing.Point(118, 6);
            this.txtRequestParam.Name = "txtRequestParam";
            this.txtRequestParam.Size = new System.Drawing.Size(346, 20);
            this.txtRequestParam.TabIndex = 2;
            // 
            // lblServerResponse
            // 
            this.lblServerResponse.AutoSize = true;
            this.lblServerResponse.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblServerResponse.Location = new System.Drawing.Point(181, 92);
            this.lblServerResponse.Name = "lblServerResponse";
            this.lblServerResponse.Size = new System.Drawing.Size(90, 31);
            this.lblServerResponse.TabIndex = 3;
            this.lblServerResponse.Text = "Hello!";
            // 
            // MainServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 142);
            this.Controls.Add(this.lblServerResponse);
            this.Controls.Add(this.txtRequestParam);
            this.Controls.Add(this.btnSendRequest);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainServerForm";
            this.Text = "Service Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSendRequest;
        private System.Windows.Forms.TextBox txtRequestParam;
        private System.Windows.Forms.Label lblServerResponse;
    }
}

