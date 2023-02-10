namespace GOL_DerrickDomena
{
    partial class OptionsDialogBox
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
            this.timerIntervalLabel = new System.Windows.Forms.Label();
            this.widthUniverseLabel = new System.Windows.Forms.Label();
            this.heightUniverseLabel = new System.Windows.Forms.Label();
            this.numericUpDownTimerInterval = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWidthUniverse = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownHeightUniverse = new System.Windows.Forms.NumericUpDown();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimerInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidthUniverse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeightUniverse)).BeginInit();
            this.SuspendLayout();
            // 
            // timerIntervalLabel
            // 
            this.timerIntervalLabel.AutoSize = true;
            this.timerIntervalLabel.Location = new System.Drawing.Point(75, 42);
            this.timerIntervalLabel.Name = "timerIntervalLabel";
            this.timerIntervalLabel.Size = new System.Drawing.Size(142, 13);
            this.timerIntervalLabel.TabIndex = 0;
            this.timerIntervalLabel.Text = "Timer Interval in Milliseconds";
            // 
            // widthUniverseLabel
            // 
            this.widthUniverseLabel.AutoSize = true;
            this.widthUniverseLabel.Location = new System.Drawing.Point(75, 74);
            this.widthUniverseLabel.Name = "widthUniverseLabel";
            this.widthUniverseLabel.Size = new System.Drawing.Size(128, 13);
            this.widthUniverseLabel.TabIndex = 1;
            this.widthUniverseLabel.Text = "Width of Universe in Cells";
            // 
            // heightUniverseLabel
            // 
            this.heightUniverseLabel.AutoSize = true;
            this.heightUniverseLabel.Location = new System.Drawing.Point(75, 106);
            this.heightUniverseLabel.Name = "heightUniverseLabel";
            this.heightUniverseLabel.Size = new System.Drawing.Size(131, 13);
            this.heightUniverseLabel.TabIndex = 2;
            this.heightUniverseLabel.Text = "Height of Universe in Cells";
            // 
            // numericUpDownTimerInterval
            // 
            this.numericUpDownTimerInterval.Location = new System.Drawing.Point(223, 42);
            this.numericUpDownTimerInterval.Name = "numericUpDownTimerInterval";
            this.numericUpDownTimerInterval.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownTimerInterval.TabIndex = 3;
            // 
            // numericUpDownWidthUniverse
            // 
            this.numericUpDownWidthUniverse.Location = new System.Drawing.Point(223, 74);
            this.numericUpDownWidthUniverse.Name = "numericUpDownWidthUniverse";
            this.numericUpDownWidthUniverse.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownWidthUniverse.TabIndex = 4;
            // 
            // numericUpDownHeightUniverse
            // 
            this.numericUpDownHeightUniverse.Location = new System.Drawing.Point(223, 106);
            this.numericUpDownHeightUniverse.Name = "numericUpDownHeightUniverse";
            this.numericUpDownHeightUniverse.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownHeightUniverse.TabIndex = 5;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(101, 189);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(182, 189);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // OptionsDialogBox
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(359, 224);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.numericUpDownHeightUniverse);
            this.Controls.Add(this.numericUpDownWidthUniverse);
            this.Controls.Add(this.numericUpDownTimerInterval);
            this.Controls.Add(this.heightUniverseLabel);
            this.Controls.Add(this.widthUniverseLabel);
            this.Controls.Add(this.timerIntervalLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialogBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OptionsDialogBox";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimerInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidthUniverse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeightUniverse)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timerIntervalLabel;
        private System.Windows.Forms.Label widthUniverseLabel;
        private System.Windows.Forms.Label heightUniverseLabel;
        private System.Windows.Forms.NumericUpDown numericUpDownTimerInterval;
        private System.Windows.Forms.NumericUpDown numericUpDownWidthUniverse;
        private System.Windows.Forms.NumericUpDown numericUpDownHeightUniverse;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}