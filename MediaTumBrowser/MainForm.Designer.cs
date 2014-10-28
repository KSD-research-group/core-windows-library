namespace MediaTumBrowser
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCheck = new System.Windows.Forms.Button();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPresharedTag = new System.Windows.Forms.TextBox();
            this.textBoxMediaTumUri = new System.Windows.Forms.TextBox();
            this.labelMediaTumUri = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxNodeId = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonCheck
            // 
            this.buttonCheck.Location = new System.Drawing.Point(98, 114);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(75, 23);
            this.buttonCheck.TabIndex = 0;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(98, 9);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(150, 20);
            this.textBoxUserName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Pre shared Tag";
            // 
            // textBoxPresharedTag
            // 
            this.textBoxPresharedTag.Location = new System.Drawing.Point(98, 35);
            this.textBoxPresharedTag.Name = "textBoxPresharedTag";
            this.textBoxPresharedTag.Size = new System.Drawing.Size(150, 20);
            this.textBoxPresharedTag.TabIndex = 4;
            // 
            // textBoxMediaTumUri
            // 
            this.textBoxMediaTumUri.Location = new System.Drawing.Point(98, 61);
            this.textBoxMediaTumUri.Name = "textBoxMediaTumUri";
            this.textBoxMediaTumUri.Size = new System.Drawing.Size(150, 20);
            this.textBoxMediaTumUri.TabIndex = 6;
            // 
            // labelMediaTumUri
            // 
            this.labelMediaTumUri.AutoSize = true;
            this.labelMediaTumUri.Location = new System.Drawing.Point(12, 64);
            this.labelMediaTumUri.Name = "labelMediaTumUri";
            this.labelMediaTumUri.Size = new System.Drawing.Size(82, 13);
            this.labelMediaTumUri.TabIndex = 5;
            this.labelMediaTumUri.Text = "MediaTUM URI";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Node ID";
            // 
            // comboBoxNodeId
            // 
            this.comboBoxNodeId.FormattingEnabled = true;
            this.comboBoxNodeId.Location = new System.Drawing.Point(98, 87);
            this.comboBoxNodeId.Name = "comboBoxNodeId";
            this.comboBoxNodeId.Size = new System.Drawing.Size(150, 21);
            this.comboBoxNodeId.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 145);
            this.Controls.Add(this.comboBoxNodeId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxMediaTumUri);
            this.Controls.Add(this.labelMediaTumUri);
            this.Controls.Add(this.textBoxPresharedTag);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.buttonCheck);
            this.Name = "MainForm";
            this.Text = "Test MediaTUM Library";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPresharedTag;
        private System.Windows.Forms.TextBox textBoxMediaTumUri;
        private System.Windows.Forms.Label labelMediaTumUri;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxNodeId;
    }
}

