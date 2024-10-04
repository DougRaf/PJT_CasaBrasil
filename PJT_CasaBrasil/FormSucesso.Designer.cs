namespace PJT_CasaBrasil
{
    partial class FormSucesso
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSucesso));
            this.buttonFechar = new System.Windows.Forms.Button();
            this.labelMensagem = new System.Windows.Forms.Label();
            this.pictureBoxSucesso = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSucesso)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonFechar
            // 
            this.buttonFechar.Location = new System.Drawing.Point(385, 65);
            this.buttonFechar.Name = "buttonFechar";
            this.buttonFechar.Size = new System.Drawing.Size(75, 23);
            this.buttonFechar.TabIndex = 5;
            this.buttonFechar.Text = "Fechar";
            this.buttonFechar.UseVisualStyleBackColor = true;
            this.buttonFechar.Click += new System.EventHandler(this.buttonFechar_Click);
            // 
            // labelMensagem
            // 
            this.labelMensagem.AutoSize = true;
            this.labelMensagem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMensagem.Location = new System.Drawing.Point(98, 24);
            this.labelMensagem.Name = "labelMensagem";
            this.labelMensagem.Size = new System.Drawing.Size(362, 25);
            this.labelMensagem.TabIndex = 4;
            this.labelMensagem.Text = "Cadastro realizado com sucesso!";
            // 
            // pictureBoxSucesso
            // 
            this.pictureBoxSucesso.Image = global::PJT_CasaBrasil.Properties.Resources.casabrasil;
            this.pictureBoxSucesso.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxSucesso.Name = "pictureBoxSucesso";
            this.pictureBoxSucesso.Size = new System.Drawing.Size(80, 77);
            this.pictureBoxSucesso.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSucesso.TabIndex = 3;
            this.pictureBoxSucesso.TabStop = false;
            // 
            // FormSucesso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 97);
            this.ControlBox = false;
            this.Controls.Add(this.buttonFechar);
            this.Controls.Add(this.labelMensagem);
            this.Controls.Add(this.pictureBoxSucesso);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSucesso";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Casa Brasil em Komatsu Ishikawa-Ken / Japão";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSucesso)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFechar;
        private System.Windows.Forms.Label labelMensagem;
        private System.Windows.Forms.PictureBox pictureBoxSucesso;
    }
}