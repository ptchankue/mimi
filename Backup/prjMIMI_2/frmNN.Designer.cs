namespace prjMIMI_2
{
    partial class frmNN
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
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLevel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDSpeed = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDAngle = new System.Windows.Forms.TextBox();
            this.btnLoadwght = new System.Windows.Forms.Button();
            this.btnTesting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(28, 12);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(75, 52);
            this.btnTrain.TabIndex = 0;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(326, 182);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 34);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(263, 94);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(75, 20);
            this.txtSpeed.TabIndex = 2;
            this.txtSpeed.Text = "76";
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(263, 143);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(75, 20);
            this.txtAngle.TabIndex = 3;
            this.txtAngle.Text = "14";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(222, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Speed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Angle";
            // 
            // txtLevel
            // 
            this.txtLevel.Location = new System.Drawing.Point(316, 233);
            this.txtLevel.Name = "txtLevel";
            this.txtLevel.Size = new System.Drawing.Size(100, 20);
            this.txtLevel.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(231, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Distraction level";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(348, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Speed\'";
            // 
            // txtDSpeed
            // 
            this.txtDSpeed.Location = new System.Drawing.Point(389, 91);
            this.txtDSpeed.Name = "txtDSpeed";
            this.txtDSpeed.Size = new System.Drawing.Size(75, 20);
            this.txtDSpeed.TabIndex = 8;
            this.txtDSpeed.Text = "3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(348, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Angle\'";
            // 
            // txtDAngle
            // 
            this.txtDAngle.Location = new System.Drawing.Point(389, 143);
            this.txtDAngle.Name = "txtDAngle";
            this.txtDAngle.Size = new System.Drawing.Size(75, 20);
            this.txtDAngle.TabIndex = 10;
            this.txtDAngle.Text = "39";
            // 
            // btnLoadwght
            // 
            this.btnLoadwght.Location = new System.Drawing.Point(28, 117);
            this.btnLoadwght.Name = "btnLoadwght";
            this.btnLoadwght.Size = new System.Drawing.Size(75, 52);
            this.btnLoadwght.TabIndex = 12;
            this.btnLoadwght.Text = "Load weights";
            this.btnLoadwght.UseVisualStyleBackColor = true;
            this.btnLoadwght.Click += new System.EventHandler(this.btnLoadwght_Click);
            // 
            // btnTesting
            // 
            this.btnTesting.Location = new System.Drawing.Point(155, 12);
            this.btnTesting.Name = "btnTesting";
            this.btnTesting.Size = new System.Drawing.Size(75, 52);
            this.btnTesting.TabIndex = 13;
            this.btnTesting.Text = "Testing";
            this.btnTesting.UseVisualStyleBackColor = true;
            // 
            // frmNN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 282);
            this.Controls.Add(this.btnTesting);
            this.Controls.Add(this.btnLoadwght);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDAngle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDSpeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLevel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.txtSpeed);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnTrain);
            this.Name = "frmNN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmNN";
            this.Load += new System.EventHandler(this.frmNN_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDSpeed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDAngle;
        private System.Windows.Forms.Button btnLoadwght;
        private System.Windows.Forms.Button btnTesting;
    }
}