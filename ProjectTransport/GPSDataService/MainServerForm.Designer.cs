namespace GPSDataService
{
    partial class ServiceForm
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
            this.btnStartService = new System.Windows.Forms.Button();
            this.btnStopService = new System.Windows.Forms.Button();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.pnStatus = new System.Windows.Forms.Panel();
            this.btnTestWrite = new System.Windows.Forms.Button();
            this.btnTestRead = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartService
            // 
            this.btnStartService.Location = new System.Drawing.Point(12, 38);
            this.btnStartService.Name = "btnStartService";
            this.btnStartService.Size = new System.Drawing.Size(656, 48);
            this.btnStartService.TabIndex = 1;
            this.btnStartService.Text = "Start Service";
            this.btnStartService.UseVisualStyleBackColor = true;
            this.btnStartService.Click += new System.EventHandler(this.btnStartService_Click);
            // 
            // btnStopService
            // 
            this.btnStopService.Location = new System.Drawing.Point(12, 92);
            this.btnStopService.Name = "btnStopService";
            this.btnStopService.Size = new System.Drawing.Size(656, 48);
            this.btnStopService.TabIndex = 2;
            this.btnStopService.Text = "Stop Service";
            this.btnStopService.UseVisualStyleBackColor = true;
            this.btnStopService.Click += new System.EventHandler(this.btnStopService_Click);
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.HorizontalScrollbar = true;
            this.lbLog.Location = new System.Drawing.Point(12, 289);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(656, 134);
            this.lbLog.TabIndex = 3;
            // 
            // pnStatus
            // 
            this.pnStatus.BackColor = System.Drawing.Color.Red;
            this.pnStatus.Location = new System.Drawing.Point(12, 12);
            this.pnStatus.Name = "pnStatus";
            this.pnStatus.Size = new System.Drawing.Size(656, 20);
            this.pnStatus.TabIndex = 4;
            // 
            // btnTestWrite
            // 
            this.btnTestWrite.Location = new System.Drawing.Point(12, 162);
            this.btnTestWrite.Name = "btnTestWrite";
            this.btnTestWrite.Size = new System.Drawing.Size(656, 48);
            this.btnTestWrite.TabIndex = 5;
            this.btnTestWrite.Text = "Test writing to database";
            this.btnTestWrite.UseVisualStyleBackColor = true;
            this.btnTestWrite.Click += new System.EventHandler(this.btnTestWrite_Click);
            // 
            // btnTestRead
            // 
            this.btnTestRead.Location = new System.Drawing.Point(12, 216);
            this.btnTestRead.Name = "btnTestRead";
            this.btnTestRead.Size = new System.Drawing.Size(656, 48);
            this.btnTestRead.TabIndex = 6;
            this.btnTestRead.Text = "Test reading from database";
            this.btnTestRead.UseVisualStyleBackColor = true;
            this.btnTestRead.Click += new System.EventHandler(this.btnTestRead_Click);
            // 
            // ServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(676, 431);
            this.Controls.Add(this.btnTestRead);
            this.Controls.Add(this.btnTestWrite);
            this.Controls.Add(this.pnStatus);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.btnStopService);
            this.Controls.Add(this.btnStartService);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ServiceForm";
            this.Text = "Service Not Active";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartService;
        private System.Windows.Forms.Button btnStopService;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Panel pnStatus;
        private System.Windows.Forms.Button btnTestWrite;
        private System.Windows.Forms.Button btnTestRead;
    }
}

