namespace microphone
{
    partial class Note2FretUi
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
            this.pulseCodeModulationChart = new ScottPlot.ScottPlotUC();
            this.fastFourierTransformationChart = new ScottPlot.ScottPlotUC();
            this.captureBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.frequencyLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pulseCodeModulationChart
            // 
            this.pulseCodeModulationChart.Location = new System.Drawing.Point(9, 10);
            this.pulseCodeModulationChart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pulseCodeModulationChart.Name = "pulseCodeModulationChart";
            this.pulseCodeModulationChart.Size = new System.Drawing.Size(831, 327);
            this.pulseCodeModulationChart.TabIndex = 4;
            // 
            // fastFourierTransformationChart
            // 
            this.fastFourierTransformationChart.Location = new System.Drawing.Point(9, 341);
            this.fastFourierTransformationChart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.fastFourierTransformationChart.Name = "fastFourierTransformationChart";
            this.fastFourierTransformationChart.Size = new System.Drawing.Size(831, 312);
            this.fastFourierTransformationChart.TabIndex = 5;
            // 
            // captureBtn
            // 
            this.captureBtn.Location = new System.Drawing.Point(945, 33);
            this.captureBtn.Margin = new System.Windows.Forms.Padding(2);
            this.captureBtn.Name = "captureBtn";
            this.captureBtn.Size = new System.Drawing.Size(100, 40);
            this.captureBtn.TabIndex = 6;
            this.captureBtn.Text = "Start capturing";
            this.captureBtn.UseVisualStyleBackColor = true;
            this.captureBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frequencyLbl
            // 
            this.frequencyLbl.AutoSize = true;
            this.frequencyLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frequencyLbl.Location = new System.Drawing.Point(856, 101);
            this.frequencyLbl.Name = "frequencyLbl";
            this.frequencyLbl.Size = new System.Drawing.Size(166, 26);
            this.frequencyLbl.TabIndex = 7;
            this.frequencyLbl.Text = "Frequency: - Hz";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 663);
            this.Controls.Add(this.frequencyLbl);
            this.Controls.Add(this.captureBtn);
            this.Controls.Add(this.fastFourierTransformationChart);
            this.Controls.Add(this.pulseCodeModulationChart);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ScottPlot.ScottPlotUC pulseCodeModulationChart;
        private ScottPlot.ScottPlotUC fastFourierTransformationChart;
        private System.Windows.Forms.Button captureBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label frequencyLbl;
    }
}

