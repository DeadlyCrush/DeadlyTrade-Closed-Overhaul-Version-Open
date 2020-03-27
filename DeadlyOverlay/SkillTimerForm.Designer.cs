namespace POExileDirection
{
    partial class SkillTimerForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbTime = new System.Windows.Forms.Label();
            this.xuiGauge1 = new XanderUI.XUIGauge();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbTime);
            this.panel1.Controls.Add(this.xuiGauge1);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(40, 30);
            this.panel1.TabIndex = 0;
            // 
            // lbTime
            // 
            this.lbTime.BackColor = System.Drawing.Color.Transparent;
            this.lbTime.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTime.ForeColor = System.Drawing.Color.Red;
            this.lbTime.Location = new System.Drawing.Point(10, 21);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(20, 10);
            this.lbTime.TabIndex = 31;
            this.lbTime.Text = "9.9";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xuiGauge1
            // 
            this.xuiGauge1.DialColor = System.Drawing.Color.Transparent;
            this.xuiGauge1.DialThickness = 4;
            this.xuiGauge1.FilledColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(250)))));
            this.xuiGauge1.GaugeStyle = XanderUI.XUIGauge.Style.Flat;
            this.xuiGauge1.Location = new System.Drawing.Point(0, 0);
            this.xuiGauge1.Name = "xuiGauge1";
            this.xuiGauge1.Percentage = 100;
            this.xuiGauge1.Size = new System.Drawing.Size(40, 24);
            this.xuiGauge1.TabIndex = 30;
            this.xuiGauge1.Text = "xuiGauge1";
            this.xuiGauge1.Thickness = 8;
            this.xuiGauge1.UnfilledColor = System.Drawing.Color.Transparent;
            this.xuiGauge1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.XuiGauge1_MouseDown);
            this.xuiGauge1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.XuiGauge1_MouseMove);
            this.xuiGauge1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.XuiGauge1_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // SkillTimerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(40, 32);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SkillTimerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SkillTimerForm_FormClosed);
            this.Load += new System.EventHandler(this.SkillTimerForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private XanderUI.XUIGauge xuiGauge1;
        private System.Windows.Forms.Label lbTime;
    }
}