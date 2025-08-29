namespace DiscountCodeClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            GenerateCodeBtn = new Button();
            DiscountLbl = new Label();
            UseCodeBtn = new Button();
            UsedCodeLbl = new Label();
            SuspendLayout();
            // 
            // GenerateCodeBtn
            // 
            GenerateCodeBtn.Location = new Point(278, 150);
            GenerateCodeBtn.Name = "GenerateCodeBtn";
            GenerateCodeBtn.Size = new Size(250, 82);
            GenerateCodeBtn.TabIndex = 0;
            GenerateCodeBtn.Text = "Generate Code";
            GenerateCodeBtn.UseVisualStyleBackColor = true;
            GenerateCodeBtn.Click += GenerateCodeBtn_Click;
            // 
            // DiscountLbl
            // 
            DiscountLbl.AutoSize = true;
            DiscountLbl.Font = new Font("Verdana", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DiscountLbl.Location = new Point(318, 80);
            DiscountLbl.Name = "DiscountLbl";
            DiscountLbl.Size = new Size(113, 38);
            DiscountLbl.TabIndex = 1;
            DiscountLbl.Text = "label1";
            DiscountLbl.Visible = false;
            // 
            // UseCodeBtn
            // 
            UseCodeBtn.Location = new Point(278, 345);
            UseCodeBtn.Name = "UseCodeBtn";
            UseCodeBtn.Size = new Size(250, 82);
            UseCodeBtn.TabIndex = 2;
            UseCodeBtn.Text = "Use code!";
            UseCodeBtn.UseVisualStyleBackColor = true;
            UseCodeBtn.Visible = false;
            UseCodeBtn.Click += UseCodeBtn_Click;
            // 
            // UsedCodeLbl
            // 
            UsedCodeLbl.AutoSize = true;
            UsedCodeLbl.Font = new Font("Verdana", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            UsedCodeLbl.Location = new Point(183, 271);
            UsedCodeLbl.Name = "UsedCodeLbl";
            UsedCodeLbl.Size = new Size(113, 38);
            UsedCodeLbl.TabIndex = 3;
            UsedCodeLbl.Text = "label1";
            UsedCodeLbl.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(UsedCodeLbl);
            Controls.Add(UseCodeBtn);
            Controls.Add(DiscountLbl);
            Controls.Add(GenerateCodeBtn);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button GenerateCodeBtn;
        private Label DiscountLbl;
        private Button UseCodeBtn;
        private Label UsedCodeLbl;
    }
}
