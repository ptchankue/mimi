namespace prjMIMI_2
{
    partial class frmCar
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
            this.components = new System.ComponentModel.Container();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnCall = new System.Windows.Forms.Button();
            this.btnPTT = new System.Windows.Forms.Button();
            this.bnPairing = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.lblFrame = new System.Windows.Forms.Label();
            this.tmrFocus = new System.Windows.Forms.Timer(this.components);
            this.tmrDialogueMonitor = new System.Windows.Forms.Timer(this.components);
            this.tmrNumber = new System.Windows.Forms.Timer(this.components);
            this.txtTask = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnCallIn = new System.Windows.Forms.Button();
            this.btnSMSIn = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(370, 668);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(121, 33);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Next";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnCall
            // 
            this.btnCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCall.Location = new System.Drawing.Point(121, 261);
            this.btnCall.Name = "btnCall";
            this.btnCall.Size = new System.Drawing.Size(56, 46);
            this.btnCall.TabIndex = 30;
            this.btnCall.Text = "Call";
            this.btnCall.UseVisualStyleBackColor = true;
            // 
            // btnPTT
            // 
            this.btnPTT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPTT.Location = new System.Drawing.Point(260, 299);
            this.btnPTT.Name = "btnPTT";
            this.btnPTT.Size = new System.Drawing.Size(56, 46);
            this.btnPTT.TabIndex = 39;
            this.btnPTT.Text = "PTT";
            this.btnPTT.UseVisualStyleBackColor = true;
            this.btnPTT.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btnPTT_KeyUp);
            this.btnPTT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPTT_MouseDown);
            this.btnPTT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnPTT_KeyPress);
            this.btnPTT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPTT_MouseUp);
            this.btnPTT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnPTT_KeyDown);
            // 
            // bnPairing
            // 
            this.bnPairing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnPairing.Location = new System.Drawing.Point(260, 387);
            this.bnPairing.Name = "bnPairing";
            this.bnPairing.Size = new System.Drawing.Size(56, 46);
            this.bnPairing.TabIndex = 38;
            this.bnPairing.Text = "Pair";
            this.bnPairing.UseVisualStyleBackColor = true;
            // 
            // btnRead
            // 
            this.btnRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRead.Location = new System.Drawing.Point(342, 343);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(56, 46);
            this.btnRead.TabIndex = 37;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(178, 343);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(56, 46);
            this.btnSend.TabIndex = 35;
            this.btnSend.Text = "Sms";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // btnReject
            // 
            this.btnReject.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReject.Location = new System.Drawing.Point(409, 261);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(56, 46);
            this.btnReject.TabIndex = 36;
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = true;
            // 
            // lblFrame
            // 
            this.lblFrame.AutoSize = true;
            this.lblFrame.Location = new System.Drawing.Point(12, 565);
            this.lblFrame.Name = "lblFrame";
            this.lblFrame.Size = new System.Drawing.Size(35, 13);
            this.lblFrame.TabIndex = 40;
            this.lblFrame.Text = "label1";
            // 
            // tmrFocus
            // 
            this.tmrFocus.Enabled = true;
            this.tmrFocus.Interval = 1000;
            this.tmrFocus.Tick += new System.EventHandler(this.tmrFocus_Tick);
            // 
            // tmrDialogueMonitor
            // 
            this.tmrDialogueMonitor.Enabled = true;
            this.tmrDialogueMonitor.Tick += new System.EventHandler(this.tmrDialogueMonitor_Tick);
            // 
            // tmrNumber
            // 
            this.tmrNumber.Interval = 4000;
            this.tmrNumber.Tick += new System.EventHandler(this.tmrNumber_Tick);
            // 
            // txtTask
            // 
            this.txtTask.Location = new System.Drawing.Point(12, 581);
            this.txtTask.Multiline = true;
            this.txtTask.Name = "txtTask";
            this.txtTask.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTask.Size = new System.Drawing.Size(555, 81);
            this.txtTask.TabIndex = 41;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(510, 668);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 42;
            this.lblStatus.Text = "label1";
            // 
            // btnCallIn
            // 
            this.btnCallIn.Location = new System.Drawing.Point(12, 668);
            this.btnCallIn.Name = "btnCallIn";
            this.btnCallIn.Size = new System.Drawing.Size(46, 23);
            this.btnCallIn.TabIndex = 43;
            this.btnCallIn.Text = "Call";
            this.btnCallIn.UseVisualStyleBackColor = true;
            // 
            // btnSMSIn
            // 
            this.btnSMSIn.Location = new System.Drawing.Point(64, 668);
            this.btnSMSIn.Name = "btnSMSIn";
            this.btnSMSIn.Size = new System.Drawing.Size(46, 23);
            this.btnSMSIn.TabIndex = 44;
            this.btnSMSIn.Text = "SMS";
            this.btnSMSIn.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(116, 672);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(101, 17);
            this.checkBox1.TabIndex = 45;
            this.checkBox1.Text = "Connect to LCT";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(223, 666);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 23);
            this.button1.TabIndex = 46;
            this.button1.Text = "Distraction level";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::prjMIMI_2.Properties.Resources.steering;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(555, 559);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 707);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnSMSIn);
            this.Controls.Add(this.btnCallIn);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtTask);
            this.Controls.Add(this.lblFrame);
            this.Controls.Add(this.btnPTT);
            this.Controls.Add(this.bnPairing);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnCall);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "M I M I : Multimodal Interface for Mobile Info-communication";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnCall;
        private System.Windows.Forms.Button btnPTT;
        private System.Windows.Forms.Button bnPairing;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Label lblFrame;
        private System.Windows.Forms.Timer tmrFocus;
        private System.Windows.Forms.Timer tmrDialogueMonitor;
        private System.Windows.Forms.Timer tmrNumber;
        private System.Windows.Forms.TextBox txtTask;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnCallIn;
        private System.Windows.Forms.Button btnSMSIn;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
    }
}

